using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Zerds.Entities;

namespace Zerds.AI
{
    public static class Extensions
    {
        public static Zerd GetNearestZerd(this Being being)
        {
            var distance = Globals.GameState.Zerds.Select(z => z.DistanceBetween(being)).Min();
            return Globals.GameState.Zerds.First(z => z.DistanceBetween(being) == distance);
        }

        public static void Face(this Being being, Being target)
        {
            being.Facing = new Vector2(target.Position.X - being.Position.X, -1 * (target.Position.Y - being.Position.Y)).Normalized();
        }
    }
}
