using System;
using System.Collections.Generic;
using Zerds.Items;

namespace Zerds.Constants
{
    public static class GameplayConstants
    {
        public static TimeSpan LevelLength = TimeSpan.FromSeconds(30);
        public static float HealthFactorPerLevel = 0.3f;
        public static float DamageFactorPerLevel = 0.12f;
        public static int StartingGold = 100;
        public static int BaseLevelGold = 135;
        public static int BonusGoldPerLevel = 15;
        public static int BaseGoldPerEnemy = 6;
        public static int BonusGoldPerEnemyPerLevel = 4;
        public static float GoldPerCombo = 2f;
        public static int AbilityUpgradeCost = 150;
        public static int FloatingSkillPointCost = 250;
        public const float WanderSpeedFactor = 0.25f;
        public static TimeSpan WaitTimeAfterLosing => TimeSpan.FromSeconds(3.75);
        public static TimeSpan DefaultEnemyWanderInterval => TimeSpan.FromSeconds(2);

        public const int FloatingPointsPerLevel = 30;
        public const int SkillPointsPerLevel = 3;
        public const float AggroRangeBuffer = 15;

        public const float BackpedalFactor = 0.35f;
        public const float SideStepFactor = 0.65f;
        public const float MaxZerdSpeed = 200f;
        public const float ZerdFrontFacingAngle = 15f;
        public const double KnockbackDecay = 0.35;

        public const float MinSpeed = 50f;
        public const float MaxSpeed = 800f;

        public const float CriticalDamageBonus = 2f;
        public const float CriticalDamageKnockbackDurationBonus = 1.25f;
        public const float CriticalDamageKnockbackSpeedBonus = 1.5f;
        public const float DamageVariance = 0.3f;

        public const float ZerdStartingHealth = 100f;
        public const float ZerdStartingMana = 200f;
        public const float ZerdStartingHealthRegen = 1.5f;
        public const float ZerdStartingManaRegen = 9f;
        public const float ZerdCritChance = 0.1f;

        public static Dictionary<ItemRarities, float> ItemRarityChances => new Dictionary<ItemRarities, float>
        {
            {ItemRarities.Novice, 0.4f},
            {ItemRarities.Apprentice, 0.68f },
            {ItemRarities.Adept, 0.85f },
            {ItemRarities.Master, 0.95f },
            {ItemRarities.Legendary, 1f }
        };

        public const float SecondItemChanceFactor = 0.5f;
        public const float ThirdItemChanceFactor = 0.25f;
        public const float BaseTreasureDropChance = 0.5f;//0.0015f;
        public const float TreasureDropChancePerLevel = 0.8f;//0.001f;
        public const float KeyDropChance = 0.76f;//0.0015f;
        public const float PotionDropChance = 0.25f;
        public const float ItemDropBaseEnemyWorth = 10f; // scale chances of dropping potions/items based on this enemy worth
        public const float PickupItemSpeed = 80f;
        public const float PickupItemSpeedDecay = 60f;
        public const float DefaultItemSize = 26f;
        public static TimeSpan PickupItemLength = TimeSpan.FromSeconds(5);
        public const float HealthPotionBonus = 0.25f;
        public const float ManaPotionBonus = 0.2f;
    }
}
