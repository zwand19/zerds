using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zerds.GameObjects;
using Zerds.Input;

namespace Zerds
{
    public class IntermissionScreen
    {
        private readonly List<Quadrant> _quadrants;

        public IntermissionScreen()
        {
            _quadrants = new List<Quadrant>
            {
                new Quadrant(PlayerIndex.One),
                new Quadrant(PlayerIndex.Two),
                new Quadrant(PlayerIndex.Three),
                new Quadrant(PlayerIndex.Four)
            };
        }

        private class Quadrant
        {
            private readonly Player _player;
            private readonly Rectangle _bounds;
            private readonly PlayerIndex _index;
            public bool Ready { get; private set; }

            private const int MenuSidePadding = 100;
            private const int QuadrantPadding = 20;
            private const int MenuTopPadding = 150;
            private const int MenuBorder = 3;

            public Quadrant(PlayerIndex index)
            {
                _index = index;
                _player = Globals.GameState.Players.First(p => p.PlayerIndex == index);
                var width = (Globals.ViewportBounds.Width - MenuSidePadding * 2 - QuadrantPadding * 3) / 4;
                var height = (Globals.ViewportBounds.Height - MenuTopPadding * 2);
                _bounds = Helpers.CreateRect(MenuSidePadding, MenuTopPadding, width, height);
                switch (_index)
                {
                    case PlayerIndex.Two:
                        _bounds = new Rectangle(_bounds.X + width + QuadrantPadding, _bounds.Y, _bounds.Width, _bounds.Height);
                        break;
                    case PlayerIndex.Three:
                        _bounds = new Rectangle(_bounds.X + width * 2 + QuadrantPadding * 2, _bounds.Y, _bounds.Width, _bounds.Height);
                        break;
                    case PlayerIndex.Four:
                        _bounds = new Rectangle(_bounds.X + width * 3 + QuadrantPadding * 3, _bounds.Y, _bounds.Width, _bounds.Height);
                        break;
                }
                Ready = !_player.IsPlaying;
            }

            public void Draw()
            {
                Globals.SpriteDrawer.DrawRect(_bounds, new Color(Color.Black, 0.75f));
                Globals.SpriteDrawer.DrawRect(new Rectangle(_bounds.X + MenuBorder, _bounds.Y + MenuBorder, _bounds.Width - 2 * MenuBorder, _bounds.Height - 2 * MenuBorder),
                    new Color(new Color(40, 40, 40), 0.65f));
                if (!_player.IsPlaying)
                {
                    Globals.SpriteDrawer.DrawText("Press A to join.", _bounds.Center.ToVector2(), 20f, Color.White);
                    return;
                }
                if (Ready)
                {
                    Globals.SpriteDrawer.DrawText("Ready!", _bounds.Center.ToVector2(), 20f, Color.White);
                    return;
                }
                Globals.SpriteDrawer.DrawText("Press Start when ready.", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Bottom - 30f), 20f, Color.White);
            }

            public void Update()
            {
                if (ControllerService.Controllers[_index].ButtonsPressed.Contains(Buttons.Start))
                    Ready = true;
            }
        }

        public void Draw()
        {
            Globals.SpriteDrawer.Begin();
            _quadrants.ForEach(q => q.Draw());
            Globals.SpriteDrawer.End();
        }

        public void Update()
        {
            _quadrants.ForEach(q => q.Update());
        }

        public bool Ready => _quadrants.All(q => q.Ready);
    }
}
