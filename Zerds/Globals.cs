using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Enums;
using Zerds.GameObjects;

namespace Zerds
{
    public static class Globals
    {
        public static bool ShowHitboxes { get; set; }
        public static Texture2D WhiteTexture { get; set; }
        public static Rectangle ViewportBounds { get; set; }
        public static SpriteBatch SpriteDrawer { get; set; }
        public static GameState GameState { get; set; }
        public static ContentManager ContentManager { get; internal set; }
        public static Dictionary<FontTypes, SpriteFont> Fonts { get; set; }
        public static Map Map { get; set; }

        public static void Initialize()
        {
            WhiteTexture = new Texture2D(SpriteDrawer.GraphicsDevice, 1, 1);
            WhiteTexture.SetData(new[] { Color.White });
            Fonts = new Dictionary<FontTypes, SpriteFont>();
        }

        public static void LoadFont(string fileName, FontTypes type)
        {
            Fonts[type] = ContentManager.Load<SpriteFont>(fileName);
        }
    }
}
