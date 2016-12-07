using System;

namespace Zerds.Constants
{
    public static class AbilityConstants
    {
        public const float DashCooldown = 10f;
        public const float DashBonus = 600f;
        public static TimeSpan DashLength => new TimeSpan(0, 0, 0, 0, 300);

        public const float SprintCooldown = 10f;
        public const float SprintBonus = 120f;
        public static TimeSpan SprintLength => new TimeSpan(0, 0, 2);

        public static float ColdSpeedFactor = 0.6f;

        public const float WandKnockback = 200f;
        public const float WandSpeed = 600f;
        public const float WandDistance = 400f;
        public static TimeSpan WandCastTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan WandFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan WandCooldown = TimeSpan.FromMilliseconds(425);

        public const float FrostOrbKnockback = 0f;
        public const float FrostOrbSpeed = 720f;
        public const float FrostOrbDistance = 650f;
        public const float FrostOrbManaCost = 50f;
        public static float FrostOrbColdLength = 1500;
        public static TimeSpan FrostOrbCastTime => TimeSpan.FromMilliseconds(500);
        public static TimeSpan FrostOrbFollowThroughTime => TimeSpan.FromMilliseconds(200);
        public static TimeSpan FrostOrbCooldown = TimeSpan.FromMilliseconds(2425);
    }
}
