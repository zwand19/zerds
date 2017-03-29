using System;
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
        private static Texture2D _aBtnTexture;
        private static Texture2D _bBtnTexture;
        private static Texture2D _xBtnTexture;
        private static Texture2D _yBtnTexture;
        private static Texture2D _rtBtnTexture;
        private static Texture2D _rbBtnTexture;
        private static Color _healthBarBackColor;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _witchTexture = TextureCacheFactory.GetOnce("HUD/witch-icon.png");
            _aBtnTexture = TextureCacheFactory.GetOnce("HUD/a_button.png");
            _bBtnTexture = TextureCacheFactory.GetOnce("HUD/b_button.png");
            _xBtnTexture = TextureCacheFactory.GetOnce("HUD/x_button.png");
            _yBtnTexture = TextureCacheFactory.GetOnce("HUD/y_button.png");
            _rtBtnTexture = TextureCacheFactory.GetOnce("HUD/rt_button.png");
            _rbBtnTexture = TextureCacheFactory.GetOnce("HUD/rb_button.png");
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
            var barWidth = Level.TimeRemaining.TotalMilliseconds / GameplayConstants.LevelLength.TotalMilliseconds * (width - border * 2);
            Globals.SpriteDrawer.DrawRect(Helpers.CreateRect(left + border, top + border, barWidth, height - border * 2), new Color(104, 40, 96));
            Globals.SpriteDrawer.DrawRect(Helpers.CreateRect(left + border + width / 4.0f, top + border, 1, height - border * 2), new Color(Color.Black, 0.5f));
            Globals.SpriteDrawer.DrawRect(Helpers.CreateRect(left + border + width / 2.0f, top + border, 1, height - border * 2), new Color(Color.Black, 0.5f));
            Globals.SpriteDrawer.DrawRect(Helpers.CreateRect(left + border + 3 * width / 4.0f, top + border, 1, height - border * 2), new Color(Color.Black, 0.5f));
            Globals.SpriteDrawer.DrawText($"LEVEL {Level.CurrentLevel}", new Vector2((float) Globals.ViewportBounds.Width / 2, top + height / 2.0f), 16f, color: Color.White);
        }

        private static void DrawLevelText()
        {
            var opacity = Level.TimeRemaining <= TimeSpan.Zero ? 0f : (float)Math.Sqrt((TimeSpan.FromSeconds(3) - Level.TimeIntoLevel).TotalSeconds / 3.0f);
            if (opacity > 0f)
            {
                var position = Globals.ViewportBounds.Center.ToVector2() + new Vector2(0, (opacity - 1f) * 80);
                Globals.SpriteDrawer.DrawText($"- Level {Level.CurrentLevel} -", position, 32f, color: new Color(Color.Black, opacity));
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
            // Player HUD
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
            DrawIcon<Wand>(player, position, 0, 0);
            DrawIcon<Fireball>(player, position, 40, 0);
            DrawIcon<Iceball>(player, position, 80, 0);
            DrawIcon<Dash>(player, position, 120, 0);
            DrawIcon<Charm>(player, position, 160, 0);
            var secondRowOffset = player.PlayerIndex == PlayerIndex.One || player.PlayerIndex == PlayerIndex.Two ? 40 : -40;
            DrawIcon<FrostPound>(player, position, 0, secondRowOffset);
            DrawIcon<DragonBreath>(player, position, 40, secondRowOffset);
            DrawIcon<Icicle>(player, position, 80, secondRowOffset);
            DrawIcon<LavaBlast>(player, position, 120, secondRowOffset);
            // Button icons
            DrawButtonIcon<FrostPound>(_aBtnTexture, player, position, 0);
            DrawButtonIcon<DragonBreath>(_xBtnTexture, player, position, 40);
            DrawButtonIcon<Icicle>(_bBtnTexture, player, position, 80);
            DrawButtonIcon<LavaBlast>(_yBtnTexture, player, position, 120);
            if (player.Zerd.Abilities.Any(a => a is Charm)) DrawButtonIcon(_rbBtnTexture, player, position, 160);
        }

        private static void DrawIcon<T>(Player p, Point position, int x, int y)
        {
            var abil = p.Zerd.Abilities.FirstOrDefault(i => i is T);
            if (abil == null) return;
            Globals.SpriteDrawer.Draw(abil.IconTexture(), Helpers.CreateRect(x + position.X, y + position.Y + PlayerHudHeight + 8, 32, 32), Color.White);
        }

        private static void DrawButtonIcon<T>(Texture2D t, Player p, Point position, int x)
        {
            var secondRowOffset = p.PlayerIndex == PlayerIndex.One || p.PlayerIndex == PlayerIndex.Two ? 40 : -40;
            var abil = p.Zerd.Abilities.FirstOrDefault(i => i is T);
            var offset = abil == null ? secondRowOffset : secondRowOffset * 2;
            Globals.SpriteDrawer.Draw(t, Helpers.CreateRect(x + position.X, offset + position.Y + PlayerHudHeight + 8, 32, 32), Color.White * 0.75f);
        }

        private static void DrawButtonIcon(Texture2D t, Player p, Point position, int x)
        {
            var secondRowOffset = p.PlayerIndex == PlayerIndex.One || p.PlayerIndex == PlayerIndex.Three ? 40 : -40;
            Globals.SpriteDrawer.Draw(t, Helpers.CreateRect(x + position.X, secondRowOffset + position.Y + PlayerHudHeight + 8, 32, 32), Color.White * 0.75f);
        }
    }
}
