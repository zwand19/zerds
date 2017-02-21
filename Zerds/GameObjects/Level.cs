using System;
using System.Collections.Generic;
using System.Linq;
using Zerds.Constants;

namespace Zerds.GameObjects
{
    public static class Level
    {
        public static Dictionary<Player, Tuple<AbilityUpgrade, AbilityUpgrade, AbilityUpgrade>> AbilityUpgrades { get; set; }
        public static int CurrentLevel { get; set; }

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

        public static int EnemiesKilledGold(Player player)
        {
            return player.Zerd.LevelEnemiesKilled * (GameplayConstants.GoldPerEnemy + CurrentLevel * GameplayConstants.BonusGoldPerEnemyPerLevel);
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
            return EnemiesKilledGold(player) + ComboGold(player) + LevelGold();
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
    
    }
}
