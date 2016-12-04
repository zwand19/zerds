using System;

namespace Zerds.Constants
{
    public static class GameplayConstants
    {
        public const float BackpedalFactor = 0.35f;
        public const float SideStepFactor = 0.65f;
        public const float MaxZerdSpeed = 200f;
        public const float ZerdFrontFacingAngle = 15f;
        public const double KnockbackDecay = 0.35;
        public static TimeSpan WandCooldown = TimeSpan.FromMilliseconds(1250);
    }
}
