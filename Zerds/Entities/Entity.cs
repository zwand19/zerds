using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.Graphics;
using Zerds.Items;

namespace Zerds.Entities
{
    public abstract class Entity
    {
        public float Speed { get; set; }
        public float HitboxSize = 1f;
        public bool IsActive { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Facing { get; set; }
        public AnimationList Animations { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Texture2D Texture { get; set; }
        public abstract void Draw();
        public abstract List<Rectangle> Hitbox();
        public Point Position => new Point((int)X, (int)Y);
        public Vector2 PositionVector => new Vector2(X, Y);

        protected Entity(string file, bool cache)
        {
            Texture = cache ? TextureCacheFactory.Get(file) : TextureCacheFactory.GetOnce(file);
            IsActive = true;
        }

        public virtual Animation GetCurrentAnimation()
        {
            return Animations.Get(AnimationTypes.Stand);
        }

        public virtual float SpriteRotation()
        {
            return 0;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Speed < 0) Speed = 0;
            X += Velocity.X * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;
            Y -= Velocity.Y * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;

            GetCurrentAnimation().Update(gameTime);
        }
    }
}
