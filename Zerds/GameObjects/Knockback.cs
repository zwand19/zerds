using Microsoft.Xna.Framework;
using System;

namespace Zerds.GameObjects
{
    public class Knockback
    {
        public Vector2 Direction { get; private set; }
        public TimeSpan MaxDuration { get; private set; }
        public TimeSpan Duration { get; set; }
        public float Speed { get; set; }

        public Knockback(Vector2 direction, TimeSpan duration, float speed)
        {
            Direction = direction;
            Duration = duration;
            MaxDuration = new TimeSpan(0, 0, 0, 0, (int)duration.TotalMilliseconds);
            Speed = speed;
        }
    }
}
