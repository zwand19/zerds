using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;

namespace Zerds.GameObjects
{
    public class Map
    {
        private const int MapNumSectionsWide = 7; // need to account for the fact that there's an outer wall around the map too
        private const int MapNumSectionsTall = 7;
        private int _mapSectionWidth;
        private int _mapSectionHeight;
        private Dictionary<TileTypes, Rectangle> _textureLocations;
        private Texture2D _texture;
        private MapSection[,] _sections;

        /// <summary>
        /// The starting Zerd location on the map
        /// </summary>
        public Vector2 StartingPosition;

        public Map(GraphicsDevice graphicsDevice, MapTypes type, Rectangle clientBounds)
        {
            switch (type)
            {
                case MapTypes.Dungeon:
                    InitializeMap(graphicsDevice, clientBounds);
                    return;
                default:
                    throw new ArgumentException("Map type not handled");
            }
        }

        public void InitializeMap(GraphicsDevice graphicsDevice, Rectangle clientBounds, MapSectionTypes? type = null)
        {
            _texture = TextureCacheFactory.GetOnce("Maps/Tiles.png");
            _textureLocations = new Dictionary<TileTypes, Rectangle> {
                [TileTypes.Floor] = new Rectangle(128, 256, 64, 64),
                [TileTypes.Wall] = new Rectangle(0, 32, 64, 64),
                [TileTypes.SingleWall] = new Rectangle(320, 32, 64, 64)
            };
            _mapSectionWidth = _textureLocations[TileTypes.Floor].Width * GameConstants.MapSectionSizeInTiles;
            _mapSectionHeight = _textureLocations[TileTypes.Floor].Height * GameConstants.MapSectionSizeInTiles;

            _sections = new MapSection[MapNumSectionsWide, MapNumSectionsTall];
            if (type.HasValue)
            {
                for (var x = 0; x < MapNumSectionsWide; x++)
                    for (var y = 0; y < MapNumSectionsTall; y++)
                        _sections[x, y] = new MapSection(type.Value, new Rectangle(_mapSectionWidth * x, _mapSectionHeight * y, _mapSectionWidth, _mapSectionHeight));
            }
            else
            {
                // First surround the map in wall sections
                for (var x = 0; x < MapNumSectionsWide; x++)
                {
                    _sections[x, 0] = new MapSection(MapSectionTypes.Wall, new Rectangle(_mapSectionWidth * x, _mapSectionHeight * 0, _mapSectionWidth, _mapSectionHeight));
                    _sections[x, MapNumSectionsTall - 1] = new MapSection(MapSectionTypes.Wall, new Rectangle(_mapSectionWidth * x, _mapSectionHeight * (MapNumSectionsTall - 1), _mapSectionWidth, _mapSectionHeight));
                }
                for (var y = 1; y < MapNumSectionsTall - 1; y++)
                {
                    _sections[0, y] = new MapSection(MapSectionTypes.Wall, new Rectangle(_mapSectionWidth * 0, _mapSectionHeight * y, _mapSectionWidth, _mapSectionHeight));
                    _sections[MapNumSectionsWide - 1, y] = new MapSection(MapSectionTypes.Wall, new Rectangle(_mapSectionWidth * (MapNumSectionsWide - 1), _mapSectionHeight * y, _mapSectionWidth, _mapSectionHeight));
                }
                // Create the start
                var startingSectionX = Helpers.RandomIntInRange(1, MapNumSectionsWide - 2);
                var startingSectionY = Helpers.RandomIntInRange(1, MapNumSectionsWide - 2);
                StartingPosition = new Vector2(startingSectionX * _mapSectionWidth + _mapSectionHeight / 2, startingSectionY * _mapSectionHeight + _mapSectionHeight / 2);
                _sections[startingSectionX, startingSectionY] = new MapSection(MapSectionTypes.Start, new Rectangle(_mapSectionWidth * startingSectionX, _mapSectionHeight * startingSectionY, _mapSectionWidth, _mapSectionHeight));
                // Create all of the other middle tiles
                for (var x = 1; x < MapNumSectionsWide - 1; x++)
                {
                    for (var y = 1; y < MapNumSectionsTall - 1; y++)
                    {
                        if (_sections[x,y] == null)
                            _sections[x, y] = new MapSection(MapSectionTypes.Walled, new Rectangle(_mapSectionWidth * x, _mapSectionHeight * y, _mapSectionWidth, _mapSectionHeight));
                    }
                }
                // Create a maze
                var numVisited = 1;
                var stack = new List<(int, int)>();
                (int X, int Y) currentSection = (startingSectionX, startingSectionY);
                while (numVisited < (MapNumSectionsTall - 2) * (MapNumSectionsWide - 2))
                {
                    var adjacentCells = new List<(int X, int Y, CardinalDirection Direction)>();

                    if (currentSection.X > 1 && _sections[currentSection.X - 1, currentSection.Y].ClosedIn)
                        adjacentCells.Add((currentSection.X - 1, currentSection.Y, CardinalDirection.Left));

                    if (currentSection.X < MapNumSectionsWide - 2 && _sections[currentSection.X + 1, currentSection.Y].ClosedIn)
                        adjacentCells.Add((currentSection.X + 1, currentSection.Y, CardinalDirection.Right));

                    if (currentSection.Y > 1 && _sections[currentSection.X, currentSection.Y - 1].ClosedIn)
                        adjacentCells.Add((currentSection.X, currentSection.Y - 1, CardinalDirection.Up));

                    if (currentSection.Y < MapNumSectionsTall - 2 && _sections[currentSection.X, currentSection.Y + 1].ClosedIn)
                        adjacentCells.Add((currentSection.X, currentSection.Y + 1, CardinalDirection.Down));

                    if (adjacentCells.Any())
                    {
                        var randomNeighbor = adjacentCells[Globals.Random.Next(adjacentCells.Count)];
                        switch (randomNeighbor.Direction)
                        {
                            case CardinalDirection.Up:
                                _sections[randomNeighbor.X, randomNeighbor.Y].DestroyWall(CardinalDirection.Down);
                                _sections[currentSection.X, currentSection.Y].DestroyWall(CardinalDirection.Up);
                                break;
                            case CardinalDirection.Down:
                                _sections[randomNeighbor.X, randomNeighbor.Y].DestroyWall(CardinalDirection.Up);
                                _sections[currentSection.X, currentSection.Y].DestroyWall(CardinalDirection.Down);
                                break;
                            case CardinalDirection.Left:
                                _sections[randomNeighbor.X, randomNeighbor.Y].DestroyWall(CardinalDirection.Right);
                                _sections[currentSection.X, currentSection.Y].DestroyWall(CardinalDirection.Left);
                                break;
                            case CardinalDirection.Right:
                                _sections[randomNeighbor.X, randomNeighbor.Y].DestroyWall(CardinalDirection.Left);
                                _sections[currentSection.X, currentSection.Y].DestroyWall(CardinalDirection.Right);
                                break;
                        }
                        stack.Add((currentSection.X, currentSection.Y));
                        currentSection = (randomNeighbor.X, randomNeighbor.Y);
                        numVisited++;
                    }
                    else
                    {
                        // Set current section to the popped section from the stack
                        currentSection = stack.Last();
                        stack.Remove(currentSection);
                    }
                }
            }

            // Tell the sections we are done making the map
            for (var x = 0; x < MapNumSectionsWide; x++)
                for (var y = 0; y < MapNumSectionsTall; y++)
                    _sections[x, y].MapComplete();
        }

        public void StartingGame()
        {
            for (var x = 0; x < _sections.GetLength(0); x++)
                for (var y = 0; y < _sections.GetLength(1); y++)
                    _sections[x, y].StartingGame();
        }

        /// <summary>
        /// Get the MapSection that a being is currently in
        /// </summary>
        /// <param name="being"></param>
        /// <returns></returns>
        public MapSection GetSection(Being being)
        {
            var x = (int)Math.Floor(being.X / _mapSectionWidth);
            var y = (int)Math.Floor(being.Y / _mapSectionHeight);
            return _sections[x, y];
        }

        public void Draw()
        {
            // NOTE: if we want to use zoom then we need to recalculate these every step
            //_mapSectionWidth = _textureLocations[TileTypes.Floor].Width * GameConstants.MapSectionSizeInTiles;
            //_mapSectionHeight = _textureLocations[TileTypes.Floor].Height * GameConstants.MapSectionSizeInTiles;
            for (var x = 0; x < _sections.GetLength(0); x++)
            {
                for (var y = 0; y < _sections.GetLength(1); y++)
                {
                    // If none of the tiles are possibly on screen don't draw
                    if (!((x + 1) * _mapSectionWidth < Globals.Camera.LeftDrawBound || x * _mapSectionWidth > Globals.Camera.RightDrawBound || (y + 1) * _mapSectionHeight < Globals.Camera.TopDrawBound || y * _mapSectionHeight > Globals.Camera.BottomDrawBound))
                        _sections[x, y].Draw(_textureLocations, _texture);
                }
            }
        }

        /// <summary>
        /// Gets all map sections that intersect with a given rectangle
        /// </summary>
        public List<MapSection> GetMapSections(Rectangle rectangle)
        {
            var minX = rectangle.Left / _mapSectionWidth;
            var maxX = rectangle.Right / _mapSectionWidth;
            var minY = rectangle.Top / _mapSectionHeight;
            var maxY = rectangle.Bottom / _mapSectionHeight;
            var sections = new List<MapSection>();
            for (var i = minX; i <= maxX; i++)
                for (var j = minY; j <= maxY; j++)
                    sections.Add(_sections[i, j]);
            return sections;
        }

        /// <summary>
        /// Gets all map sections that intersect with the hitbox of a given entity
        /// </summary>
        public bool CollidesWithWall(Entity entity)
        {
            // TODO: is this okay?
            var hitbox = entity.Hitbox().First();
            return CollidesWithWall(hitbox);
        }

        /// <summary>
        /// Gets all map sections that intersect with a given rectangle
        /// </summary>
        public bool CollidesWithWall(Rectangle hitbox)
        {
            foreach (var section in GetMapSections(hitbox))
            {
                if (section.HasCollidingWallTile(hitbox))
                    return true;
            }
            return false;
        }
    }
}
