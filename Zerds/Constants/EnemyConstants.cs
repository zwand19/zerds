using System;

namespace Zerds.Constants
{
    public static class EnemyConstants
    {
        private const int BaseHealth = 20;
        private const float BaseRegen = 0.25f;
        private const float BaseSpeed = 150f;
        private static int GetHealth(float factor) => (int)(BaseHealth * factor);

        public class EnemyProperties
        {
            public int MinHealth { get; set; }
            public int MinMana { get; set; }
            public int MaxHealth { get; set; }
            public int MaxMana { get; set; }
            public float HealthRegen { get; set; }
            public float ManaRegen { get; set; }
            public float MinSpeed { get; set; }
            public float MaxSpeed { get; set; }
            public float CritChance { get; set; }
        }
        
        public static EnemyProperties GetZombieProperties()
        {
            return new EnemyProperties
            {
                MinHealth = GetHealth(0.8f),
                HealthRegen = BaseRegen,
                MinSpeed = BaseSpeed * 0.6f,
                MaxHealth = GetHealth(1.2f),
                MaxSpeed = BaseSpeed * 0.85f,
                CritChance = 0.06f
            };    
        }

        public static EnemyProperties GetArcherProperties()
        {
            return new EnemyProperties
            {
                MinHealth = GetHealth(1.1f),
                HealthRegen = BaseRegen * 2,
                MinSpeed = BaseSpeed * 0.85f,
                MaxHealth = GetHealth(1.4f),
                MaxSpeed = BaseSpeed * 1.15f,
                CritChance = 0.2f
            };
        }

        public static EnemyProperties GetDogProperties()
        {
            return new EnemyProperties
            {
                MinHealth = GetHealth(0.4f),
                HealthRegen = BaseRegen,
                MinSpeed = BaseSpeed * 1.4f,
                MaxHealth = GetHealth(0.7f),
                MaxSpeed = BaseSpeed * 1.6f,
                CritChance = 0.25f
            };
        }
        public const float ArcherArrowSpeed = 675f;
        public const float ArcherArrowLength = 850f;
        public const float ArcherArrowKnockback = 100f;
        public static TimeSpan ArcherArrowKnockbackLength => TimeSpan.FromMilliseconds(300);
        public static TimeSpan ArcherArrowCooldown => TimeSpan.FromMilliseconds(1250);

        public static EnemyProperties GetDemonProperties()
        {
            return new EnemyProperties
            {
                MinHealth = GetHealth(1.65f),
                HealthRegen = BaseRegen * 1.5f,
                MinSpeed = BaseSpeed * 0.8f,
                MaxHealth = GetHealth(2f),
                MaxSpeed = BaseSpeed * 0.9f,
                CritChance = 0.12f
            };
        }
        public const float DemonMissileKnockback = 350f;
        public const float DemonMissileSpeed = 550f;
        public const float DemonMissileDistance = 750f;
        public const float DemonMissileBurnDamagePercentage = 0.2f;
        public static TimeSpan DemonMissileBurnLength => TimeSpan.FromMilliseconds(1500);
        public static TimeSpan DemonMissileKnockbackLength => TimeSpan.FromMilliseconds(315);
        public static TimeSpan DemonMissileCooldown => TimeSpan.FromMilliseconds(2000);

        public static EnemyProperties GetFrostDemonProperties()
        {
            return new EnemyProperties
            {
                MinHealth = GetHealth(1.65f),
                HealthRegen = BaseRegen * 1.5f,
                MinSpeed = BaseSpeed * 0.8f,
                MaxHealth = GetHealth(2f),
                MaxSpeed = BaseSpeed * 0.9f,
                CritChance = 0.12f
            };
        }
        public const float FrostDemonMissileSpeed = 550f;
        public const float FrostDemonMissileLength = 700f;
        public const float FrostDemonSlowAmount = -100f;
        public static TimeSpan FrostDemonColdLength => TimeSpan.FromMilliseconds(2200);
        public static TimeSpan FrostDemonMissileCooldown => TimeSpan.FromMilliseconds(2000);
    }
}
