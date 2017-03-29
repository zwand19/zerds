using System;

namespace Zerds.Items
{
    public class RobeItem : Item
    {
        public int Thorns { get; set; }
        public float KnockbackReduction { get; set; }
        private readonly ChestType _chestItemType;

        public RobeItem(ItemRarities rarity, double color = -1, double itemType = -1) : base(rarity, ItemTypes.Robe, "robe")
        {
            color = color < 0 ? Globals.Random.NextDouble() : color;
            if (color < 0.25)
            {
                Name = $"{rarity} Red Robe";
                AnimationFile = "Zerds/red-robe.png";
            }
            else if (color < 0.5)
            {
                Name = $"{rarity} Blue Robe";
                AnimationFile = "Zerds/blue-robe.png";
            }
            else if (color < 0.7)
            {
                Name = $"{rarity} Green Robe";
                AnimationFile = "Zerds/green-robe.png";
            }
            else if (color < 0.9)
            {
                Name = $"{rarity} Purple Robe";
                AnimationFile = "Zerds/purple-robe.png";
            }
            else
            {
                Name = $"{rarity} Dark Robe";
                AnimationFile = "Zerds/dark-robe.png";
            }
            itemType = itemType < 0 ? Globals.Random.NextDouble() : itemType;
            if (itemType < 0.5)
            {
                _chestItemType = ChestType.Knockback;
                KnockbackReduction = 0.05f + 0.03f * (int)rarity;
                KnockbackReduction *= 0.9f + (float)Globals.Random.NextDouble() * 0.2f;
            }   
            else
            {
                _chestItemType = ChestType.Thorns;
                Thorns = 1 + (int) rarity;
                if (itemType < 0.65)
                    Thorns++;
                else if (itemType < 0.8 && Thorns > 1)
                    Thorns--;
            }
        }

        public override string Description1()
        {
            switch (_chestItemType)
            {
                case ChestType.Thorns:
                    return $"{Thorns} Thorns Damage";
                case ChestType.Knockback:
                    return $"{KnockbackReduction * 100:F1}% Knockback Reduction";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string Description2()
        {
            return "";
        }

        public override string ToSaveString()
        {
            switch (_chestItemType)
            {
                case ChestType.Thorns:
                    return $"{(int) _chestItemType}-{Thorns}";
                case ChestType.Knockback:
                    return $"{(int) _chestItemType}-{KnockbackReduction * 100:F1}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void LoadSaveString(string str)
        {
            var type = (ChestType) int.Parse(str.Substring(0, str.IndexOf("-", StringComparison.Ordinal)));
            var dashIndex = str.IndexOf("-", StringComparison.Ordinal);
            switch (type)
            {
                case ChestType.Thorns:
                    Thorns = int.Parse(str.Substring(dashIndex + 1));
                    break;
                case ChestType.Knockback:
                    KnockbackReduction = float.Parse(str.Substring(dashIndex + 1)) / 100f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string InformalName()
        {
            return $"{Rarity} Robe";
        }

        private enum ChestType
        {
            Thorns = 1,
            Knockback = 2
        }
    }
}
