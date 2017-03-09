using System;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Factories;

namespace Zerds.GameObjects
{
    public static class AbilityUpgradeHelper
    {
        public static int DashDistanceUpgrade => (int) Helpers.RandomInRange(4f, 9f);
        public static int SprintSpeedUpgrade => (int) Helpers.RandomInRange(3f, 7f);
        public static int FireballManaUpgrade => (int) Helpers.RandomInRange(3f, 7f);
        public static int FireballDamageUpgrade => (int)Helpers.RandomInRange(3f, 6f);
        public static int IceballManaUpgrade => (int) Helpers.RandomInRange(2f, 5f);
        public static int IceballCritUpgrade => (int)Helpers.RandomInRange(2f, 5f);
        public static int LavaBlastDistanceUpgrade => (int) Helpers.RandomInRange(7f, 13f);
        public static int MovementSpeedUpgrade => (int) Helpers.RandomInRange(2f, 4f);
        public static int DamageTakenUpgrade => (int) Helpers.RandomInRange(1f, 4f);
        public static int HealthRegenUpgrade => (int) Helpers.RandomInRange(4f, 8f);
        public static int ManaRegenUpgrade => (int)Helpers.RandomInRange(4f, 7f);
        public static int MaxHealthUpgrade => (int)Helpers.RandomInRange(5f, 8f);
        public static int MaxManaUpgrade => (int)Helpers.RandomInRange(4f, 7f);

        public static AbilityUpgrade GetRandomUpgrade()
        {
            var upgrade = new Random().Next(13);
            int amt;
            switch (upgrade)
            {
                case 0:
                    amt = DashDistanceUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.DashDistance, "Increase the distance", $"of dash by {amt}%", amt);
                case 1:
                    amt = SprintSpeedUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.SprintSpeed, "Increase your sprint", $"speed by {amt}%", amt);
                case 2:
                    amt = FireballManaUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.FireballMana, "Reduce the mana cost", $"of fireball by {amt}%", amt);
                case 3:
                    amt = FireballDamageUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.FireballDamage, "Increase the damage", $"of fireball by {amt}%", amt);
                case 4:
                    amt = IceballCritUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.IceballCrit, "Increase the crit chance", $"of iceball by {amt}%", amt);
                case 5:
                    amt = IceballManaUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.IceballMana, "Reduce the mana cost" , $"of iceball by {amt}%", amt);
                case 6:
                    amt = LavaBlastDistanceUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.LavaBlastDistance, "Increase the range", $"of lava blast by {amt}%", amt);
                case 7:
                    amt = MovementSpeedUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.MovementSpeed, "Increase your movement speed", $"while not sprinting by {amt}%", amt);
                case 8:
                    amt = DamageTakenUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.DamageTaken, "Reduce all damage", $"taken by {amt}%", amt);
                case 9:
                    amt = HealthRegenUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.HealthRegen, "Increase health regen", $"by {amt}%", amt);
                case 10:
                    amt = ManaRegenUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.HealthRegen, "Increase mana regen", $"by {amt}%", amt);
                case 11:
                    amt = MaxHealthUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.MaxHealth, "Increase max health", $"by {amt}%", amt);
                case 12:
                    amt = MaxManaUpgrade;
                    return new AbilityUpgrade(AbilityUpgradeType.MaxHealth, "Increase max mana", $"by {amt}%", amt);
                default:
                    throw new Exception("Invalid upgrade");
            }
        }
    }

    public class AbilityUpgrade
    {
        public AbilityUpgradeType Type { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public int Amount { get; set; }
        public Texture2D Texture { get; set; }

        public AbilityUpgrade(AbilityUpgradeType type, string text1, string text2, int amt)
        {
            Type = type;
            Text1 = text1;
            Text2 = text2;
            Amount = amt;
            switch (type)
            {
                case AbilityUpgradeType.DamageTaken:
                    Texture = TextureCacheFactory.GetOnce("Icons/viking-shield.png");
                    return;
                case AbilityUpgradeType.DashDistance:
                    Texture = TextureCacheFactory.GetOnce("Icons/charging-bull.png");
                    return;
                case AbilityUpgradeType.FireballDamage:
                case AbilityUpgradeType.FireballMana:
                    Texture = TextureCacheFactory.GetOnce("Icons/fireball.png");
                    return;
                case AbilityUpgradeType.IceballCrit:
                case AbilityUpgradeType.IceballMana:
                    Texture = TextureCacheFactory.GetOnce("Icons/ice-bolt.png");
                    return;
                case AbilityUpgradeType.HealthRegen:
                    Texture = TextureCacheFactory.GetOnce("Icons/glass-heart.png");
                    return;
                case AbilityUpgradeType.ManaRegen:
                case AbilityUpgradeType.MaxMana:
                    Texture = TextureCacheFactory.GetOnce("Icons/pentagram-rose.png");
                    return;
                case AbilityUpgradeType.LavaBlastDistance:
                    Texture = TextureCacheFactory.GetOnce("Icons/lava_blast.png");
                    return;
                case AbilityUpgradeType.SprintSpeed:
                    Texture = TextureCacheFactory.GetOnce("Icons/sprint.png");
                    return;
                case AbilityUpgradeType.MovementSpeed:
                    Texture = TextureCacheFactory.GetOnce("Icons/walking-boot.png");
                    return;
                case AbilityUpgradeType.MaxHealth:
                    Texture = TextureCacheFactory.GetOnce("Icons/health-normal.png");
                    return;
                default:
                    throw new Exception("Unhandled upgrade type");
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case AbilityUpgradeType.DamageTaken:
                    return $"-{Amount}% Damage Taken";
                case AbilityUpgradeType.DashDistance:
                    return $"+{Amount}% Dash Distance";
                case AbilityUpgradeType.FireballDamage:
                    return $"+{Amount}% Fireball Damage";
                case AbilityUpgradeType.FireballMana:
                    return $"-{Amount}% Fireball Mana Cost";
                case AbilityUpgradeType.IceballCrit:
                    return $"+{Amount}% Iceball Crit Chance";
                case AbilityUpgradeType.IceballMana:
                    return $"-{Amount}% Iceball Mana Cost";
                case AbilityUpgradeType.HealthRegen:
                    return $"+{Amount}% Health Regen";
                case AbilityUpgradeType.ManaRegen:
                    return $"+{Amount}% Mana Regen";
                case AbilityUpgradeType.MaxMana:
                    return $"+{Amount}% Max Mana";
                case AbilityUpgradeType.LavaBlastDistance:
                    return $"+{Amount}% Lava Blast Distance";
                case AbilityUpgradeType.SprintSpeed:
                    return $"+{Amount}% Sprint Speed";
                case AbilityUpgradeType.MovementSpeed:
                    return $"+{Amount}% Movement Speed";
                case AbilityUpgradeType.MaxHealth:
                    return $"+{Amount}% Max Health";
                default:
                    throw new Exception("Unhandled upgrade type");
            }
        }
    }

    public enum AbilityUpgradeType
    {
        DashDistance,
        SprintSpeed,
        FireballMana,
        FireballDamage,
        IceballMana,
        IceballCrit,
        LavaBlastDistance,
        MovementSpeed,
        DamageTaken,
        HealthRegen,
        ManaRegen,
        MaxHealth,
        MaxMana
    }
}
