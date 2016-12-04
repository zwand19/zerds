using System;

namespace Zerds.Constants
{
    public static class AbilityConstants
    {
        public const float DashCooldown = 10f;
        public const float DashBonus = 400f;
        public static TimeSpan DashLength => new TimeSpan(0, 0, 0, 0, 300);

        public const float SprintCooldown = 10f;
        public const float SprintBonus = 120f;
        public static TimeSpan SprintLength => new TimeSpan(0, 0, 2);
        
        public const float WandKnockback = 200f;
        public const float WandSpeed = 600f;
        public static TimeSpan WandCastTime => TimeSpan.FromMilliseconds(150);
        public static TimeSpan WandFollowThroughTime => TimeSpan.FromMilliseconds(150);
    }
}
