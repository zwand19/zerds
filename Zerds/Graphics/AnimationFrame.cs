using System;
using Microsoft.Xna.Framework;

namespace Zerds.Graphics
{
    public class AnimationFrame
    {
        public Rectangle SourceRectangle { get; set; }
        public TimeSpan Duration { get; set; }
        public Func<bool> StartFunc { get; set; }
        public Vector2 Origin { get; set; }
    }
}