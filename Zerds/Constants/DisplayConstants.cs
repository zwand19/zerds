using System;

namespace Zerds.Constants
{
    public static class DisplayConstants
    {
        public const int MinHealthBarHeight = 6;
        public const int MaxHealthBarHeight = 12;
        public const int HealthBarBorder = 2;
        public const float DamageTextSpeed = 150f;
        public static TimeSpan DamageTextDuration => TimeSpan.FromMilliseconds(500);
    }
}
