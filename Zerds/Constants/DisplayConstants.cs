using System;

namespace Zerds.Constants
{
    public static class DisplayConstants
    {
        public const int MinHealthBarHeight = 6;
        public const int MaxHealthBarHeight = 12;
        public const int HealthBarBorder = 2;
        public const float DamageTextSpeed = 150f;
        public const int TileWidth = 64;
        public const int TileHeight = 64;
        public const int CameraBuffer = 150; // draw things within screen bounds + this buffer
        public static TimeSpan DamageTextDuration => TimeSpan.FromMilliseconds(500);
        public static TimeSpan ZerdDeathTime => TimeSpan.FromSeconds(2.5);
    }
}
