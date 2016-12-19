using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Entities;
using Zerds.Enums;

namespace Zerds
{
    public static class Extensions
    {
        private const float DegToRad = (float)Math.PI / 180f;
        
        public static float AngleBetween(this Vector2 a, Vector2 b)
        {
            var v1 = new Vector2(a.X, a.Y).Normalized();
            var v2 = new Vector2(b.X, b.Y).Normalized();
            return Math.Abs((float)Math.Acos(Vector2.Dot(v1, v2)) * 180f / (float)Math.PI);
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

        public static Vector2 Move(this Vector2 v, float x = 0f, float y = 0f)
        {
            return new Vector2(v.X + x, v.Y + y);
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

        public static Rectangle BasicHitbox(this Entity r)
        {
            return new Rectangle((int) (r.X - r.Width*r.HitboxSize/2), (int) (r.Y - r.Width*r.HitboxSize/2), (int) (r.Width*r.HitboxSize),
                (int) (r.Width*r.HitboxSize));
        }

        public static Rectangle Scale(this Rectangle r, float scale)
        {
            return Helpers.CreateRect(r.X + r.Width * (1 - scale) / 2, r.Y + r.Height * (1 - scale) / 2, r.Width * scale, r.Height * scale);
        }

        public static bool Intersects(this Entity r1, Entity r2)
        {
            return r1.Hitbox().Any(hitbox => r2.Hitbox().Any(hitbox.Intersects));
        }

        public static void DrawTextLeftAlign(this SpriteBatch spriteBatch, string text, Vector2 position, float fontSize,
            FontTypes type = FontTypes.Pericles, Color? color = null)
        {
            var font = Globals.Fonts[type];
            var scale = fontSize / 50f;
            var fontColor = color ?? Color.Black;
            Globals.SpriteDrawer.DrawString(font, text, position, fontColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public static void DrawText(this SpriteBatch spriteBatch, string text, Vector2 position, float fontSize, Color? color = null, FontTypes type = FontTypes.Pericles)
        {
            var font = Globals.Fonts[type];
            var size = font.MeasureString(text);
            spriteBatch.DrawTextLeftAlign(text, position - fontSize * size / 100f, fontSize, type, color);
        }

        public static void DrawRect(this SpriteBatch spriteBatch, Rectangle rect, Color? color = null)
        {
            var colorVal = color ?? Color.Black;
            spriteBatch.Draw(Globals.WhiteTexture, rect, colorVal);

        }
    }
}
