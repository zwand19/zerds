using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zerds.Constants;
using Zerds.GameObjects;
using Zerds.Input;
using Zerds.Menus;

namespace Zerds
{
    public class IntermissionScreen
    {
        private readonly List<Quadrant> _quadrants;

        private enum Screen
        {
            LevelRecap,
            Skills,
            FireSkills,
            FrostSkills,
            ArcaneSkills,
            AbilityUpgrade,
            Potion,
            Dead
        }

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
            public Screen Screen { get; private set; }
            private readonly MenuList _mainMenu;
            private MenuList _abilityUpgradeMenu;

            private const int MenuSidePadding = 100;
            private const int QuadrantPadding = 20;
            private const int MenuTopPadding = 150;
            private const int MenuBorder = 3;

            public Quadrant(PlayerIndex index)
            {
                _index = index;
                _player = Globals.GameState.Players.First(p => p.PlayerIndex == index);
                Ready = !_player.IsPlaying;
                Screen = !_player.IsPlaying || _player.Zerd.IsAlive ? Screen.LevelRecap : Screen.Dead;
                var width = (Globals.ViewportBounds.Width - MenuSidePadding * 2 - QuadrantPadding * 3) / 4;
                var height = Globals.ViewportBounds.Height - MenuTopPadding * 2;
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
                _mainMenu = new MenuList(new List<MenuListItem>
                {
                    new MenuListItem("Fire", () =>
                    {
                        Screen = Screen.FireSkills;
                        return true;
                    }),
                    new MenuListItem("Frost", () =>
                    {
                        Screen = Screen.FrostSkills;
                        return true;
                    }),
                    new MenuListItem("Arcane", () =>
                    {
                        Screen = Screen.ArcaneSkills;
                        return true;
                    }),
                    new MenuListItem($"Ability Upgrade ({GameplayConstants.AbilityUpgradeCost} gold)", BuyAbilityUpgrade),
                    new MenuListItem($"Buy Skill Point ({GameplayConstants.FloatingSkillPointCost} gold)", BuyFloatingSkillPoint)
                });
            }

            private bool BuyFloatingSkillPoint()
            {
                if (_player.Gold < GameplayConstants.FloatingSkillPointCost)
                    return false;
                _player.Gold -= GameplayConstants.FloatingSkillPointCost;
                _player.FloatingSkillPoints++;
                return true;
            }

            private bool BuyAbilityUpgrade()
            {
                if (_player.Gold < GameplayConstants.AbilityUpgradeCost)
                    return false;
                _player.Gold -= GameplayConstants.AbilityUpgradeCost;
                Level.AbilityUpgrades[_player] = AbilityUpgradeHelper.GetRandomUpgrades();
                Screen = Screen.AbilityUpgrade;
                return true;
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
                var top = 40f;
                switch (Screen)
                {
                    case Screen.LevelRecap:
                        if (Globals.GameState.Zerds.Count > 1)
                        {
                            Globals.SpriteDrawer.DrawText($"Killing Blows: {_player.Zerd.Stats.LevelKillingBlows.Count} ({Level.KillingBlowGold(_player)} Gold)",
                                new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                            top += 60f;
                            Globals.SpriteDrawer.DrawText($"Team Kills: {Globals.GameState.Zerds.Sum(z => z.Stats.LevelKillingBlows.Count)} ({Level.TotalEnemiesKilledGold(_player)} Gold)",
                                new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                            top += 60f;
                        }
                        else
                        {
                            Globals.SpriteDrawer.DrawText($"Enemies Killed: {_player.Zerd.Stats.LevelKillingBlows.Count} ({Level.TotalEnemiesKilledGold(_player) + Level.KillingBlowGold(_player)} Gold)",
                                new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                            top += 60f;
                        }
                        Globals.SpriteDrawer.DrawText($"Max Combo: {_player.Zerd.Stats.MaxLevelCombo} ({Level.ComboGold(_player)} Gold)", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                        top += 60f;
                        if (_player.Zerd.Stats.PerfectRound)
                        {
                            Globals.SpriteDrawer.DrawText($"Perfect Round ({GameplayConstants.PerfectRoundBonus} Gold)", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                            top += 60f;
                        }
                        else if (_player.Zerd.Stats.NoMissRound)
                        {
                            Globals.SpriteDrawer.DrawText($"No Misses ({GameplayConstants.NoMissesBonus} Gold)", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                            top += 60f;
                        }
                        else if (_player.Zerd.Stats.CleanRound)
                        {
                            Globals.SpriteDrawer.DrawText($"Clean Round ({GameplayConstants.CleanRoundBonus} Gold)", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                            top += 60f;
                        }
                        Globals.SpriteDrawer.DrawText($"Level Bonus: {Level.LevelGold()} Gold", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                        top += 60f;
                        Globals.SpriteDrawer.DrawText($"Total: {Level.TotalLevelGold(_player)} Gold", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Globals.GoldColor);
                        Globals.SpriteDrawer.DrawText("Press A to Continue", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Bottom - 40f), 17f, Globals.ContinueColor);
                        return;
                    case Screen.Skills:
                        Globals.SpriteDrawer.DrawText($"Gold: {_player.Gold}", new Vector2(_bounds.Center.X, _bounds.Top + 25f), 17f, Globals.GoldColor);
                        _mainMenu.Items[0].Text = $"Fire (Pts To Spend: {_player.Skills.FireSkillTree.PointsAvailable})";
                        _mainMenu.Items[1].Text = $"Frost (Pts To Spend: {_player.Skills.FrostSkillTree.PointsAvailable})";
                        _mainMenu.Items[2].Text = $"Arcane (Pts To Spend: {_player.Skills.ArcaneSkillTree.PointsAvailable})";
                        Globals.SpriteDrawer.DrawText($"Floating Points: {_player.FloatingSkillPoints}", new Vector2(_bounds.Center.X, _bounds.Bottom - 70f), 17f, Color.White);
                        _mainMenu.Draw(new Vector2(_bounds.X + 20, _bounds.Y + 70), 17f, 50f, Color.White, new Color(200, 200, 200));
                        Globals.SpriteDrawer.DrawText("Press Start when ready.", new Vector2(_bounds.Center.X, _bounds.Bottom - 30f), 20f, Globals.ContinueColor);
                        return;
                    case Screen.FireSkills:
                        _player.Skills.FireSkillTree.Draw(_bounds);
                        return;
                    case Screen.FrostSkills:
                        _player.Skills.FrostSkillTree.Draw(_bounds);
                        return;
                    case Screen.ArcaneSkills:
                        _player.Skills.ArcaneSkillTree.Draw(_bounds);
                        return;
                    case Screen.AbilityUpgrade:
                        Globals.SpriteDrawer.DrawText("UPGRADE", new Vector2(_bounds.Left + _bounds.Width / 2, _bounds.Top + 40f), 40f, Color.White);
                        var height = (_bounds.Height - 180) / 3;
                        DrawAbilityUpgrade(Level.AbilityUpgrades[_player].Item1, new Rectangle(_bounds.Left + 20, _bounds.Top + 100, _bounds.Width - 40, height), _abilityUpgradeMenu.Selected == _abilityUpgradeMenu.Items[0]);
                        DrawAbilityUpgrade(Level.AbilityUpgrades[_player].Item2, new Rectangle(_bounds.Left + 20, _bounds.Top + height + 110, _bounds.Width - 40, height), _abilityUpgradeMenu.Selected == _abilityUpgradeMenu.Items[1]);
                        DrawAbilityUpgrade(Level.AbilityUpgrades[_player].Item3, new Rectangle(_bounds.Left + 20, _bounds.Top + 2 * height + 120, _bounds.Width - 40, height), _abilityUpgradeMenu.Selected == _abilityUpgradeMenu.Items[2]);
                        Globals.SpriteDrawer.DrawText("Press A to Choose", new Vector2(_bounds.Left + _bounds.Width / 2, _bounds.Bottom - 30f), 20f, Globals.ContinueColor);
                        return;
                    case Screen.Potion:
                        return;
                    case Screen.Dead:
                        Globals.SpriteDrawer.DrawText("You Were Slain!", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 20f, Color.White);
                        top += 60f;
                        Globals.SpriteDrawer.DrawText("Revival Penalty For You:", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                        top += 60f;
                        Globals.SpriteDrawer.DrawText($"-{DifficultyConstants.RevivalSelfPenalty}% Max Health", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                        top += 60f;
                        Globals.SpriteDrawer.DrawText($"-{DifficultyConstants.RevivalSelfPenalty}% Damage", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                        top += 60f;
                        Globals.SpriteDrawer.DrawText($"-{DifficultyConstants.RevivalSelfPenalty}% Speed", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                        top += 60f;
                        if (Globals.GameState.Zerds.Count > 1)
                        {
                            Globals.SpriteDrawer.DrawText("Revival Penalty For Teammates:", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                            top += 60f;
                            Globals.SpriteDrawer.DrawText($"-{DifficultyConstants.RevivalTeammatePenalty}% Max Health", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                            top += 60f;
                            Globals.SpriteDrawer.DrawText($"-{DifficultyConstants.RevivalTeammatePenalty}% Damage", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                            top += 60f;
                            Globals.SpriteDrawer.DrawText($"-{DifficultyConstants.RevivalTeammatePenalty}% Speed", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Top + top), 17f, Color.White);
                        }
                        Globals.SpriteDrawer.DrawText("Press Start to Revive", new Vector2(_bounds.X + _bounds.Width / 2.0f, _bounds.Bottom - 40f), 17f, Globals.ContinueColor);
                        return;
                }
            }

            private void DrawAbilityUpgrade(AbilityUpgrade upgrade, Rectangle bounds, bool selected)
            {
                Globals.SpriteDrawer.DrawRect(bounds, selected ? Color.White : new Color(30, 30, 30));
                Globals.SpriteDrawer.DrawRect(new Rectangle(bounds.Left + 8, bounds.Top + 8, bounds.Width - 16, bounds.Height - 16));
                Globals.SpriteDrawer.Draw(upgrade.Texture, new Rectangle(bounds.Center.X - 32, bounds.Top + 20, 64, 64), color: Color.White);
                Globals.SpriteDrawer.DrawText(upgrade.Text1, new Vector2(bounds.Center.X, bounds.Center.Y + 30), 16f, Color.White);
                Globals.SpriteDrawer.DrawText(upgrade.Text2, new Vector2(bounds.Center.X, bounds.Center.Y + 56), 16f, Color.White);
            }

            public void Update()
            {
                if (ControllerService.Controllers[_index].IsPressed(Buttons.Start))
                {
                    switch (Screen)
                    {
                        case Screen.Skills:
                        case Screen.FireSkills:
                        case Screen.FrostSkills:
                        case Screen.ArcaneSkills:
                            Ready = true;
                            break;
                        case Screen.LevelRecap:
                        case Screen.AbilityUpgrade:
                        case Screen.Potion:
                            break;
                        case Screen.Dead:
                            Revive();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                if (ControllerService.Controllers[_index].IsPressed(Buttons.A))
                {
                    if (Screen == Screen.LevelRecap)
                    {
                        Screen = Screen.AbilityUpgrade;
                        _abilityUpgradeMenu = new MenuList(new List<MenuListItem>
                        {
                            new MenuListItem("", () => UpgradeSelect(Level.AbilityUpgrades[_player].Item1)),
                            new MenuListItem("", () => UpgradeSelect(Level.AbilityUpgrades[_player].Item2)),
                            new MenuListItem("", () => UpgradeSelect(Level.AbilityUpgrades[_player].Item3))
                        });
                        return; // return so we don't call _abilityUpgradeMenu.Update and purchase an ability
                    }
                }
                if (ControllerService.Controllers[_index].IsPressed(Buttons.B))
                {
                    switch (Screen)
                    {
                        case Screen.ArcaneSkills:
                        case Screen.FireSkills:
                        case Screen.FrostSkills:
                            Screen = Screen.Skills;
                            break;
                    }
                }
                switch (Screen)
                {
                    case Screen.Skills:
                        _mainMenu.Update(_index);
                        return;
                    case Screen.FireSkills:
                        _player.Skills.FireSkillTree.Update();
                        return;
                    case Screen.FrostSkills:
                        _player.Skills.FrostSkillTree.Update();
                        return;
                    case Screen.ArcaneSkills:
                        _player.Skills.ArcaneSkillTree.Update();
                        return;
                    case Screen.AbilityUpgrade:
                        _abilityUpgradeMenu.Update(_index);
                        return;
                }
            }

            private bool UpgradeSelect(AbilityUpgrade upgrade)
            {
                _player.Zerd.Stats.AbilityUpgradesPurchased++;
                _player.AbilityUpgrades[upgrade.Type] += (100 - _player.AbilityUpgrades[upgrade.Type]) * upgrade.Amount / 100;
                Screen = Screen.Skills;
                switch (upgrade.Type)
                {
                    case AbilityUpgradeType.MaxHealth:
                        _player.Zerd.MaxHealth *= 1 + upgrade.Amount / 100f;
                        _player.Zerd.Health *= 1 + upgrade.Amount / 100f;
                        return true;
                    case AbilityUpgradeType.MaxMana:
                        _player.Zerd.MaxMana *= 1 + upgrade.Amount / 100f;
                        _player.Zerd.Mana *= 1 + upgrade.Amount / 100f;
                        return true;
                    default:
                        return true;
                }
            }

            private void Revive()
            {
                Globals.GameState.Zerds.ForEach(z =>
                {
                    if (z == _player.Zerd)
                    {
                        z.BaseSpeed *= 1 - DifficultyConstants.RevivalSelfPenalty / 100f;
                        z.MaxHealth *= 1 - DifficultyConstants.RevivalSelfPenalty / 100f;
                        z.Health = z.MaxHealth;
                        z.Mana = z.MaxMana;
                        z.Deaths++;
                    }
                    else
                    {
                        z.BaseSpeed *= 1 - DifficultyConstants.RevivalTeammatePenalty / 100f;
                        z.MaxHealth *= 1 - DifficultyConstants.RevivalTeammatePenalty / 100f;
                        z.Health = z.MaxHealth;
                        z.Mana = z.MaxMana;
                        z.TeammateDeaths++;
                    }
                });
                Screen = Screen.LevelRecap;
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

        public bool Ready => _quadrants.All(q => q.Ready || q.Screen == Screen.Dead);
    }
}
