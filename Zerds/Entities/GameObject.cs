using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zerds.Entities
{
    public abstract class GameObject
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Texture2D Texture { get; set; }
        public abstract void Draw();
        public Point Position => new Point((int)X, (int)Y);
        public Vector2 PositionVector => new Vector2(X, Y);
    }
}
