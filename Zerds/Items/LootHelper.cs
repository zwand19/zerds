using Zerds.Constants;
using Zerds.Consumables;
using Zerds.Entities;
using Zerds.GameObjects;

namespace Zerds.Items
{
    public static class LootHelper
    {
        public static void TryCreateTreasureChest(Enemy enemy)
        {
            var chance = GameplayConstants.BaseTreasureDropChance + GameplayConstants.TreasureDropChancePerLevel * Level.CurrentLevel;
            chance *= enemy.Worth() / GameplayConstants.ItemDropBaseEnemyWorth;
            if (!Helpers.RandomChance(chance))
                return;

            Globals.GameState.Items.Add(new TreasureChest(enemy));
        }

        public static void TryCreatePotion(Enemy enemy)
        {
            var chance = GameplayConstants.PotionDropChance;
            chance *= enemy.Worth() / GameplayConstants.ItemDropBaseEnemyWorth;
            if (enemy.Killer is Zerd)
                chance += enemy.Killer.SkillValue(SkillType.Guzzler, false) / 100f;
            if (!Helpers.RandomChance(chance))
                return;

            if (Helpers.RandomChance(0.5f))
                Globals.GameState.Items.Add(new HealthPotion(enemy));
            else
                Globals.GameState.Items.Add(new ManaPotion(enemy));
        }

        public static void TryCreateKey(Enemy enemy)
        {
            var chance = GameplayConstants.KeyDropChance;
            chance *= enemy.Worth() / GameplayConstants.ItemDropBaseEnemyWorth;
            if (!Helpers.RandomChance(chance))
                return;

            Globals.GameState.Items.Add(new Key(enemy));
        }
    }
}
