using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.GameObjects;

namespace Zerds
{
    public static class Helpers
    {
        private static readonly Random Random = new Random();

        public static bool RandomChance(float chance)
        {
            return Random.NextDouble() < chance;
        }

        public static Rectangle CreateRect(double x, double y, double width, double height)
        {
            return new Rectangle((int) x, (int) y, (int) width, (int) height);
        }

        public static float RandomInRange(float min, float max)
        {
            return (float)Random.NextDouble() * (max - min) + min;
        }
        
        public static int SkillPoints(this Being being, SkillType type)
        {
            var zerd = being as Zerd;
            return zerd?.Player.Skills.Pts(type) ?? 0;
        }

        public static float SkillValue(this Being being, SkillType type, bool asPercentage)
        {
            var zerd = being as Zerd;
            if (zerd == null)
                return 0;
            var val = zerd.Player.Skills.Pts(type) * SkillConstants.Values[type].Stat;
            return asPercentage ? 1 + val / 100 : val;
        }

        public static float AbilityValue(this Being being, AbilityUpgradeType type, bool asPercentage = false)
        {
            var zerd = being as Zerd;
            if (zerd == null)
                return 0;
            var val = zerd.Player.AbilityUpgrades[type];
            val += zerd.Inventory.SelectMany(i => i.AbilityUpgrades).Where(i => i.Type == type).Sum(i => i.Amount);
            return asPercentage ? 1 + val / 100 : val;
        }

        public static void AddHealth(this Being b, float amt)
        {
            b.Health = MathHelper.Clamp(b.Health + amt, -1, b.MaxHealth);
        }

        public static TimeSpan Split(this TimeSpan span, int num)
        {
            return TimeSpan.FromMilliseconds(span.TotalMilliseconds / num);
        }
    }
}
