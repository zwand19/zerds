using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Constants;

namespace Zerds.GameObjects
{
    public static class Level
    {
        public static Dictionary<Player, Tuple<AbilityUpgrade, AbilityUpgrade, AbilityUpgrade>> AbilityUpgrades { get; set; }
        public static int CurrentLevel { get; set; }
        public static TimeSpan TimeRemaining { get; set; }
        public static TimeSpan TimeSinceLosing { get; set; }
        public static TimeSpan TimeIntoLevel => GameplayConstants.LevelLength - TimeRemaining;
        public static Func<bool> GameOverFunc;
        private static bool _levelHasEnded;

        public static void Initialize(List<Player> players)
        {
            _levelHasEnded = false;
            CurrentLevel = 1;
            AbilityUpgrades = new Dictionary<Player, Tuple<AbilityUpgrade, AbilityUpgrade, AbilityUpgrade>>
            {
                {players[0], new Tuple<AbilityUpgrade, AbilityUpgrade, AbilityUpgrade>(null, null, null)},
                {players[1], new Tuple<AbilityUpgrade, AbilityUpgrade, AbilityUpgrade>(null, null, null)},
                {players[2], new Tuple<AbilityUpgrade, AbilityUpgrade, AbilityUpgrade>(null, null, null)},
                {players[3], new Tuple<AbilityUpgrade, AbilityUpgrade, AbilityUpgrade>(null, null, null)}
            };
            players.ForEach(p =>
            {
                AbilityUpgrades[p] = new Tuple<AbilityUpgrade, AbilityUpgrade, AbilityUpgrade>(AbilityUpgradeHelper.GetRandomUpgrade(), AbilityUpgradeHelper.GetRandomUpgrade(),
                    AbilityUpgradeHelper.GetRandomUpgrade());
            });
        }

        public static void StartLevel()
        {
            _levelHasEnded = false;
            TimeRemaining = GameplayConstants.LevelLength;
            Globals.GameState.Players.ForEach(p =>
            {
                p.FloatingSkillPoints += GameplayConstants.FloatingPointsPerLevel;
                p.Skills.ArcaneSkillTree.PointsAvailable += GameplayConstants.SkillPointsPerLevel;
                p.Skills.FireSkillTree.PointsAvailable += GameplayConstants.SkillPointsPerLevel;
                p.Skills.FrostSkillTree.PointsAvailable += GameplayConstants.SkillPointsPerLevel;
                p.Zerd?.Stats.ResetLevel();
            });
        }

        public static int KillingBlowGold(Player player)
        {
            return player.Zerd.Stats.LevelKillingBlows.Sum(e => e == null || !EnemyConstants.EnemyGoldValues.ContainsKey(e.Type) ? 0 : EnemyConstants.EnemyGoldValues[e.Type]);
        }

        public static int TotalEnemiesKilledGold(Player player)
        {
            return Globals.GameState.Zerds.Sum(z => z.Stats.LevelKillingBlows.Count) * (GameplayConstants.BaseGoldPerEnemy + CurrentLevel * GameplayConstants.BonusGoldPerEnemyPerLevel);
        }

        public static int ComboGold(Player player)
        {
            return (int) (player.Zerd.Stats.MaxLevelCombo * GameplayConstants.GoldPerCombo * player.Zerd.SkillValue(SkillType.ComboMaster, true));
        }

        public static int LevelGold()
        {
            return GameplayConstants.BaseLevelGold + CurrentLevel * GameplayConstants.BonusGoldPerLevel;
        }

        public static int TotalLevelGold(Player player)
        {
            var gold = KillingBlowGold(player) + TotalEnemiesKilledGold(player) + ComboGold(player) + LevelGold();
            if (player.Zerd.Stats.PerfectRound)
                gold += GameplayConstants.PerfectRoundBonus;
            else if (player.Zerd.Stats.NoMissRound)
                gold += GameplayConstants.NoMissesBonus;
            else if (player.Zerd.Stats.CleanRound)
                gold += GameplayConstants.CleanRoundBonus;
            return gold;
        }

        public static void LevelComplete()
        {
            foreach (var player in Globals.GameState.Players.Where(p => p.IsPlaying))
            {
                var gold = TotalLevelGold(player);
                player.Zerd.Stats.GoldEarned += gold;
                player.Gold += gold;
                AbilityUpgrades[player] = new Tuple<AbilityUpgrade, AbilityUpgrade, AbilityUpgrade>(AbilityUpgradeHelper.GetRandomUpgrade(), AbilityUpgradeHelper.GetRandomUpgrade(),
                    AbilityUpgradeHelper.GetRandomUpgrade());
            }
            CurrentLevel++;
        }

        public static void Update(GameTime gameTime)
        {
            TimeRemaining = TimeRemaining.SubtractWithGameSpeed(gameTime.ElapsedGameTime);
            if (TimeRemaining < TimeSpan.Zero)
            {
                TimeRemaining = TimeSpan.Zero;
                if (!_levelHasEnded)
                {
                    _levelHasEnded = true;
                    Globals.GameState.Beings.ForEach(b => b.LevelEnded());
                }
            }
            if (Globals.GameState.Zerds.Any(z => z.IsAlive))
            {
                TimeSinceLosing = GameplayConstants.WaitTimeAfterLosing;
            }
            else
            {
                TimeSinceLosing = TimeSinceLosing.SubtractWithGameSpeed(gameTime.ElapsedGameTime);
                if (TimeSinceLosing < TimeSpan.Zero)
                    GameOver();
            }
        }

        private static void GameOver()
        {
            Globals.GameState.Zerds.ForEach(z =>
                { z.Stats.GameOver(); });
            GameOverFunc();
        }
    }
}
