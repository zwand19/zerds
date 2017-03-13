using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Zerds.Consumables;
using Zerds.Data;
using Zerds.GameObjects;
using Zerds.Input;
using Zerds.Items;

namespace Zerds
{
    public class PostGameScreen
    {
        private Player _currentPlayer;
        private readonly List<Player> _playersProcessed;
        private readonly Rectangle _recapBounds;
        private readonly Rectangle _treasureBounds;
        private readonly Rectangle _treasureInfoBounds;
        public bool Ready { get; set; }

        private readonly Texture2D _highlightTexture;
        private readonly int _numTreasuresWide;
        private Item _reward;
        private bool _confirmedReward = true;
        private TreasureChest _selectedChest;
        private const int SidePadding = 80;
        private const int MenuPadding = 50;
        private const int MiddlePadding = 15;
        private const float RecapMenuSize = 0.4f;
        private const float TreasureWidth = 80f;
        private const float TreasureHeight = 70;
        private bool DoneOpeningTreasure => _currentPlayer == null || !_currentPlayer.Zerd.TreasureChests.Any() || !_currentPlayer.Zerd.Keys.Any();

        public PostGameScreen()
        {
            _playersProcessed = new List<Player>();
            _currentPlayer = Globals.GameState.Zerds.First().Player;
            _selectedChest = _currentPlayer.Zerd.TreasureChests.FirstOrDefault();
            var recapMenuWidth = (int)(Globals.ViewportBounds.Width * RecapMenuSize - SidePadding - MiddlePadding);
            _recapBounds = new Rectangle(SidePadding, SidePadding, recapMenuWidth, Globals.ViewportBounds.Height - 2 * SidePadding);
            var treasureMenuWidth = Globals.ViewportBounds.Width - 2 * SidePadding - 2 * MiddlePadding - recapMenuWidth;
            var left = SidePadding + recapMenuWidth + MiddlePadding * 2;
            var treasureMenuHeight = (Globals.ViewportBounds.Height - SidePadding * 2 - MiddlePadding * 2) / 2;
            _treasureInfoBounds = new Rectangle(left, SidePadding, treasureMenuWidth, treasureMenuHeight);
            _treasureBounds = new Rectangle(left, SidePadding + treasureMenuHeight + MiddlePadding * 2, treasureMenuWidth, treasureMenuHeight);
            _numTreasuresWide = (int)Math.Floor((_treasureBounds.Width - MenuPadding * 2) / TreasureWidth);
            Globals.GameState.Zerds.ForEach(z =>
            {
                z.Width *= 2;
                z.Height *= 2;
                z.X = _recapBounds.Center.X;
                z.Y = _recapBounds.Top + MenuPadding + z.Height / 2;
                z.Health = z.MaxHealth;
                z.ZerdAnimations.ResetAll();
                z.Facing = new Vector2(0, -1);
                z.Keys.ForEach(k =>
                {
                    k.Width *= 0.75f;
                    k.Height *= 0.75f;
                });
            });
            _highlightTexture = new Texture2D(Globals.SpriteDrawer.GraphicsDevice, 1, 1);
            _highlightTexture.SetData(new[] { new Color(0.15f, 0.55f, 0.15f, 0.45f) });
        }
        
        public void Update()
        {
            if (_currentPlayer == null)
                return;
            if (ControllerService.ButtonPressed(_currentPlayer.PlayerIndex, Buttons.A))
            {
                if (_confirmedReward)
                    ProcessTreasureSelection();
                else
                    _confirmedReward = true;
            }

            if (!_confirmedReward) return;

            if (ControllerService.ButtonPressed(_currentPlayer.PlayerIndex, Buttons.Start) && DoneOpeningTreasure)
            {
                NextPlayer();
                return;
            }

            if (_selectedChest == null) return;

            var coords = TreasureCoords(_selectedChest);
            TreasureChest chest = null;
            if (ControllerService.ButtonPressed(_currentPlayer.PlayerIndex, Buttons.LeftThumbstickRight))
                chest = _currentPlayer.Zerd.TreasureChests.FirstOrDefault(t => TreasureCoords(t).Item1 == coords.Item1 + 1 && TreasureCoords(t).Item2 == coords.Item2);
            if (ControllerService.ButtonPressed(_currentPlayer.PlayerIndex, Buttons.LeftThumbstickLeft))
                chest = _currentPlayer.Zerd.TreasureChests.FirstOrDefault(t => TreasureCoords(t).Item1 == coords.Item1 - 1 && TreasureCoords(t).Item2 == coords.Item2);
            if (ControllerService.ButtonPressed(_currentPlayer.PlayerIndex, Buttons.LeftThumbstickUp))
                chest = _currentPlayer.Zerd.TreasureChests.FirstOrDefault(t => TreasureCoords(t).Item1 == coords.Item1 && TreasureCoords(t).Item2 == coords.Item2 - 1);
            if (ControllerService.ButtonPressed(_currentPlayer.PlayerIndex, Buttons.LeftThumbstickDown))
                chest = _currentPlayer.Zerd.TreasureChests.FirstOrDefault(t => TreasureCoords(t).Item1 == coords.Item1 && TreasureCoords(t).Item2 == coords.Item2 + 1);

            _selectedChest = chest ?? _selectedChest;
        }

        public void Draw()
        {
            Globals.Map.Draw();
            Globals.SpriteDrawer.Begin(blendState: BlendState.AlphaBlend);
            Globals.SpriteDrawer.DrawRect(_recapBounds, new Color(Color.Black, 0.75f));
            Globals.SpriteDrawer.DrawRect(_treasureBounds, new Color(Color.Black, 0.75f));
            Globals.SpriteDrawer.DrawRect(_treasureInfoBounds, new Color(Color.Black, 0.75f));
            _currentPlayer.Zerd.Draw();
            DrawPlayerStats();
            DrawKeys();
            DrawTreasureChests();
            DrawTreasureChestInfo();
            if (!_confirmedReward) DrawReward();
            Globals.SpriteDrawer.End();
        }

        private void ProcessTreasureSelection()
        {
            if (!_currentPlayer.Zerd.Keys.Any() || _selectedChest == null) return;
            var item = _selectedChest.PotentialItems[0].Process() ?? _selectedChest.PotentialItems[1].Process() ?? _selectedChest.PotentialItems[2].Process();
            if (item != null)
                _currentPlayer.Items.Add(item);

            _confirmedReward = false;
            _reward = item;
            _currentPlayer.Zerd.Keys.RemoveAt(0);
            _currentPlayer.Zerd.TreasureChests.Remove(_selectedChest);
            _selectedChest = _currentPlayer.Zerd.TreasureChests.First();
        }

        private Tuple<int, int> TreasureCoords(TreasureChest chest)
        {
            return new Tuple<int, int>(_currentPlayer.Zerd.TreasureChests.IndexOf(chest) % _numTreasuresWide, _currentPlayer.Zerd.TreasureChests.IndexOf(chest) / _numTreasuresWide);
        }

        private void DrawReward()
        {
            var bounds = Helpers.CreateRect(Globals.ViewportBounds.Width / 2 - 200, Globals.ViewportBounds.Height / 2 - 200, 400, 400);
            Globals.SpriteDrawer.DrawRect(bounds, Color.Black);
            if (_reward == null)
            {
                Globals.SpriteDrawer.DrawText("This chest is empty.", new Vector2(bounds.Center.X, bounds.Top + 50f), 20f, Color.White);
            }
            else
            {
                Globals.SpriteDrawer.DrawText(_reward.ToString(), new Vector2(bounds.Center.X, bounds.Top + 50f), 20f, _reward.TextColor);
                _reward.Draw(bounds.Center.X, bounds.Center.Y - 30f, 70, 70);
                var y = bounds.Center.Y + 50f;
                foreach (var buff in _reward.AbilityUpgrades)
                {
                    Globals.SpriteDrawer.DrawText(buff.ToString(), new Vector2(bounds.Center.X, y), 14f, Color.White);
                    y += 22f;
                }
            }
            Globals.SpriteDrawer.DrawText("Press A to Continue", new Vector2(bounds.Center.X, bounds.Bottom - 50f), 20f, Globals.ContinueColor);
        }

        private void DrawPlayerStats()
        {
            const float spacing = 60f;
            var top = _recapBounds.Top + MenuPadding + Globals.GameState.Zerds.First().Height + spacing;
            Globals.SpriteDrawer.DrawText($"Killed on Level {Level.CurrentLevel}", new Vector2(_recapBounds.Center.X, top), 20f, Color.White);
            Globals.SpriteDrawer.DrawText($"Killing Blows: {_currentPlayer.Zerd.Stats.KillingBlows}", new Vector2(_recapBounds.Center.X, top + spacing), 20f, Color.White);
            Globals.SpriteDrawer.DrawText($"Damage Dealt: {Math.Floor(_currentPlayer.Zerd.Stats.DamageDealt)}", new Vector2(_recapBounds.Center.X, top + spacing * 2), 20f, Color.White);
            Globals.SpriteDrawer.DrawText($"Damage Taken: {Math.Floor(_currentPlayer.Zerd.Stats.DamageTaken)}", new Vector2(_recapBounds.Center.X, top + spacing * 3), 20f, Color.White);
            Globals.SpriteDrawer.DrawText($"Largest Combo: {_currentPlayer.Zerd.Stats.MaxCombo}", new Vector2(_recapBounds.Center.X, top + spacing * 4), 20f, Color.White);
            Globals.SpriteDrawer.DrawText($"Gold Earned: {_currentPlayer.Zerd.Stats.GoldEarned}", new Vector2(_recapBounds.Center.X, top + spacing * 5), 20f, Color.White);
            Globals.SpriteDrawer.DrawText($"Skill Points Spent: {_currentPlayer.Zerd.Stats.SkillPointsSpent}", new Vector2(_recapBounds.Center.X, top + spacing * 6), 20f, Color.White);
            Globals.SpriteDrawer.DrawText($"Ability Upgrades Bought: {_currentPlayer.Zerd.Stats.AbilityUpgradesPurchased}", new Vector2(_recapBounds.Center.X, top + spacing * 7), 20f, Color.White);
        }

        private void DrawKeys()
        {
            var drawX = _treasureBounds.Left + MenuPadding;
            var drawY = _treasureBounds.Top + MenuPadding;
            foreach (var key in _currentPlayer.Zerd.Keys)
            {
                key.X = drawX;
                key.Y = drawY;
                key.Draw();
                drawX += (int)key.Width + 20;
            }
        }

        private void DrawTreasureChests()
        {
            var index = 0;
            var drawX = _treasureBounds.Left + MenuPadding + 0f;
            var drawY = _treasureBounds.Top + MenuPadding + 60f;
            var gapSize = (_treasureBounds.Width - MenuPadding * 2 - _numTreasuresWide * TreasureWidth) / (_numTreasuresWide - 1);
            foreach (var chest in _currentPlayer.Zerd.TreasureChests)
            {
                chest.X = drawX;
                chest.Y = drawY;
                if (_selectedChest == chest) //highlight
                    Globals.SpriteDrawer.Draw(_highlightTexture, Helpers.CreateRect(drawX - _selectedChest.Width / 2 + 15, drawY - _selectedChest.Height / 2 + 15, _selectedChest.Width, _selectedChest.Height), Color.White * 0.65f);
                chest.Draw();
                drawX += gapSize + TreasureWidth;
                index++;
                if (index % _numTreasuresWide == 0)
                {
                    drawY += TreasureHeight;
                    drawX = _treasureBounds.Left + MenuPadding;
                }
            }
        }

        private void DrawTreasureChestInfo()
        {
            if (_selectedChest == null) return;
            var top = _treasureInfoBounds.Top + MenuPadding;
            Globals.SpriteDrawer.DrawText("Chest Info", new Vector2(_treasureInfoBounds.Center.X, top), 24f, Color.White);
            Globals.SpriteDrawer.DrawText($"{_selectedChest.Chances()[0] * 100:##.#}% Chance of {_selectedChest.PotentialItems[0].Item}", new Vector2(_treasureInfoBounds.Center.X, top + 50f), 18f, Color.White);
            Globals.SpriteDrawer.DrawText($"{_selectedChest.Chances()[1] * 100:##.#}% Chance of {_selectedChest.PotentialItems[1].Item}", new Vector2(_treasureInfoBounds.Center.X, top + 100f), 18f, Color.White);
            Globals.SpriteDrawer.DrawText($"{_selectedChest.Chances()[2] * 100:##.#}% Chance of {_selectedChest.PotentialItems[2].Item}", new Vector2(_treasureInfoBounds.Center.X, top + 200f), 18f, Color.White);
            Globals.SpriteDrawer.DrawText($"{_selectedChest.Chances()[3] * 100:##.#}% Chance of no item", new Vector2(_treasureInfoBounds.Center.X, top + 150f), 18f, Color.White);
            var msg = DoneOpeningTreasure ? "Press Start to Continue" : "Press A to unlock";
            Globals.SpriteDrawer.DrawText(msg, new Vector2(_treasureInfoBounds.Center.X, _treasureInfoBounds.Bottom - MenuPadding - 40f), 20f, Globals.ContinueColor);
        }

        private void NextPlayer()
        {
            _currentPlayer.Save();
            _playersProcessed.Add(_currentPlayer);
            _currentPlayer = Globals.GameState.Zerds.FirstOrDefault(z => !_playersProcessed.Contains(z.Player))?.Player;
            _selectedChest = _currentPlayer?.Zerd.TreasureChests.FirstOrDefault();
            Ready = _currentPlayer == null;
        }
    }
}
