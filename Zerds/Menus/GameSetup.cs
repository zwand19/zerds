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
        private readonly Func<List<Tuple<bool, string, List<Item>, List<Item>>>, bool> _playGameFunc;
        public List<string> PlayerNames;
        public List<string> FirstNames;
        public List<string> MiddleNames;
        public List<string> LastNames;

        public GameSetup(Func<List<Tuple<bool, string, List<Item>, List<Item>>>, bool> playGameFunc)
        {
            _playGameFunc = playGameFunc;
            var width = Globals.ViewportBounds.Width / 2;
            var height = Globals.ViewportBounds.Height / 2;
            PlayerNames = XmlStorage.SavedData.Players.Select(p => p.Name).ToList();
            PlayerNames.Add("Create New");
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
                new Quadrant(PlayerIndex.One, Helpers.CreateRect(40, 40, width - 40, height - 40), this),
                new Quadrant(PlayerIndex.Two, Helpers.CreateRect(width, 40, width - 40, height - 40), this),
                new Quadrant(PlayerIndex.Three, Helpers.CreateRect(40, height, width - 40, height - 40), this),
                new Quadrant(PlayerIndex.Four, Helpers.CreateRect(width, height, width - 40, height - 40), this)
            };
        }

        public bool StartGame()
        {
            if (_quadrants.Any(q => q.IsJoining) || _quadrants.All(q => !q.IsPlaying))
                return false;
            return _playGameFunc(_quadrants.Select(q => new Tuple<bool, string, List<Item>, List<Item>>(q.IsPlaying, q.Name, q.Gear, q.Items)).ToList());
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
            private int _firstNameIndex;
            private int _middleNameIndex;
            private int _lastNameIndex;
            private int _nameSelectorIndex;
            private Item _selectedItem;
            private Rectangle _selectedItemBounds;
            private Rectangle _itemListBounds;
            private PickingGearStage _pickingGearStage;
            private bool _creatingPlayer;
            public string Name;
            public List<Item> Gear;
            public List<Item> Items;

            public Quadrant(PlayerIndex playerIndex, Rectangle bounds, GameSetup setup)
            {
                _playerIndex = playerIndex;
                _bounds = bounds;
                _step = Stage.NotJoined;
                _setup = setup;
                Name = _setup.PlayerNames.First();
                _firstNameIndex = Globals.Random.Next(_setup.FirstNames.Count);
                _middleNameIndex = Globals.Random.Next(_setup.MiddleNames.Count);
                _lastNameIndex = Globals.Random.Next(_setup.LastNames.Count);
                _selectedItemBounds = new Rectangle(bounds.Left + 40, bounds.Top + 80, (bounds.Width - 120) / 2, bounds.Height - 140);
                _itemListBounds = new Rectangle(bounds.Left + bounds.Width / 2 + 20, bounds.Top + 80, (bounds.Width - 120) / 2, bounds.Height - 140);
                Gear = new List<Item>();
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
                        Globals.SpriteDrawer.DrawText(Name, _bounds.Center.ToVector2(), 20f, Globals.ContinueColor);
                        Globals.SpriteDrawer.DrawText("<", new Vector2(_bounds.Center.X - 200f, _bounds.Center.Y), 20f, Color.White);
                        Globals.SpriteDrawer.DrawText(">", new Vector2(_bounds.Center.X + 200f, _bounds.Center.Y), 20f, Color.White);
                        break;
                    case Stage.Waiting:
                        Globals.SpriteDrawer.DrawText(playersAreJoining ? "Waiting for others" : "Press Start to Begin.", c.ToVector2(), 20f, Globals.ContinueColor);
                        break;
                    case Stage.CreatingPlayer:
                        Globals.SpriteDrawer.DrawText("Create Your Name", new Vector2(_bounds.Center.X, _bounds.Top + 40f),  20f, Color.White);
                        if (_nameSelectorIndex == 0 && _firstNameIndex > 0) Globals.SpriteDrawer.DrawTextRightAlign(_setup.FirstNames[_firstNameIndex - 1], new Vector2(_bounds.Center.X - 40f, _bounds.Center.Y - 40f), 16f, Color.White * 0.5f);
                        Globals.SpriteDrawer.DrawTextRightAlign(_setup.FirstNames[_firstNameIndex], new Vector2(_bounds.Center.X - 40f, _bounds.Center.Y), 20f, new Color(Color.White, _nameSelectorIndex == 0 ? 255 : 180));
                        if (_nameSelectorIndex == 0 && _firstNameIndex < _setup.FirstNames.Count - 1) Globals.SpriteDrawer.DrawTextRightAlign(_setup.FirstNames[_firstNameIndex + 1], new Vector2(_bounds.Center.X - 40f, _bounds.Center.Y + 40f), 16f, Color.White * 0.5f);
                        if (_nameSelectorIndex == 1 && _middleNameIndex > 0) Globals.SpriteDrawer.DrawText(_setup.MiddleNames[_middleNameIndex - 1], new Vector2(_bounds.Center.X, _bounds.Center.Y - 40f), 16f, Color.White * 0.5f);
                        Globals.SpriteDrawer.DrawText(_setup.MiddleNames[_middleNameIndex], new Vector2(_bounds.Center.X, _bounds.Center.Y), 20f, new Color(Color.White, _nameSelectorIndex == 1 ? 255 : 180));
                        if (_nameSelectorIndex == 1 && _middleNameIndex < _setup.MiddleNames.Count - 1) Globals.SpriteDrawer.DrawText(_setup.MiddleNames[_middleNameIndex + 1], new Vector2(_bounds.Center.X, _bounds.Center.Y + 40f), 16f, Color.White * 0.5f);
                        if (_nameSelectorIndex == 2 && _lastNameIndex > 0) Globals.SpriteDrawer.DrawTextLeftAlign(_setup.LastNames[_lastNameIndex - 1], new Vector2(_bounds.Center.X + 40f, _bounds.Center.Y - 40f), 16f, Color.White * 0.5f);
                        Globals.SpriteDrawer.DrawTextLeftAlign(_setup.LastNames[_lastNameIndex], new Vector2(_bounds.Center.X + 40f, _bounds.Center.Y), 20f, new Color(Color.White, _nameSelectorIndex == 2 ? 255 : 180));
                        if (_nameSelectorIndex == 2 && _lastNameIndex < _setup.LastNames.Count - 1) Globals.SpriteDrawer.DrawTextLeftAlign(_setup.LastNames[_lastNameIndex + 1], new Vector2(_bounds.Center.X + 40f, _bounds.Center.Y + 40f), 16f, Color.White * 0.5f);
                        break;
                    case Stage.PickingGear:
                        Globals.SpriteDrawer.DrawText("Pick Your Gear", new Vector2(_bounds.Center.X, _bounds.Top + 40f), 20f, Color.White);
                        // Draw item info on the left
                        Globals.SpriteDrawer.DrawRect(_selectedItemBounds.BorderRect(2), Color.White);
                        Globals.SpriteDrawer.DrawRect(_selectedItemBounds, Color.Black);
                        Globals.SpriteDrawer.DrawRect(_itemListBounds.BorderRect(2), Color.White);
                        Globals.SpriteDrawer.DrawRect(_itemListBounds, Color.Black);
                        Globals.SpriteDrawer.DrawText(_selectedItem.Name, new Vector2(_selectedItemBounds.Center.X, _selectedItemBounds.Top + 50f), 20f, _selectedItem.TextColor);
                        _selectedItem.Draw(_selectedItemBounds.Center.X, _selectedItemBounds.Center.Y - 60f, 70, 70);
                        var y = _selectedItemBounds.Center.Y;
                        foreach (var upgrade in _selectedItem.AbilityUpgrades)
                        {
                            Globals.SpriteDrawer.DrawText(upgrade.ToString(), new Vector2(_selectedItemBounds.Center.X, y), 14f, Color.White);
                            y += 22;
                        }
                        Globals.SpriteDrawer.DrawText(_selectedItem.Description1(), new Vector2(_selectedItemBounds.Center.X, y), 14f, Color.White);
                        y += 22;
                        Globals.SpriteDrawer.DrawText(_selectedItem.Description2(), new Vector2(_selectedItemBounds.Center.X, y), 14f, Color.White);
                        // Draw the item we are highlighting
                        var rect = new Rectangle(_itemListBounds.Left + 10, _itemListBounds.Center.Y - 40, _itemListBounds.Width - 20, 80);
                        Globals.SpriteDrawer.DrawRect(rect, _selectedItem.TextColor * 0.25f);
                        _selectedItem.Draw(rect.Left + 40, rect.Top + 40, 60, 60);
                        Globals.SpriteDrawer.DrawTextLeftAlign(_selectedItem.Name, new Vector2(rect.Left + 80, rect.Center.Y), 20f, Color.White);
                        // If there's an item before this in the list, draw it slightly faded above
                        const float fade = 0.4f;
                        if (ItemBefore != null)
                        {
                            rect = new Rectangle(_itemListBounds.Left + 10, _itemListBounds.Center.Y - 140, _itemListBounds.Width - 20, 80);
                            Globals.SpriteDrawer.DrawRect(rect, ItemBefore.TextColor * 0.25f * fade);
                            ItemBefore.Draw(rect.Left + 40, rect.Top + 40, 60, 60, fade);
                            Globals.SpriteDrawer.DrawTextLeftAlign(ItemBefore.Name, new Vector2(rect.Left + 80, rect.Center.Y), 20f, Color.White * fade);
                        }
                        // If there's an item after this in the list, draw it slightly faded below
                        if (ItemAfter != null)
                        {
                            rect = new Rectangle(_itemListBounds.Left + 10, _itemListBounds.Center.Y + 60, _itemListBounds.Width - 20, 80);
                            Globals.SpriteDrawer.DrawRect(rect, ItemAfter.TextColor * 0.25f * fade);
                            ItemAfter.Draw(rect.Left + 40, rect.Top + 40, 60, 60, fade);
                            Globals.SpriteDrawer.DrawTextLeftAlign(ItemAfter.Name, new Vector2(rect.Left + 80, rect.Center.Y), 20f, Color.White * fade);
                        }
                        Globals.SpriteDrawer.DrawText("Press A to Equip", new Vector2(_selectedItemBounds.Center.X, _selectedItemBounds.Bottom - 40f), 20f, Globals.ContinueColor);
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
                            else {
                                Name = _setup.FirstNames[_firstNameIndex] + _setup.MiddleNames[_middleNameIndex] + _setup.LastNames[_lastNameIndex];
                                if (_setup.PlayerNames.All(n => n != Name))
                                    CreatePlayer();
                                else Name = "Create New";
                            }
                            break;
                        case Stage.PickingGear:
                            Gear.Add(_selectedItem);
                            switch (_pickingGearStage)
                            {
                                case PickingGearStage.Boots:
                                    _pickingGearStage = PickingGearStage.Robe;
                                    break;
                                case PickingGearStage.Robe:
                                    _pickingGearStage = PickingGearStage.Helm;
                                    break;
                                case PickingGearStage.Helm:
                                    _pickingGearStage = PickingGearStage.Glove;
                                    break;
                                case PickingGearStage.Glove:
                                    _pickingGearStage = PickingGearStage.Wand;
                                    break;
                                case PickingGearStage.Wand:
                                    _step = Stage.Waiting;
                                    return;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            _selectedItem = Items.First(i => i.Type == CurrentGearType());
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
                                Name = _setup.PlayerNames.First();
                            }
                            return;
                        case Stage.PickingGear:
                            _step = Stage.SelectingPlayer;
                            Gear = new List<Item>();
                            if (!_creatingPlayer)
                                _setup.PlayerNames.Insert(0, Name);
                            Name = _setup.PlayerNames.First();
                            break;
                        case Stage.Waiting:
                            _step = Stage.PickingGear;
                            _pickingGearStage = PickingGearStage.Boots;
                            _selectedItem = Items.First(i => i.Type == ItemTypes.Boots);
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
                        case Stage.PickingGear:
                            _selectedItem = ItemBefore ?? _selectedItem;
                            break;
                        case Stage.NotJoined:
                        case Stage.SelectingPlayer:
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
                        case Stage.PickingGear:
                            _selectedItem = ItemAfter ?? _selectedItem;
                            break;
                        case Stage.NotJoined:
                        case Stage.SelectingPlayer:
                        case Stage.Waiting:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                if (buttonsPressed.Contains(Buttons.LeftThumbstickLeft))
                {
                    switch (_step)
                    {
                        case Stage.SelectingPlayer:
                            var index = _setup.PlayerNames.IndexOf(Name);
                            if (index > 0)
                                Name = _setup.PlayerNames[index - 1];
                            break;
                        case Stage.PickingGear:
                        case Stage.CreatingPlayer:
                        case Stage.NotJoined:
                        case Stage.Waiting:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                if (buttonsPressed.Contains(Buttons.LeftThumbstickRight))
                {
                    switch (_step)
                    {
                        case Stage.SelectingPlayer:
                            var index = _setup.PlayerNames.IndexOf(Name);
                            if (index < _setup.PlayerNames.Count - 1)
                                Name = _setup.PlayerNames[index + 1];
                            break;
                        case Stage.PickingGear:
                        case Stage.CreatingPlayer:
                        case Stage.NotJoined:
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
                if (Name == "Create New")
                {
                    _step = Stage.CreatingPlayer;
                    _creatingPlayer = true;
                }
                else
                {
                    _creatingPlayer = false;
                    _setup._quadrants.Where(q => q != this && q.Name == Name).ToList().ForEach(q => { q.Name = _setup.PlayerNames.First(n => n != Name); });
                    _setup.PlayerNames.Remove(Name);
                    _step = Stage.PickingGear;
                    Items = XmlStorage.GetPlayerInventory(Name);
                    _pickingGearStage = PickingGearStage.Boots;
                    _selectedItem = Items.First(i => i.Type == ItemTypes.Boots);
                }
            }

            private void CreatePlayer()
            {
                _step = Stage.PickingGear;
                Items = LootHelper.GetDefaultItems();
                _pickingGearStage = PickingGearStage.Boots;
                _selectedItem = Items.First(i => i.Type == ItemTypes.Boots);
            }

            private Item ItemBefore => Items.Take(Items.IndexOf(_selectedItem)).LastOrDefault(i => i.Type == CurrentGearType() && Gear.All(g => g != i));
            private Item ItemAfter => Items.Skip(Items.IndexOf(_selectedItem) + 1).FirstOrDefault(i => i.Type == CurrentGearType() && Gear.All(g => g != i));

            private enum Stage
            {
                NotJoined,
                SelectingPlayer,
                CreatingPlayer,
                PickingGear,
                Waiting
            }

            private ItemTypes CurrentGearType()
            {
                switch (_pickingGearStage)
                {
                    case PickingGearStage.Boots:
                        return ItemTypes.Boots;
                    case PickingGearStage.Robe:
                        return ItemTypes.Robe;
                    case PickingGearStage.Helm:
                        return ItemTypes.Hood;
                    case PickingGearStage.Glove:
                        return ItemTypes.Glove;
                    case PickingGearStage.Wand:
                        return ItemTypes.Wand;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private enum PickingGearStage
            {
                Boots,
                Robe,
                Helm,
                Glove,
                Wand
            }
        }
    }
}
