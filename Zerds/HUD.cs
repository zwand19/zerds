using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Zerds.Abilities;
using Zerds.Constants;
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
            _healthBarBackColor = new Color(0.35f, 0.35f, 0.35f);
        }

        public static void Draw()
        {
            Globals.SpriteDrawer.Begin();
            Globals.GameState.Players.Where(p => p.IsPlaying).ToList().ForEach(DrawPlayer);
            DrawLevelText();
            DrawLevelBar();
            Globals.SpriteDrawer.End();
        }

        private static void DrawLevelBar()
        {
            const int width = 240;
            const int height = 28;
            const int border = 2;
            const int top = 30;
            var left = (float)Globals.ViewportBounds.Width / 2 - width / 2.0f;
            Globals.SpriteDrawer.DrawRect(Helpers.CreateRect(left, top, width, height));
            Globals.SpriteDrawer.DrawRect(Helpers.CreateRect(left + border, top + border, width - border * 2, height - border * 2), new Color(0.35f, 0.35f, 0.35f));
            var barWidth = Globals.GameState.LevelTimeRemaining.TotalMilliseconds / GameplayConstants.LevelLength.TotalMilliseconds * (width - border * 2);
            Globals.SpriteDrawer.DrawRect(Helpers.CreateRect(left + border, top + border, barWidth, height - border * 2), new Color(104, 40, 96));
            Globals.SpriteDrawer.DrawRect(Helpers.CreateRect(left + border + width / 4.0f, top + border, 1, height - border * 2), new Color(Color.Black, 0.5f));
            Globals.SpriteDrawer.DrawRect(Helpers.CreateRect(left + border + width / 2.0f, top + border, 1, height - border * 2), new Color(Color.Black, 0.5f));
            Globals.SpriteDrawer.DrawRect(Helpers.CreateRect(left + border + 3 * width / 4.0f, top + border, 1, height - border * 2), new Color(Color.Black, 0.5f));
            Globals.SpriteDrawer.DrawText($"LEVEL {Globals.GameState.Level}", new Vector2((float) Globals.ViewportBounds.Width / 2, top + height / 2.0f), 16f, color: Color.White);
        }

        private static void DrawLevelText()
        {
            var opacity = (float)Math.Sqrt((TimeSpan.FromSeconds(3) - Globals.GameState.TimeIntoLevel).TotalSeconds / 3.0f);
            if (opacity > 0f)
            {
                var position = Globals.ViewportBounds.Center.ToVector2() + new Vector2(0, (opacity - 1f) * 80);
                Globals.SpriteDrawer.DrawText($"- Level {Globals.GameState.Level} -", position, 32f, color: new Color(Color.Black, opacity));
            }
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
            Globals.SpriteDrawer.DrawRect(new Rectangle(position.X, position.Y, PlayerHudWidth, PlayerHudHeight));
            Globals.SpriteDrawer.Draw(_witchTexture,
                new Rectangle(position.X + Border, position.Y + Border, PlayerImageHeight, PlayerImageHeight), Color.White);
            Globals.SpriteDrawer.DrawRect(
                new Rectangle(position.X + PlayerImageHeight + Border * 2, position.Y + Border, PlayerHealthBarWidth, PlayerHealthBarHeight),
                _healthBarBackColor);
            Globals.SpriteDrawer.DrawRect(
                new Rectangle(position.X + PlayerImageHeight + Border * 2, position.Y + Border,
                    (int) (PlayerHealthBarWidth * player.Zerd.HealthPercentage), PlayerHealthBarHeight), player.Zerd.HealthColor);
            Globals.SpriteDrawer.DrawRect(
                new Rectangle(position.X + PlayerImageHeight + Border * 2,
                    position.Y + Border + PlayerHealthBarHeight + PlayerHudBarsSeparator, PlayerHealthBarWidth, PlayerHealthBarHeight),
                _healthBarBackColor);
            Globals.SpriteDrawer.DrawRect(
                new Rectangle(position.X + PlayerImageHeight + Border * 2,
                    position.Y + Border + PlayerHealthBarHeight + PlayerHudBarsSeparator,
                    (int) (PlayerHealthBarWidth * player.Zerd.ManaPercentage), PlayerHealthBarHeight),
                new Color(0.25f, 0.25f, 0.55f));
            // Ability Icons
            var iconPosition = new Vector2(position.X, position.Y + PlayerHudHeight + 8);
            foreach (var ability in player.Zerd.Abilities)
            {
                var color = ability.Cooldown > TimeSpan.Zero ? new Color(Color.Black, 0.35f) : Color.White;
                Globals.SpriteDrawer.Draw(ability.Texture, Helpers.CreateRect(iconPosition.X, iconPosition.Y, 32, 32), color);
                iconPosition = iconPosition.Move(40);
            }
        }
    }
}
