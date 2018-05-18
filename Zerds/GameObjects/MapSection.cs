using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Enums;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;
using System.Linq;
using Zerds.Factories;

namespace Zerds.GameObjects
{
    public class MapSection
    {
        public const float OpenTileChance = 0.15f;
        public const float OneWalledChance = 0.25f;
        public const float TwoParallelWallChance = 0.6f;
        public const float CornerWallChance = 0.85f;

        private const float RandomWallChance = 0.012f;

        public TileTypes[,] Tiles;

        // Flags determining whether a certain side of the section is open (not walled off)
        public bool HasOpenLeft { get; private set; }
        public bool HasOpenTop { get; private set; }
        public bool HasOpenRight { get; private set; }
        public bool HasOpenBottom { get; private set; }
        public bool ClosedIn => !HasOpenBottom && !HasOpenLeft && !HasOpenRight && !HasOpenTop;

        public List<Rectangle> Walls { get; private set; }

        public Point Center => _bounds.Center;

        private Rectangle _bounds;
        private MapSectionTypes _type;

        /// <summary>
        /// Create a Map Section all containing one type of tile.
        /// </summary>
        /// 
        public MapSection(MapSectionTypes type, Rectangle bounds)
        {
            _bounds = bounds;
            _type = type;
            Tiles = new TileTypes[GameConstants.MapSectionSizeInTiles, GameConstants.MapSectionSizeInTiles];

            switch (type)
            {
                case MapSectionTypes.Start:
                    HasOpenTop = true;
                    HasOpenRight = true;
                    HasOpenLeft = true;
                    HasOpenBottom = true;
                    for (var x = 0; x < Tiles.GetLength(0); x++)
                        for (var y = 0; y < Tiles.GetLength(1); y++)
                            Tiles[x, y] = TileTypes.Floor;
                    Tiles[8, 8] = TileTypes.SingleWall;
                    Tiles[Tiles.GetLength(0) - 7, 8] = TileTypes.SingleWall;
                    Tiles[8, Tiles.GetLength(1) - 7] = TileTypes.SingleWall;
                    Tiles[Tiles.GetLength(0) - 7, Tiles.GetLength(1) - 7] = TileTypes.SingleWall;
                    break;
                case MapSectionTypes.Wall:
                    for (var x = 0; x < Tiles.GetLength(0); x++)
                        for (var y = 0; y < Tiles.GetLength(1); y++)
                            Tiles[x, y] = TileTypes.Wall;
                    break;
                case MapSectionTypes.Floor:
                    HasOpenTop = true;
                    HasOpenRight = true;
                    HasOpenLeft = true;
                    HasOpenBottom = true;
                    for (var x = 0; x < Tiles.GetLength(0); x++)
                        for (var y = 0; y < Tiles.GetLength(1); y++)
                            Tiles[x, y] = TileTypes.Floor;
                    break;
                case MapSectionTypes.Walled:
                    for (var x = 0; x < Tiles.GetLength(0); x++)
                    {
                        for (var y = 0; y < Tiles.GetLength(1); y++)
                        {
                            if (x == 0 || x == Tiles.GetLength(0) - 1 || y == 0 || y == Tiles.GetLength(1) - 1 || Globals.Random.NextDouble() < RandomWallChance)
                                Tiles[x, y] = TileTypes.Wall;
                            else
                                Tiles[x, y] = TileTypes.Floor;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Call once the map is done changing so we can set some static data.
        /// </summary>
        public void MapComplete()
        {
            Walls = new List<Rectangle>();
            for (var x = 0; x < Tiles.GetLength(0); x++)
                for (var y = 0; y < Tiles.GetLength(1); y++)
                    if (TileIsWall(x, y))
                        Walls.Add(GetTileRectangle(x, y));
        }

        /// <summary>
        /// Called once the game is starting so we can create some enemies.
        /// </summary>
        public void StartingGame()
        {
            if (_type == MapSectionTypes.Floor || _type == MapSectionTypes.Walled)
                EnemyCreatorFactory.CreateMapSectionEnemies(this);
        }

        #region Open Sides Methods
        public void OpenTop()
        {
            if (HasOpenTop)
                return;

            HasOpenTop = true;
            var startX = HasOpenLeft ? 0 : 1;
            var endX = HasOpenRight ? Tiles.GetLength(0) - 1 : Tiles.GetLength(0) - 2;
            for (var x = 0; x <= endX; x++)
                Tiles[x, 0] = TileTypes.Floor;
        }

        public void OpenBottom()
        {
            if (HasOpenBottom)
                return;

            HasOpenBottom = true;
            var startX = HasOpenLeft ? 0 : 1;
            var endX = HasOpenRight ? Tiles.GetLength(0) - 1 : Tiles.GetLength(0) - 2;
            for (var x = 0; x <= endX; x++)
                Tiles[x, Tiles.GetLength(1) - 1] = TileTypes.Floor;
        }

        public void OpenLeft()
        {
            if (HasOpenLeft)
                return;

            HasOpenLeft = true;
            var startY = HasOpenTop ? 0 : 1;
            var endY = HasOpenBottom ? Tiles.GetLength(1) - 1 : Tiles.GetLength(1) - 2;
            for (var y = 0; y <= endY; y++)
                Tiles[0, y] = TileTypes.Floor;
        }

        public void OpenRight()
        {
            if (HasOpenRight)
                return;

            HasOpenRight = true;
            var startY = HasOpenTop ? 0 : 1;
            var endY = HasOpenBottom ? Tiles.GetLength(1) - 1 : Tiles.GetLength(1) - 2;
            for (var y = 0; y <= endY; y++)
                Tiles[Tiles.GetLength(0) - 1, y] = TileTypes.Floor;
        }
        #endregion

        public bool HasCollidingWallTile(Rectangle rectangle)
        {
            var startX = (rectangle.Left - _bounds.Left) / DisplayConstants.TileWidth;
            if (startX < 0) startX = 0;
            var endX = (rectangle.Right - _bounds.Left) / DisplayConstants.TileWidth;
            if (endX > Tiles.GetLength(0) - 1) endX = Tiles.GetLength(0) - 1;
            var startY = (rectangle.Top - _bounds.Top) / DisplayConstants.TileHeight;
            if (startY < 0) startY = 0;
            var endY = (rectangle.Bottom - _bounds.Top) / DisplayConstants.TileHeight;
            if (endY > Tiles.GetLength(1) - 1) endY = Tiles.GetLength(1) - 1;

            for (var x = startX; x <= endX; x++)
                for (var y = startY; y <= endY; y++)
                    if (TileIsWall(x, y))
                        return true;

            return false;
        }

        public void DestroyWall(CardinalDirection direction)
        {
            switch (direction)
            {
                case CardinalDirection.Down:
                    HasOpenBottom = true;
                    for (var x = 1; x < Tiles.GetLength(0) - 1; x++)
                        Tiles[x, Tiles.GetLength(1) - 1] = TileTypes.Floor;
                    break;
                case CardinalDirection.Up:
                    HasOpenTop = true;
                    for (var x = 1; x < Tiles.GetLength(0) - 1; x++)
                        Tiles[x, 0] = TileTypes.Floor;
                    break;
                case CardinalDirection.Left:
                    HasOpenLeft = true;
                    for (var y = 1; y < Tiles.GetLength(1) - 1; y++)
                        Tiles[0, y] = TileTypes.Floor;
                    break;
                case CardinalDirection.Right:
                    HasOpenRight = true;
                    for (var y = 1; y < Tiles.GetLength(1) - 1; y++)
                        Tiles[Tiles.GetLength(0) - 1, y] = TileTypes.Floor;
                    break;
            }
        }

        public void Draw(Dictionary<TileTypes, Rectangle> textureLocations, Texture2D texture)
        {
            for (var x = 0; x < Tiles.GetLength(0); x++)
                for (var y = 0; y < Tiles.GetLength(1); y++)
                    texture.Draw(
                        destinationRectangle: GetTileRectangle(x, y),
                        sourceRectangle: textureLocations[Tiles[x, y]],
                        color: Color.White);
        }

        /// <summary>
        /// Get a random point within the section to spawn an entity
        /// </summary>
        /// <returns></returns>
        public Point GetSpawnSpot(Being b)
        {
            var x = _bounds.Left + Globals.Random.Next(_bounds.Width);
            var y = _bounds.Top + Globals.Random.Next(_bounds.Height);
            // TODO: is this okay?
            var hitbox = b.Hitbox().First().Scale(1.35f);
            var tries = 0;
            while (HasCollidingWallTile(new Rectangle(x, y, hitbox.Width, hitbox.Height)))
            {
                x = _bounds.Left + Globals.Random.Next(_bounds.Width);
                y = _bounds.Top + Globals.Random.Next(_bounds.Height);
                // Some ting wong?
                if (tries++ > 50)
                    throw new Exception("Tried to create an enemy but could not");
            }
            return new Point(x, y);
        }

        private bool TileIsWall(int x, int y) => Tiles[x, y] == TileTypes.Wall || Tiles[x, y] == TileTypes.SingleWall;

        private Rectangle GetTileRectangle(int x, int y)
        {
            return new Rectangle(
                _bounds.Left + x * DisplayConstants.TileWidth,
                _bounds.Top + y * DisplayConstants.TileHeight,
                DisplayConstants.TileWidth,
                DisplayConstants.TileHeight);
        }
    }
}
