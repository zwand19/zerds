using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zerds.Data;
using Zerds.Input;
using Zerds.Items;

namespace Zerds.Menus
{
    public class GameSetup
    {
        private readonly List<Quadrant> _quadrants;
        private readonly Func<List<bool>, bool> _playGameFunc;
        public List<string> PlayerNames;
        public List<string> FirstNames;
        public List<string> MiddleNames;
        public List<string> LastNames;

        public GameSetup(Func<List<bool>, bool> playGameFunc)
        {
            _playGameFunc = playGameFunc;
            var width = Globals.ViewportBounds.Width / 2;
            var height = Globals.ViewportBounds.Height / 2;
            PlayerNames = XmlStorage.SavedData.Players.Select(p => p.Name).ToList();
            PlayerNames.Insert(0, "Create New");
            FirstNames = new List<string>
            {
                "Son",
                "Bin",
                "Po",
                "War",
                "Min",
                "Kahn",
                "Rah",
                "Zoe",
                "Ah",
                "Vu"
            };
            MiddleNames = new List<string>
            {
                "tu",
                "mo",
                "wu",
                "bo",
                "jin",
                "zu",
                "ka"
            };
            LastNames = new List<string>
            {
                "sa",
                "li",
                "tu",
                "pi",
                "zi",
                "pe",
                "do",
                "ka"
            };
            _quadrants = new List<Quadrant>
            {
                new Quadrant(PlayerIndex.One, Helpers.CreateRect(0, 0, width, height), this),
                new Quadrant(PlayerIndex.Two, Helpers.CreateRect(width, 0, width, height), this),
                new Quadrant(PlayerIndex.Three, Helpers.CreateRect(0, height, width, height), this),
                new Quadrant(PlayerIndex.Four, Helpers.CreateRect(width, height, width, height), this)
            };
        }

        public bool StartGame()
        {
            if (_quadrants.Any(q => q.IsJoining) || _quadrants.All(q => !q.IsPlaying))
                return false;
            return _playGameFunc(_quadrants.Select(q => q.IsPlaying).ToList());
        }

        public void Draw()
        {
            Globals.SpriteDrawer.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Globals.Map.Draw();
            Globals.SpriteDrawer.Draw(Globals.WhiteTexture, Globals.ViewportBounds, new Color(0, 0, 0, 0.5f));
            //Crosshairs
            Globals.SpriteDrawer.Draw(Globals.WhiteTexture,
                Helpers.CreateRect(Globals.ViewportBounds.Width / 2 - 3, 0, 6, Globals.ViewportBounds.Height), Color.Black);
            Globals.SpriteDrawer.Draw(Globals.WhiteTexture,
                Helpers.CreateRect(0, Globals.ViewportBounds.Height / 2 - 3, Globals.ViewportBounds.Width, 6), Color.Black);
            _quadrants.ForEach(q => q.Draw(_quadrants.Any(p => p.IsJoining)));
            Globals.SpriteDrawer.DrawText("PICK YOUR ZERD", Globals.ViewportBounds.Center.ToVector2(), 50f, Color.White);
            Globals.SpriteDrawer.End();
        }

        public void Update()
        {
            _quadrants.ForEach(q => q.Update());
        }

        private class Quadrant
        {
            private readonly PlayerIndex _playerIndex;
            private readonly GameSetup _setup;
            private Rectangle _bounds;
            private Stage _step;
            private List<Item> _items;
            private string _selectedName;
            private int _firstNameIndex;
            private int _middleNameIndex;
            private int _lastNameIndex;
            private int _nameSelectorIndex;

            public Quadrant(PlayerIndex playerIndex, Rectangle bounds, GameSetup setup)
            {
                _playerIndex = playerIndex;
                _bounds = bounds;
                _step = Stage.NotJoined;
                _setup = setup;
                _selectedName = _setup.PlayerNames.First();
                _firstNameIndex = new Random().Next(_setup.FirstNames.Count);
                _middleNameIndex = new Random().Next(_setup.MiddleNames.Count);
                _lastNameIndex = new Random().Next(_setup.LastNames.Count);
            }

            public void Draw(bool playersAreJoining)
            {
                var c = _bounds.Center;
                switch (_step)
                {
                    case Stage.NotJoined:
                        Globals.SpriteDrawer.DrawText("Press A to join.", _bounds.Center.ToVector2(), 20f, Color.White);
                        break;
                    case Stage.SelectingPlayer:
                        Globals.SpriteDrawer.DrawText(_selectedName, _bounds.Center.ToVector2(), 20f, Globals.ContinueColor);
                        Globals.SpriteDrawer.DrawText("<", new Vector2(_bounds.Left + 30f, _bounds.Center.Y), 20f, Color.White);
                        Globals.SpriteDrawer.DrawText(">", new Vector2(_bounds.Right - 30f, _bounds.Center.Y), 20f, Color.White);
                        break;
                    case Stage.Waiting:
                        Globals.SpriteDrawer.DrawText(playersAreJoining ? "Waiting for others" : "Press Start to Begin.", c.ToVector2(), 20f, Color.White);
                        break;
                    case Stage.CreatingPlayer:
                        Globals.SpriteDrawer.DrawText("Create Your Name", new Vector2(_bounds.Center.X, _bounds.Top + 40f),  20f, Color.White);
                        if (_nameSelectorIndex == 0 && _firstNameIndex > 0) Globals.SpriteDrawer.DrawText(_setup.FirstNames[_firstNameIndex - 1], new Vector2(_bounds.Center.X - 70f, _bounds.Center.Y - 40f), 16f, Color.White * 0.5f);
                        Globals.SpriteDrawer.DrawText(_setup.FirstNames[_firstNameIndex], new Vector2(_bounds.Center.X - 70f, _bounds.Center.Y), 20f, new Color(Color.White, _nameSelectorIndex == 0 ? 255 : 180));
                        if (_nameSelectorIndex == 0 && _firstNameIndex < _setup.FirstNames.Count - 1) Globals.SpriteDrawer.DrawText(_setup.FirstNames[_firstNameIndex + 1], new Vector2(_bounds.Center.X - 70f, _bounds.Center.Y + 40f), 16f, Color.White * 0.5f);
                        if (_nameSelectorIndex == 1 && _middleNameIndex > 0) Globals.SpriteDrawer.DrawText(_setup.MiddleNames[_middleNameIndex - 1], new Vector2(_bounds.Center.X, _bounds.Center.Y - 40f), 16f, Color.White * 0.5f);
                        Globals.SpriteDrawer.DrawText(_setup.MiddleNames[_middleNameIndex], new Vector2(_bounds.Center.X, _bounds.Center.Y), 20f, new Color(Color.White, _nameSelectorIndex == 1 ? 255 : 180));
                        if (_nameSelectorIndex == 1 && _middleNameIndex < _setup.MiddleNames.Count - 1) Globals.SpriteDrawer.DrawText(_setup.MiddleNames[_middleNameIndex + 1], new Vector2(_bounds.Center.X, _bounds.Center.Y + 40f), 16f, Color.White * 0.5f);
                        if (_nameSelectorIndex == 2 && _lastNameIndex > 0) Globals.SpriteDrawer.DrawText(_setup.LastNames[_lastNameIndex - 1], new Vector2(_bounds.Center.X + 70f, _bounds.Center.Y - 40f), 16f, Color.White * 0.5f);
                        Globals.SpriteDrawer.DrawText(_setup.LastNames[_lastNameIndex], new Vector2(_bounds.Center.X + 70f, _bounds.Center.Y), 20f, new Color(Color.White, _nameSelectorIndex == 2 ? 255 : 180));
                        if (_nameSelectorIndex == 2 && _lastNameIndex < _setup.LastNames.Count - 1) Globals.SpriteDrawer.DrawText(_setup.LastNames[_lastNameIndex + 1], new Vector2(_bounds.Center.X + 70f, _bounds.Center.Y + 40f), 16f, Color.White * 0.5f);
                        break;
                    case Stage.PickingGear:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public void Update()
            {
                var buttonsPressed = ControllerService.Controllers[_playerIndex].ButtonsPressed;
                if (buttonsPressed.Contains(Buttons.A))
                {
                    switch (_step)
                    {
                        case Stage.NotJoined:
                            _step = Stage.SelectingPlayer;
                            return;
                        case Stage.SelectingPlayer:
                            SelectPlayer();
                            return;
                        case Stage.CreatingPlayer:
                            if (_nameSelectorIndex < 2)
                                _nameSelectorIndex++;
                            else
                                CreatePlayer();
                            break;
                        case Stage.PickingGear:
                            break;
                        case Stage.Waiting:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                if (buttonsPressed.Contains(Buttons.B))
                {
                    switch (_step)
                    {
                        case Stage.NotJoined:
                            break;
                        case Stage.SelectingPlayer:
                            _step = Stage.NotJoined;
                            return;
                        case Stage.CreatingPlayer:
                            if (_nameSelectorIndex > 0)
                            {
                                _nameSelectorIndex--;
                            }
                            else
                            {
                                _step = Stage.SelectingPlayer;
                                _selectedName = _setup.PlayerNames.First();
                            }
                            return;
                        case Stage.PickingGear:
                            _step = Stage.SelectingPlayer;
                            _setup.PlayerNames.Insert(0, _selectedName);
                            _selectedName = _setup.PlayerNames.First();
                            break;
                        case Stage.Waiting:
                            _step = Stage.PickingGear;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                if (buttonsPressed.Contains(Buttons.LeftThumbstickUp))
                {
                    switch (_step)
                    {
                        case Stage.CreatingPlayer:
                            if (_nameSelectorIndex == 0 && _firstNameIndex > 0) _firstNameIndex--;
                            if (_nameSelectorIndex == 1 && _middleNameIndex > 0) _middleNameIndex--;
                            if (_nameSelectorIndex == 2 && _lastNameIndex > 0) _lastNameIndex--;
                            break;
                        case Stage.NotJoined:
                        case Stage.SelectingPlayer:
                        case Stage.PickingGear:
                        case Stage.Waiting:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                if (buttonsPressed.Contains(Buttons.LeftThumbstickDown))
                {
                    switch (_step)
                    {
                        case Stage.CreatingPlayer:
                            if (_nameSelectorIndex == 0 && _firstNameIndex < _setup.FirstNames.Count - 1) _firstNameIndex++;
                            if (_nameSelectorIndex == 1 && _middleNameIndex < _setup.MiddleNames.Count - 1) _middleNameIndex++;
                            if (_nameSelectorIndex == 2 && _lastNameIndex < _setup.LastNames.Count - 1) _lastNameIndex++;
                            break;
                        case Stage.NotJoined:
                        case Stage.SelectingPlayer:
                        case Stage.PickingGear:
                        case Stage.Waiting:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (buttonsPressed.Contains(Buttons.Start))
                    _setup.StartGame();
            }
            public bool IsPlaying => _step == Stage.Waiting;
            public bool IsJoining => _step != Stage.NotJoined && _step != Stage.Waiting;

            private void SelectPlayer()
            {
                if (_selectedName == "Create New")
                {
                    _step = Stage.CreatingPlayer;
                }
                else
                {
                    _setup.PlayerNames.Remove(_selectedName);
                    _setup._quadrants.Where(q => q._selectedName == _selectedName).ToList().ForEach(q => { q._selectedName = _setup.PlayerNames.First(); });
                    _step = Stage.PickingGear;
                    _items = XmlStorage.GetPlayerInventory(_selectedName);
                }
            }

            private void CreatePlayer()
            {
                _step = Stage.PickingGear;
            }


            private enum Stage
            {
                NotJoined,
                SelectingPlayer,
                CreatingPlayer,
                PickingGear,
                Waiting
            }
        }
    }
}
