using System;

namespace Zerds.Constants
{
    public static class AbilityConstants
    {
        public const float DashBonus = 600f;
        public static TimeSpan DashCooldown = TimeSpan.FromSeconds(8);
        public static TimeSpan DashLength => new TimeSpan(0, 0, 0, 0, 300);
        
        public const float SprintBonus = 175f;
        public static TimeSpan SprintLength => new TimeSpan(0, 0, 2);
        public const float SprintManaPerSecond = 15f;

        public static float ColdSpeedFactor = -150f;

        public const float WandKnockback = 200f;
        public const float WandSpeed = 600f;
        public const float WandDistance = 400f;
        public const float WandDamage = 6f;
        public static TimeSpan WandKnockbackLength => TimeSpan.FromMilliseconds(250);
        public static TimeSpan WandCastTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan WandFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan WandCooldown => TimeSpan.FromMilliseconds(425);

        public const float IceballKnockback = 0f;
        public const float IceballSpeed = 720f;
        public const float IceballDistance = 650f;
        public const float IceballManaCost = 50f;
        public const float IceballExplosionDistance = 100f;
        public const float IceballDamage = 10f;
        public static TimeSpan IceballColdLength => TimeSpan.FromMilliseconds(5000);
        public static TimeSpan IceballKnockbackLength => TimeSpan.Zero;
        public static TimeSpan IceballCastTime => TimeSpan.FromMilliseconds(450);
        public static TimeSpan IceballFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan IceballCooldown = TimeSpan.FromMilliseconds(3050);
        
        public const float CharmSpeed = 350f;
        public const float CharmDistance = 700f;
        public const float CharmManaCost = 90f;
        public const float CharmDegeneration = 0.8f;
        public static TimeSpan CharmCastTime => TimeSpan.FromMilliseconds(750);
        public static TimeSpan CharmFollowThroughTime => TimeSpan.FromMilliseconds(400);
        public static TimeSpan CharmCooldown = TimeSpan.FromMilliseconds(30000);
        
        public const float IcicleSpeed = 650f;
        public const float IcicleDistance = 650f;
        public const float IcicleManaCost = 75f;
        public const float IcicleDamage = 15f;
        public static TimeSpan IcicleColdLength => TimeSpan.FromMilliseconds(3000);
        public static TimeSpan IcicleCooldown = TimeSpan.FromMilliseconds(8000);

        public const float FireballKnockback = 350f;
        public const float FireballSpeed = 550f;
        public const float FireballDistance = 750f;
        public const float FireballExplosionDistance = 100f;
        public const float FireballManaCost = 50f;
        public const float FireballBurnDamagePercentage = 0.15f;
        public const float FireballDamage = 10f;
        public static TimeSpan FireballBurnLength = TimeSpan.FromMilliseconds(1500);
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
        public const float LavaBlastDamage = 14f;
        public static TimeSpan LavaBlastKnockbackLength => TimeSpan.FromMilliseconds(400);
        public static TimeSpan LavaBlastCastTime => TimeSpan.FromMilliseconds(1250);
        public static TimeSpan LavaBlastFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan LavaBlastCooldown = TimeSpan.FromMilliseconds(10000);
        
        public const float DragonBreathSpeed = 275f;
        public const float DragonBreathDistance = 250f;
        public const float DragonBreathManaCost = 80f;
        public const float DragonBreathDamage = 2f;
        public static TimeSpan DragonBreathInterval = TimeSpan.FromSeconds(0.08);
        public static TimeSpan DragonBreathDuration = TimeSpan.FromSeconds(2);
        public static TimeSpan DragonBreathCooldown = TimeSpan.FromMilliseconds(10000);

        public const float FrostPoundDamage = 10f;
        public const float FrostPoundManaCost = 75f;
        public const float FrostPoundRange = 260f;
        public static TimeSpan FrostPoundCastTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan FrostPoundFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan FrostPoundFrozenLength = TimeSpan.FromMilliseconds(2250);
        public static TimeSpan FrostPoundCooldown = TimeSpan.FromMilliseconds(12000);
    }
}
