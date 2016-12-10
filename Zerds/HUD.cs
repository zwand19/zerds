using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Zerds.Factories;
using Zerds.GameObjects;

namespace Zerds
{
    public static class HUD
    {
        private const int PlayerHudHeight = 60;
        private const int PlayerHudWidth = 200;
        private const int PlayerHudPadding = 50;
        private const int Border = 6;
        private const int PlayerHudBarsSeparator = 4;
        private static int PlayerImageHeight => PlayerHudHeight - Border * 2;
        private static int PlayerHealthBarWidth => PlayerHudWidth - PlayerImageHeight - Border * 3;
        private static int PlayerHealthBarHeight => (PlayerImageHeight - PlayerHudBarsSeparator) / 2;
        private static Texture2D _witchTexture;
        private static Color _healthBarBackColor;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _witchTexture = TextureCacheFactory.GetOnce("HUD/witch-icon.png");
            _healthBarBackColor = new Color(0.55f, 0.55f, 0.55f);
        }

        public static void Draw()
        {
            Globals.SpriteDrawer.Begin();
            Globals.GameState.Players.Where(p => p.IsPlaying).ToList().ForEach(DrawPlayer);
            Globals.SpriteDrawer.End();
        }

        private static void DrawPlayer(Player player)
        {
            Point position;
            switch (player.PlayerIndex)
            {
                case PlayerIndex.One:
                    position = new Point(PlayerHudPadding, PlayerHudPadding);
                    break;
                case PlayerIndex.Two:
                    position = new Point(Globals.ViewportBounds.Width - PlayerHudPadding - PlayerHudWidth, PlayerHudPadding);
                    break;
                case PlayerIndex.Three:
                    position = new Point(PlayerHudPadding, Globals.ViewportBounds.Height - PlayerHudPadding - PlayerHudHeight);
                    break;
                default:
                    position = new Point(Globals.ViewportBounds.Width - PlayerHudPadding - PlayerHudWidth,
                                        Globals.ViewportBounds.Height - PlayerHudPadding - PlayerHudHeight);
                    break;
            }
            Globals.SpriteDrawer.Draw(
                texture: Globals.WhiteTexture,
                destinationRectangle: new Rectangle(position.X, position.Y, PlayerHudWidth, PlayerHudHeight),
                color: Color.Black);
            Globals.SpriteDrawer.Draw(
                texture: _witchTexture,
                destinationRectangle: new Rectangle(position.X + Border, position.Y + Border, PlayerImageHeight, PlayerImageHeight),
                color: Color.White);
            Globals.SpriteDrawer.Draw(
                texture: Globals.WhiteTexture,
                destinationRectangle: new Rectangle(position.X + PlayerImageHeight + Border * 2, position.Y + Border, PlayerHealthBarWidth, PlayerHealthBarHeight),
                color: _healthBarBackColor);
            Globals.SpriteDrawer.Draw(
                texture: Globals.WhiteTexture,
                destinationRectangle: new Rectangle(position.X + PlayerImageHeight + Border * 2, position.Y + Border, (int)(PlayerHealthBarWidth * player.Zerd.HealthPercentage), PlayerHealthBarHeight),
                color: player.Zerd.HealthColor);
            Globals.SpriteDrawer.Draw(
                texture: Globals.WhiteTexture,
                destinationRectangle: new Rectangle(position.X + PlayerImageHeight + Border * 2, position.Y + Border + PlayerHealthBarHeight + PlayerHudBarsSeparator, PlayerHealthBarWidth, PlayerHealthBarHeight),
                color: _healthBarBackColor);
            Globals.SpriteDrawer.Draw(
                texture: Globals.WhiteTexture,
                destinationRectangle: new Rectangle(position.X + PlayerImageHeight + Border * 2, position.Y + Border + PlayerHealthBarHeight + PlayerHudBarsSeparator, (int)(PlayerHealthBarWidth * player.Zerd.ManaPercentage), PlayerHealthBarHeight),
                color: new Color(0.25f, 0.25f, 0.55f));
        }
    }
}
