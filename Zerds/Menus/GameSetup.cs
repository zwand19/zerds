using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zerds.Input;

namespace Zerds.Menus
{
    public class GameSetup
    {
        private readonly List<Quadrant> _quadrants;
        private readonly Func<List<bool>, bool> _playGameFunc;

        public GameSetup(Func<List<bool>, bool> playGameFunc)
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
            return _playGameFunc(_quadrants.Select(q => q.Active).ToList());
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

            public Quadrant(PlayerIndex playerIndex, Rectangle bounds, Func<bool> startFunc)
            {
                _playerIndex = playerIndex;
                _bounds = bounds;
                _step = 0;
                _startFunc = startFunc;
            }

            public void Draw()
            {
                switch (_step)
                {
                    case 0:
                        Globals.SpriteDrawer.DrawText("Press A to join.", _bounds.Center.ToVector2(), 20f, color: Color.White);
                        return;
                    case 1:
                        Globals.SpriteDrawer.DrawText("Press Start to begin.", _bounds.Center.ToVector2(), 20f, color: Color.White);
                        return;
                }
            }

            public void Update()
            {
                var buttonsPressed = ControllerService.Controllers[_playerIndex].ButtonsPressed;
                if (buttonsPressed.Contains(Buttons.A) && _step == 0)
                    _step = 1;
                if (buttonsPressed.Contains(Buttons.Start) && _step == 1)
                    _startFunc();
                if (buttonsPressed.Contains(Buttons.B) && _step == 1)
                    _step = 0;
            }

            public bool Active => _step == 1;
        }
    }
}
