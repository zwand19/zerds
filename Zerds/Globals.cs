using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zerds
{
    public static class Globals
    {
        public static Rectangle ViewportBounds { get; set; }
        public static SpriteBatch SpriteDrawer { get; set; }
        public static GameState GameState { get; set; }
    }
}
