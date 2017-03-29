using System;

namespace Zerds.Items
{
    public class PotentialItem
    {
        public Item Item { get; set; }
        public double Chance { get; set; }

        public PotentialItem(Item item, double baseChanceFactor)
        {
            Item = item;
            switch (Item.Rarity)
            {
                case ItemRarities.Novice:
                    SetChances(0.5, 0.95, baseChanceFactor);
                    break;
                case ItemRarities.Apprentice:
                    SetChances(0.35, 0.6, baseChanceFactor);
                    break;
                case ItemRarities.Adept:
                    SetChances(0.2, 0.4, baseChanceFactor);
                    break;
                case ItemRarities.Master:
                    SetChances(0.12, 0.28, baseChanceFactor);
                    break;
                case ItemRarities.Legendary:
                    SetChances(0.03, 0.15, baseChanceFactor);
                    break;
                case ItemRarities.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Item.Rarity), Item.Rarity, null);
            }
        }

        private void SetChances(double min, double max, double chanceFactor)
        {
            Chance = Globals.Random.NextDouble() * (max - min) + min;
            Chance *= chanceFactor;
        }

        public Item Process()
        {
            var chance = Globals.Random.NextDouble();
            return chance > Chance ? null : Item;
        }
    }
}
