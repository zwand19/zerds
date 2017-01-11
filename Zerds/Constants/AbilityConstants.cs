using System;

namespace Zerds.Constants
{
    public static class AbilityConstants
    {
        public const float DashBonus = 600f;
        public static TimeSpan DashCooldown = TimeSpan.FromSeconds(10);
        public static TimeSpan DashLength => new TimeSpan(0, 0, 0, 0, 300);
        
        public const float SprintBonus = 175f;
        public static TimeSpan SprintCooldown = TimeSpan.FromSeconds(10);
        public static TimeSpan SprintLength => new TimeSpan(0, 0, 2);
        public const float SprintManaPerSecond = 15f;

        public static float ColdSpeedFactor = -150f;

        public const float WandKnockback = 200f;
        public const float WandSpeed = 600f;
        public const float WandDistance = 400f;
        public static TimeSpan WandKnockbackLength => TimeSpan.FromMilliseconds(250);
        public static TimeSpan WandCastTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan WandFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan WandCooldown => TimeSpan.FromMilliseconds(425);

        public const float IceballKnockback = 0f;
        public const float IceballSpeed = 720f;
        public const float IceballDistance = 650f;
        public const float IceballManaCost = 50f;
        public const float IceballExplosionDistance = 100f;
        public static TimeSpan IceballColdLength => TimeSpan.FromMilliseconds(5000);
        public static TimeSpan IceballKnockbackLength => TimeSpan.Zero;
        public static TimeSpan IceballCastTime => TimeSpan.FromMilliseconds(450);
        public static TimeSpan IceballFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan IceballCooldown = TimeSpan.FromMilliseconds(3050);

        public const float FireballKnockback = 350f;
        public const float FireballSpeed = 550f;
        public const float FireballDistance = 750f;
        public const float FireballManaCost = 50f;
        public const float FireballBurnLength = 1500f;
        public const float FireballBurnDamagePercentage = 0.15f;
        public static TimeSpan FireballKnockbackLength => TimeSpan.FromMilliseconds(315);
        public static TimeSpan FireballCastTime => TimeSpan.FromMilliseconds(550);
        public static TimeSpan FireballFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan FireballCooldown = TimeSpan.FromMilliseconds(3250);

        public const float LavaBlastKnockback = 420f;
        public const float LavaBlastSpeed = 325f;
        public const float LavaBlastDistance = 450f;
        public const float LavaBlastManaCost = 80f;
        public const float LavaBlastBurnLength = 3000f;
        public const float LavaBlastBurnDamagePercentage = 0.1f;
        public static TimeSpan LavaBlastKnockbackLength => TimeSpan.FromMilliseconds(400);
        public static TimeSpan LavaBlastCastTime => TimeSpan.FromMilliseconds(1250);
        public static TimeSpan LavaBlastFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan LavaBlastCooldown = TimeSpan.FromMilliseconds(10000);

        public const float DemonMissileKnockback = 350f;
        public const float DemonMissileSpeed = 550f;
        public const float DemonMissileDistance = 750f;
        public const float DemonMissileBurnDamagePercentage = 0.2f;
        public static TimeSpan DemonMissileBurnLength => TimeSpan.FromMilliseconds(1500);
        public static TimeSpan DemonMissileKnockbackLength => TimeSpan.FromMilliseconds(315);
        public static TimeSpan DemonMissileCooldown => TimeSpan.FromMilliseconds(2000);
        
        public const float FrostDemonMissileSpeed = 550f;
        public const float FrostDemonMissileLength = 700f;
        public const float FrostDemonSlowAmount = -100f;
        public static TimeSpan FrostDemonColdLength => TimeSpan.FromMilliseconds(2200);
        public static TimeSpan FrostDemonMissileCooldown => TimeSpan.FromMilliseconds(2000);

        public const float ArcherArrowSpeed = 675f;
        public const float ArcherArrowLength = 850f;
        public const float ArcherArrowKnockback = 100f;
        public static TimeSpan ArcherArrowKnockbackLength => TimeSpan.FromMilliseconds(300);
        public static TimeSpan ArcherArrowCooldown => TimeSpan.FromMilliseconds(750);
    }
}
