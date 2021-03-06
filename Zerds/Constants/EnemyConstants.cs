﻿using System;
using System.Collections.Generic;
using Zerds.Enums;

namespace Zerds.Constants
{
    public static class EnemyConstants
    {
        private const int BaseHealth = 20;
        private const float BaseRegen = 0.25f;
        private const float BaseSpeed = 150f;
        private static int GetHealth(float factor) => (int)(BaseHealth * factor);

        public static int ZombieWorth = 5;
        public static int DogWorth = 7;
        public static int ArcherWorth = 8;
        public static int DemonWorth = 17;
        public static int FrostDemonWorth = 17;
        public static int SkeletonKingWorth = 75;

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

        public static Dictionary<EnemyTypes, int> EnemyGoldValues => new Dictionary<EnemyTypes, int>
        {
            { EnemyTypes.Archer, 10 },
            { EnemyTypes.Demon, 40 },
            { EnemyTypes.Dog, 6 },
            { EnemyTypes.FrostDemon, 40 },
            { EnemyTypes.SkeletonKing, 400 },
            { EnemyTypes.Zombie, 4 }
        };

        public const int SkeletonKingKnockbackMillis = 500;
        public const float SkeletonKingKnockback = 1250;
        public const float SkeletonKingMinDamage = 35f;
        public const float SkeletonKingMaxDamage = 50f;
        public const float SkeletonKingBlastMissileDamage = 40f;
        public const float SkeletonKingBlastMissileKnockback = 500f;
        public const float SkeletonKingEnrageSpeedFactor = 1.5f;
        public static TimeSpan SkeletonKingBlastCooldown => TimeSpan.FromSeconds(11);
        public static TimeSpan SkeletonKingBlastMissileKnockbackLength => TimeSpan.FromMilliseconds(500);
        public static EnemyProperties GetSkeletonKingProperties()
        {
            return new EnemyProperties
            {
                MinHealth = GetHealth(15.5f),
                HealthRegen = BaseRegen * 3f,
                MinSpeed = BaseSpeed * 0.825f,
                MaxHealth = GetHealth(15.5f),
                MaxSpeed = BaseSpeed * 0.825f,
                CritChance = 0f
            };
        }

        public const float ZombieMinDamage = 8f;
        public const float ZombieMaxDamage = 12f;
        public const float ZombieAggroRange = 325f;
        public const float ZombieAttackRange = 64f;
        public static TimeSpan ZombieWanderLength = TimeSpan.FromSeconds(2);
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

        public const float ArcherAggroRange = 900f;
        public static TimeSpan ArcherWanderLength = TimeSpan.FromSeconds(3);
        public static EnemyProperties GetArcherProperties()
        {
            return new EnemyProperties
            {
                MinHealth = GetHealth(1.1f),
                HealthRegen = BaseRegen * 2,
                MinSpeed = BaseSpeed * 0.6f,
                MaxHealth = GetHealth(1.4f),
                MaxSpeed = BaseSpeed * 0.75f,
                CritChance = 0.2f
            };
        }

        public const float DogAggroRange = float.MaxValue;
        public static TimeSpan DogWanderLength = TimeSpan.FromSeconds(1);
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
        public const float ArcherArrowSpeed = 425f;
        public const float ArcherArrowLength = 850f;
        public const float ArcherArrowKnockback = 100f;
        public static TimeSpan ArcherArrowKnockbackLength => TimeSpan.FromMilliseconds(300);
        public static TimeSpan ArcherArrowCooldown => TimeSpan.FromMilliseconds(2250);

        public const float DemonAggroRange = 1200f;
        public static TimeSpan DemonWanderLength = TimeSpan.FromSeconds(2.5);
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

        public const float FrostDemonAggroRange = 1200f;
        public static TimeSpan FrostDemonWanderLength = TimeSpan.FromSeconds(2.5);
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
