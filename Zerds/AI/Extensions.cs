using System;
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
            var enemies = being.Enemies().Where(e => e.IsAlive).ToList();
            if (!enemies.Any()) return null;
            var distance = enemies.Select(z => z.DistanceBetween(being)).Min();
            return enemies.First(z => Math.Abs(z.DistanceBetween(being) - distance) < CodingConstants.Tolerance);
        }

        public static void Face(this Being being, Being target)
        {
            being.Facing = new Vector2(target.Position.X - being.Position.X, -1 * (target.Position.Y - being.Position.Y)).Normalized();
        }

        public static void Face(this Being being, Point target)
        {
            being.Facing = new Vector2(target.X - being.Position.X, -1 * (target.Y - being.Position.Y)).Normalized();
        }
    }
}
