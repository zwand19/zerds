using System.Collections.Generic;
using System.Linq;
using Zerds.Constants;
using Zerds.Consumables;
using Zerds.Entities;
using Zerds.Entities.Enemies;
using Zerds.GameObjects;

namespace Zerds.Items
{
    public static class LootHelper
    {
        public static void TryCreateTreasureChest(Enemy enemy)
        {
            if (enemy is SkeletonKing)
            {
                Globals.GameState.Zerds.Where(z => z.IsAlive).ToList().ForEach(z =>
                {
                    var treasure = new TreasureChest(enemy);
                    treasure.X = z.X;
                    treasure.Y = z.Y;
                    Globals.GameState.Items.Add(treasure);
                });
                return;
            }
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
            if (enemy is SkeletonKing)
            {
                Globals.GameState.Zerds.Where(z => z.IsAlive).ToList().ForEach(z =>
                {
                    var key = new Key(enemy);
                    key.X = z.X;
                    key.Y = z.Y;
                    Globals.GameState.Items.Add(key);
                });
                return;
            }
            var chance = GameplayConstants.KeyDropChance;
            chance *= enemy.Worth() / GameplayConstants.ItemDropBaseEnemyWorth;
            if (!Helpers.RandomChance(chance))
                return;

            Globals.GameState.Items.Add(new Key(enemy));
        }

        public static List<Item> GetDefaultItems()
        {
            return new List<Item>
            {
                new WandItem(ItemRarities.Novice),
                new WandItem(ItemRarities.Novice),
                new BootItem(ItemRarities.Novice),
                new BootItem(ItemRarities.Novice),
                new RobeItem(ItemRarities.Novice, -1, 0.1),
                new RobeItem(ItemRarities.Novice, -1, 0.9),
                new HoodItem(ItemRarities.Novice, 0.1),
                new HoodItem(ItemRarities.Novice, 0.1),
                new GloveItem(ItemRarities.Novice, 0.1),
                new GloveItem(ItemRarities.Novice, 0.1),
                new GloveItem(ItemRarities.Novice, 0.1)
            };
        }
    }
}
