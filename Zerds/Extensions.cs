using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Constants;
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

        public static TimeSpan SubtractWithGameSpeed(this TimeSpan t1, TimeSpan t2)
        {
            return t1.Subtract(TimeSpan.FromMilliseconds(t2.TotalMilliseconds * Globals.GameState.GameSpeed));
        }

        public static TimeSpan AddWithGameSpeed(this TimeSpan t1, TimeSpan t2)
        {
            return t1.Add(TimeSpan.FromMilliseconds(t2.TotalMilliseconds * Globals.GameState.GameSpeed));
        }

        public static void DrawLine(this SpriteBatch sb, Vector2 start, Vector2 end, int width = 1, Color? color = null)
        {
            var colorVal = color ?? Color.Black; 
            var edge = end - start;
            // calculate angle to rotate line
            var angle = (float)Math.Atan2(edge.Y, edge.X);
            
            sb.Draw(Globals.WhiteTexture,
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    width), //width of line, change this to make thicker line
                null,
                colorVal, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }

        public static string Wrap(this string text, float maxLineWidth, float fontSize, FontTypes fontType = FontTypes.Pericles)
        {
            var font = Globals.Fonts[fontType];
            var words = text.Split(' ');
            var sb = new StringBuilder();
            var lineWidth = 0f;
            var spaceWidth = font.MeasureString(" ").X;

            foreach (var word in words)
            {
                var size = font.MeasureString(word) * fontSize / CodingConstants.FontSize;

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }
    }
}
