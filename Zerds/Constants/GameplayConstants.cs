using System;

namespace Zerds.Constants
{
    public static class GameplayConstants
    {
        public static TimeSpan LevelLength = TimeSpan.FromSeconds(2);
        public static float HealthFactorPerLevel = 0.3f;
        public static float DamageFactorPerLevel = 0.12f;
        public static int StartingGold = 100;

        public const int FloatingPointsPerLevel = 3;
        public const int SkillPointsPerLevel = 3;

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
        public const float ZerdStartingMana = 100f;
        public const float ZerdStartingHealthRegen = 1f;
        public const float ZerdStartingManaRegen = 1.5f;
        public const float ZerdCritChance = 0.1f;

        public const float PotionDropChance = 0.75f;
        public const float PickupItemSpeed = 80f;
        public const float PickupItemSpeedDecay = 60f;
        public const float DefaultItemSize = 26f;
        public static TimeSpan PickupItemLength = TimeSpan.FromSeconds(5);
        public const float HealthPotionBonus = 0.25f;
        public const float ManaPotionBonus = 0.15f;
    }
}
