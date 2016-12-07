using Microsoft.Xna.Framework;
using System;
using Zerds.Entities;

namespace Zerds
{
    public static class Extensions
    {
        private const float DegToRad = (float)Math.PI / 180f;

        /// <summary>
        /// Returns the absolute value of the angle between two vectors in degrees
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float AngleBetween(this Vector2 a, Vector2 b)
        {
            var v1 = new Vector2(a.X, a.Y);
            var v2 = new Vector2(b.X, b.Y);
            a.Normalize();
            b.Normalize();
            return Math.Abs((float)Math.Acos(Vector2.Dot(a, b)) * 180f / (float)Math.PI);
        }

        public static float DistanceBetween(this Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public static float DistanceBetween(this Point a, Point b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public static float DistanceBetween(this Being a, Being b)
        {
            return DistanceBetween(a.Position, b.Position);
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            return v.RotateRadians(degrees * DegToRad);
        }

        public static Rectangle RotateAround(this Rectangle rect, Point p, float radians)
        {
            var center = new Vector2(rect.Center.X - p.X, rect.Center.Y - p.Y);
            var newCenter = center.RotateRadians(radians);
            rect.Offset(newCenter.X - center.X, newCenter.Y - center.Y);
            return rect;
        }

        public static Vector2 RotateRadians(this Vector2 v, float radians)
        {
            var ca = (float)Math.Cos(radians);
            var sa = (float)Math.Sin(radians);
            return new Vector2(ca * v.X - sa * v.Y, sa * v.X + ca * v.Y);
        }
        
        public static Vector2 Normalized(this Vector2 v)
        {
            v.Normalize();
            return v;
        }
    }
}
