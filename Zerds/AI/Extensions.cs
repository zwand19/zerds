using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using Zerds.Constants;
using Zerds.Entities;

namespace Zerds.AI
{
    public static class Extensions
    {
        public static Being GetNearestEnemy(this Being being)
        {
            if (!being.Enemies().Any()) return null;
            var distance = being.Enemies().Select(z => z.DistanceBetween(being)).Min();
            return being.Enemies().First(z => Math.Abs(z.DistanceBetween(being) - distance) < CodingConstants.Tolerance);
        }

        public static void Face(this Being being, Being target)
        {
            being.Facing = new Vector2(target.Position.X - being.Position.X, -1 * (target.Position.Y - being.Position.Y)).Normalized();
        }
    }
}
