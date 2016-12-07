using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zerds
{
    public static class Globals
    {
        public static bool ShowHitboxes { get; set; }
        public static Texture2D WhiteTexture { get; set; }
        public static Rectangle ViewportBounds { get; set; }
        public static SpriteBatch SpriteDrawer { get; set; }
        public static GameState GameState { get; set; }

        public static void Initialize()
        {
            WhiteTexture = new Texture2D(SpriteDrawer.GraphicsDevice, 1, 1);
            WhiteTexture.SetData(new Color[] { Color.White });
        }
    }
}
