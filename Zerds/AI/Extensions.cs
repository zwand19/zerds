﻿using System;
using Microsoft.Xna.Framework;
using System.Linq;
using Zerds.Constants;
using Zerds.Entities;

namespace Zerds.AI
{
    public static class Extensions
    {
        public static Zerd GetNearestZerd(this Being being)
        {
            if (!Globals.GameState.Zerds.Any()) return null;
            var distance = Globals.GameState.Zerds.Select(z => z.DistanceBetween(being)).Min();
            return Globals.GameState.Zerds.First(z => Math.Abs(z.DistanceBetween(being) - distance) < CodingConstants.Tolerance);
        }

        public static void Face(this Being being, Being target)
        {
            being.Facing = new Vector2(target.Position.X - being.Position.X, -1 * (target.Position.Y - being.Position.Y)).Normalized();
        }
    }
}
