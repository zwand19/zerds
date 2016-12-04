using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Zerds.Enums;
using Zerds.Factories;

namespace Zerds.GameObjects
{
    public class Map
    {
        private Texture2D _texture;
        private SpriteBatch _spriteBatch;

        public Map(GraphicsDevice graphicsDevice, MapTypes type, Rectangle clientBounds)
        {
            _spriteBatch = new SpriteBatch(graphicsDevice);
            switch (type)
            {
                case MapTypes.Dungeon:
                    InitializeMap(graphicsDevice, clientBounds);
                    return;
            }
            throw new ArgumentException("Map type not handled");
        }

        public void InitializeMap(GraphicsDevice graphicsDevice, Rectangle clientBounds)
        {
            _texture = TextureCacheFactory.GetOnce("Maps/DungeonTile.png");
        }

        public void Draw()
        {
            _spriteBatch.Begin(samplerState: SamplerState.LinearWrap);
            _spriteBatch.Draw(
                texture: _texture,
                sourceRectangle: Globals.ViewportBounds,
                destinationRectangle: Globals.ViewportBounds,
                color: Color.White);
            _spriteBatch.End();
        }
    }
}
