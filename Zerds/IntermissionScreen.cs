using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zerds.GameObjects;
using Zerds.Input;
using Zerds.Menus;

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
            private int _menu;
            private readonly MenuList _mainMenu;

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
                _mainMenu = new MenuList(new List<MenuListItem>
                {
                    new MenuListItem("Fire", () =>
                    {
                        _menu = 1;
                        return true;
                    }),
                    new MenuListItem("Frost", () =>
                    {
                        _menu = 2;
                        return true;
                    }),
                    new MenuListItem("Arcane", () =>
                    {
                        _menu = 3;
                        return true;
                    })
                });
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
                switch (_menu)
                {
                    case 0:
                        _mainMenu.Items[0].Text = $"Fire (Pts To Spend: {_player.Skills.FireSkillTree.PointsAvailable})";
                        _mainMenu.Items[1].Text = $"Frost (Pts To Spend: {_player.Skills.FrostSkillTree.PointsAvailable})";
                        _mainMenu.Items[2].Text = $"Arcane (Pts To Spend: {_player.Skills.ArcaneSkillTree.PointsAvailable})";
                        Globals.SpriteDrawer.DrawText($"Floating Points: {_player.FloatingSkillPoints}", new Vector2(_bounds.X + _bounds.Width / 2, _bounds.Bottom - 50), 20f, Color.White);
                        _mainMenu.Draw(new Vector2(_bounds.X + 20, _bounds.Y + 20), 20f, 50f, Color.White, new Color(200, 200, 200));
                        return;
                    case 1:
                        _player.Skills.FireSkillTree.Draw(_bounds);
                        return;
                    case 2:
                        _player.Skills.FrostSkillTree.Draw(_bounds);
                        return;
                    case 3:
                        _player.Skills.ArcaneSkillTree.Draw(_bounds);
                        return;
                }
                Globals.SpriteDrawer.DrawText("Press Start when ready.", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Bottom - 30f), 20f, Color.White);
            }

            public void Update()
            {
                if (ControllerService.Controllers[_index].IsPressed(Buttons.Start))
                    Ready = true;
                if (ControllerService.Controllers[_index].IsPressed(Buttons.B))
                    _menu = 0;                
                switch (_menu)
                {
                    case 0:
                        _mainMenu.Update();
                        return;
                    case 1:
                        _player.Skills.FireSkillTree.Update();
                        return;
                    case 2:
                        _player.Skills.FrostSkillTree.Update();
                        return;
                    case 3:
                        _player.Skills.ArcaneSkillTree.Update();
                        return;
                }
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
