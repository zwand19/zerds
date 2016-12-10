using System;

namespace Zerds.Constants
{
    public static class AbilityConstants
    {
        public const float DashBonus = 600f;
        public static TimeSpan DashCooldown = TimeSpan.FromSeconds(10);
        public static TimeSpan DashLength => new TimeSpan(0, 0, 0, 0, 300);
        
        public const float SprintBonus = 120f;
        public static TimeSpan SprintCooldown = TimeSpan.FromSeconds(10);
        public static TimeSpan SprintLength => new TimeSpan(0, 0, 2);

        public static float ColdSpeedFactor = 0.6f;

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
        public static float IceballColdLength = 1500;
        public static TimeSpan IceballKnockbackLength => TimeSpan.Zero;
        public static TimeSpan IceballCastTime => TimeSpan.FromMilliseconds(450);
        public static TimeSpan IceballFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan IceballCooldown = TimeSpan.FromMilliseconds(2325);

        public const float FireballKnockback = 350f;
        public const float FireballSpeed = 550f;
        public const float FireballDistance = 750f;
        public const float FireballManaCost = 50f;
        public const float FireballBurnLength = 1500f;
        public const float FireballBurnDamagePercentage = 0.15f;
        public static TimeSpan FireballKnockbackLength => TimeSpan.FromMilliseconds(315);
        public static TimeSpan FireballCastTime => TimeSpan.FromMilliseconds(550);
        public static TimeSpan FireballFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan FireballCooldown = TimeSpan.FromMilliseconds(2500);
    }
}
