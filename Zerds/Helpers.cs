using System;
using Microsoft.Xna.Framework;

namespace Zerds
{
    public static class Helpers
    {
        private static readonly Random Random = new Random();

        public static bool RandomChance(float chance)
        {
            return Random.NextDouble() < chance;
        }

        public static Rectangle CreateRect(double x, double y, double width, double height)
        {
            return new Rectangle((int) x, (int) y, (int) width, (int) height);
        }

        public static float RandomInRange(float min, float max)
        {
            return (float)Random.NextDouble() * (max - min) + min;
        }
    }
}
