using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;

namespace Zerds.GameObjects
{
    public static class Level
    {
        public static Dictionary<Player, Tuple<AbilityUpgrade, AbilityUpgrade, AbilityUpgrade>> AbilityUpgrades { get; set; }
        public static int CurrentLevel { get; set; }
        public static TimeSpan TimeRemaining { get; set; }
        public static TimeSpan TimeIntoLevel => GameplayConstants.LevelLength - TimeRemaining;
        private static bool _levelHasEnded;

        public static void Initialize(List<Player> players)
        {
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
                if (p.Zerd != null) p.Zerd.MaxLevelCombo = 0;
                if (p.Zerd != null) p.Zerd.LevelEnemiesKilled = new List<Enemy>();
            });
        }

        public static int PersonalEnemiesKilledGold(Player player)
        {
            return player.Zerd.LevelEnemiesKilled.Sum(e => EnemyConstants.EnemyGoldValues[e.Type]);
        }

        public static int TotalEnemiesKilledGold(Player player)
        {
            return Globals.GameState.Zerds.Sum(z => z.LevelEnemiesKilled.Count) * (GameplayConstants.BaseGoldPerEnemy + CurrentLevel * GameplayConstants.BonusGoldPerEnemyPerLevel);
        }

        public static int ComboGold(Player player)
        {
            return (int) (player.Zerd.MaxLevelCombo * GameplayConstants.GoldPerCombo * player.Zerd.SkillValue(SkillType.ComboMaster, true));
        }

        public static int LevelGold()
        {
            return GameplayConstants.BaseLevelGold + CurrentLevel * GameplayConstants.BonusGoldPerLevel;
        }

        public static int TotalLevelGold(Player player)
        {
            return PersonalEnemiesKilledGold(player) + TotalEnemiesKilledGold(player) + ComboGold(player) + LevelGold();
        }

        public static void LevelComplete()
        {
            foreach (var player in Globals.GameState.Players.Where(p => p.IsPlaying))
            {
                player.Gold += TotalLevelGold(player);
                AbilityUpgrades[player] = new Tuple<AbilityUpgrade, AbilityUpgrade, AbilityUpgrade>(AbilityUpgradeHelper.GetRandomUpgrade(), AbilityUpgradeHelper.GetRandomUpgrade(),
                    AbilityUpgradeHelper.GetRandomUpgrade());
            }
            CurrentLevel++;
        }

        public static void Update(GameTime gameTime)
        {
            TimeRemaining -= gameTime.ElapsedGameTime;
            if (TimeRemaining < TimeSpan.Zero)
            {
                TimeRemaining = TimeSpan.Zero;
                if (!_levelHasEnded)
                {
                    _levelHasEnded = true;
                    Globals.GameState.Beings.ForEach(b => b.LevelEnded());
                }
            }
        }
    }
}
