using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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

        public static void DrawLeftAlign(this string text, Vector2 position, float fontSize, Color? color = null, FontTypes type = FontTypes.Pericles)
        {
            var font = Globals.Fonts[type];
            var size = font.MeasureString(text);
            var scale = fontSize / 50f;
            var y = (position - scale * size / 2).Y;
            var fontColor = color ?? Color.Black;
            Globals.SpriteDrawer.DrawString(font, text, new Vector2(position.X, y), fontColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public static void DrawRightAlign(this string text, Vector2 position, float fontSize, Color? color = null, FontTypes type = FontTypes.Pericles)
        {
            var font = Globals.Fonts[type];
            var size = font.MeasureString(text);
            var scale = fontSize / 50f;
            var x = (position - scale * size).X;
            text.DrawLeftAlign(new Vector2(x, position.Y), fontSize, color, type);
        }

        public static void Draw(this string text, Vector2 position, float fontSize, Color? color = null, FontTypes type = FontTypes.Pericles)
        {
            var font = Globals.Fonts[type];
            var size = font.MeasureString(text);
            var scale = fontSize / 50f;
            var x = (position - scale * size / 2).X;
            text.DrawLeftAlign(new Vector2(x, position.Y), fontSize, color, type);
        }

        public static void DrawWrapped(this string text, Vector2 position, float fontSize, float maxWidth, Color? color = null, FontTypes type = FontTypes.Pericles)
        {
            var font = Globals.Fonts[type];
            var size = font.MeasureString(text);
            var scale = fontSize / 50f;
            var x = (position - scale * size / 2).X;
            text.Wrap(maxWidth, fontSize).DrawLeftAlign(new Vector2(x, position.Y), fontSize, color, type);
        }

        public static void Draw(this Rectangle rect, Color? color = null)
        {
            var colorVal = color ?? Color.Black;
            Globals.SpriteDrawer.Draw(Globals.WhiteTexture, rect, colorVal);
        }

        public static Rectangle BorderRect(this Rectangle rect, int borderSize)
        {
            return new Rectangle(rect.Left - borderSize, rect.Top - borderSize, rect.Width + borderSize * 2, rect.Height + borderSize * 2);
        }

        public static TimeSpan SubtractWithGameSpeed(this TimeSpan t1, TimeSpan t2)
        {
            return t1.Subtract(TimeSpan.FromMilliseconds(t2.TotalMilliseconds * Globals.GameState.GameSpeed));
        }

        public static TimeSpan AddWithGameSpeed(this TimeSpan t1, TimeSpan t2)
        {
            return t1.Add(TimeSpan.FromMilliseconds(t2.TotalMilliseconds * Globals.GameState.GameSpeed));
        }

        public static T RandomElement<T>(this List<T> elements)
        {
            if (!elements.Any())
                return default(T);

            var index = Globals.Random.Next(elements.Count);
            return elements[index];
        }

        public static List<Being> Enemies(this Being b)
        {
            var enemies = Globals.GameState.Enemies.Select(e => e as Being).ToList();
            return b is Zerd || ((Enemy) b).Charmed ? enemies : Globals.GameState.Friendlies;
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

        /// <summary>
        /// Draw some text (or don't) relative to the camera
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="fontSize"></param>
        /// <param name="color"></param>
        /// <param name="type"></param>
        public static void DrawGameText(this string text, Vector2 position, float fontSize, Color? color = null, FontTypes type = FontTypes.Pericles)
        {
            // Only draw if on screen
            if (position.X > Globals.Camera.LeftDrawBound && position.X < Globals.Camera.RightDrawBound && position.Y > Globals.Camera.TopDrawBound && position.Y < Globals.Camera.BottomDrawBound)
            {
                var font = Globals.Fonts[type];
                var size = font.MeasureString(text);
                var scale = fontSize / 50f;
                var scaledX = (position - scale * size / 2).X;
                var x = scaledX - Globals.Camera.ScreenLeft;
                var y = position.Y - Globals.Camera.ScreenTop;
                text.DrawLeftAlign(new Vector2(x, y), fontSize, color, type);
            }
        }

        /// <summary>
        /// Draw a game object (or don't) relative to the camera
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        /// <param name="destinationRectangle"></param>
        /// <param name="scale"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        public static void DrawGameObject(this GameObject obj, Rectangle sourceRectangle, Color color, Rectangle? destinationRectangle = null, Vector2? scale = null, float rotation = 0, Vector2? origin = null)
        {
            // Only draw if on screen
            if (obj.X > Globals.Camera.LeftDrawBound && obj.X < Globals.Camera.RightDrawBound && obj.Y > Globals.Camera.TopDrawBound && obj.Y < Globals.Camera.BottomDrawBound)
            {
                if (destinationRectangle.HasValue)
                {
                    Globals.SpriteDrawer.Draw(
                      texture: obj.Texture,
                      sourceRectangle: sourceRectangle,
                      destinationRectangle: new Rectangle((int)(obj.X - Globals.Camera.ScreenLeft), (int)(obj.Y - Globals.Camera.ScreenTop), destinationRectangle.Value.Width, destinationRectangle.Value.Height),
                      color: color,
                      origin: origin,
                      scale: scale,
                      rotation: rotation);
                }
                else
                {
                    Globals.SpriteDrawer.Draw(
                      texture: obj.Texture,
                      sourceRectangle: sourceRectangle,
                      position: new Vector2(obj.X - Globals.Camera.ScreenLeft, obj.Y - Globals.Camera.ScreenTop),
                      color: color,
                      origin: origin,
                      scale: scale,
                      rotation: rotation);
                }
            }
        }

        public static void Draw(this Texture2D texture, Rectangle? sourceRectangle = null, Color? color = null, Vector2? position = null, Rectangle? destinationRectangle = null, Vector2? scale = null, float rotation = 0, Vector2? origin = null)
        {
            var x = destinationRectangle?.X ?? position.Value.X;
            var y = destinationRectangle?.Y ?? position.Value.Y;
            // Only draw if on screen
            if (x > Globals.Camera.LeftDrawBound && x < Globals.Camera.RightDrawBound && y > Globals.Camera.TopDrawBound && y < Globals.Camera.BottomDrawBound)
            {
                if (position.HasValue)
                {
                    Globals.SpriteDrawer.Draw(
                        texture: texture,
                          sourceRectangle: sourceRectangle,
                          position: new Vector2(x - Globals.Camera.ScreenLeft, y - Globals.Camera.ScreenTop),
                          color: color,
                          origin: origin,
                          scale: scale,
                          rotation: rotation);
                } else
                {
                    Globals.SpriteDrawer.Draw(
                        texture: texture,
                          sourceRectangle: sourceRectangle,
                          destinationRectangle: new Rectangle((int)(x - Globals.Camera.ScreenLeft), (int)(y - Globals.Camera.ScreenTop), destinationRectangle.Value.Width, destinationRectangle.Value.Height),
                          color: color,
                          origin: origin,
                          scale: scale,
                          rotation: rotation);
                }
            }
        }

        public static void Draw(this Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            // Only draw if on screen
            if (destinationRectangle.Right > Globals.Camera.LeftDrawBound && destinationRectangle.Left < Globals.Camera.RightDrawBound && destinationRectangle.Bottom > Globals.Camera.TopDrawBound && destinationRectangle.Top < Globals.Camera.BottomDrawBound)
            {
                Globals.SpriteDrawer.Draw(
                    texture: texture,
                    destinationRectangle: new Rectangle((int)(destinationRectangle.X - Globals.Camera.ScreenLeft), (int)(destinationRectangle.Y - Globals.Camera.ScreenTop), destinationRectangle.Width, destinationRectangle.Height),
                    color: color);
            }
        }
    }
}
