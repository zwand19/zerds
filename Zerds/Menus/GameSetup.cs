using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.Input;

namespace Zerds.Menus
{
    public class GameSetup
    {
        private readonly List<Quadrant> _quadrants;
        private readonly Func<List<ZerdTypes>, bool> _playGameFunc;

        public GameSetup(Func<List<ZerdTypes>, bool> playGameFunc)
        {
            _playGameFunc = playGameFunc;
            var width = Globals.ViewportBounds.Width / 2;
            var height = Globals.ViewportBounds.Height / 2;
            _quadrants = new List<Quadrant>
            {
                new Quadrant(PlayerIndex.One, Helpers.CreateRect(0, 0, width, height), StartGameFunc),
                new Quadrant(PlayerIndex.Two, Helpers.CreateRect(width, 0, width, height), StartGameFunc),
                new Quadrant(PlayerIndex.Three, Helpers.CreateRect(0, height, width, height), StartGameFunc),
                new Quadrant(PlayerIndex.Four, Helpers.CreateRect(width, height, width, height), StartGameFunc)
            };
        }

        public bool StartGameFunc()
        {
            return _playGameFunc(_quadrants.Select(q => q.ZerdType).ToList());
        }

        public void Draw()
        {
            Globals.SpriteDrawer.Begin();
            Globals.Map.Draw();
            Globals.SpriteDrawer.Draw(Globals.WhiteTexture, Globals.ViewportBounds, new Color(0, 0, 0, 0.75f));
            //Crosshairs
            Globals.SpriteDrawer.Draw(Globals.WhiteTexture,
                Helpers.CreateRect(Globals.ViewportBounds.Width / 2 - 3, 0, 6, Globals.ViewportBounds.Height), Color.Black);
            Globals.SpriteDrawer.Draw(Globals.WhiteTexture,
                Helpers.CreateRect(0, Globals.ViewportBounds.Height / 2 - 3, Globals.ViewportBounds.Width, 6), Color.Black);
            _quadrants.ForEach(q => q.Draw());
            Globals.SpriteDrawer.DrawText("PICK YOUR ZERD", Globals.ViewportBounds.Center.ToVector2(), 50f, color: Color.White);
            Globals.SpriteDrawer.End();
        }

        public void Update()
        {
            _quadrants.ForEach(q => q.Update());
        }

        private class Quadrant
        {
            private readonly PlayerIndex _playerIndex;
            private readonly Func<bool> _startFunc;
            private Rectangle _bounds;
            private int _step;
            private readonly Texture2D _blackZerd;
            private readonly Texture2D _blueZerd;
            private readonly Texture2D _brownZerd;
            private readonly Texture2D _cyanZerd;
            private readonly Texture2D _redZerd;
            private readonly List<Texture2D> _textures;
            private int _selection;

            public Quadrant(PlayerIndex playerIndex, Rectangle bounds, Func<bool> startFunc)
            {
                _playerIndex = playerIndex;
                _bounds = bounds;
                _step = 0;
                _startFunc = startFunc;

                _blackZerd = TextureCacheFactory.Get("Icons/zerd-black.png");
                _blueZerd = TextureCacheFactory.Get("Icons/zerd-blue.png");
                _brownZerd = TextureCacheFactory.Get("Icons/zerd-brown.png");
                _cyanZerd = TextureCacheFactory.Get("Icons/zerd-cyan.png");
                _redZerd = TextureCacheFactory.Get("Icons/zerd-red.png");
                _textures = new List<Texture2D> {_blackZerd, _blueZerd, _brownZerd, _cyanZerd, _redZerd};
            }

            public void Draw()
            {
                var c = _bounds.Center;
                switch (_step)
                {
                    case 0:
                        Globals.SpriteDrawer.DrawText("Press A to join.", _bounds.Center.ToVector2(), 20f, color: Color.White);
                        return;
                    case 1:
                        var t = _textures[_selection];
                        Globals.SpriteDrawer.Draw(t, new Vector2(_bounds.Center.X, c.Y - 20f), color: Color.White, origin: new Vector2(-t.Width / 2.0f, -t.Height / 2.0f));
                        Globals.SpriteDrawer.DrawText("Press A to select Zerd.", new Vector2(c.X, _bounds.Bottom - 50f), 20f, color: Color.White);
                        return;
                    case 2:
                        Globals.SpriteDrawer.DrawText("Press Start to being.", c.ToVector2(), 20f, color: Color.White);
                        return;
                }
            }

            public void Update()
            {
                var buttonsPressed = ControllerService.Controllers[_playerIndex].ButtonsPressed;
                if (buttonsPressed.Contains(Buttons.A) && _step < 2)
                    _step++;
                if (buttonsPressed.Contains(Buttons.B) && _step > 0)
                    _step--;
                if (buttonsPressed.Contains(Buttons.LeftThumbstickRight) && _step == 1)
                    _selection = (_selection + 1) % _textures.Count;
                if (buttonsPressed.Contains(Buttons.LeftThumbstickLeft) && _step == 1)
                    _selection = (_selection + _textures.Count + 1) % _textures.Count;
                if (buttonsPressed.Contains(Buttons.Start) && _step == 2)
                    _startFunc();
            }

            public ZerdTypes ZerdType
                =>
                _step < 2
                    ? ZerdTypes.NotPlaying
                    : _selection == 0 ? ZerdTypes.Black : _selection == 1 ? ZerdTypes.Blue : _selection == 2 ? ZerdTypes.Brown : _selection == 3 ? ZerdTypes.Cyan : ZerdTypes.Red;
        }
    }
}
