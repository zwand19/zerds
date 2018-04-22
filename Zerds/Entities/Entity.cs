using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.Graphics;

namespace Zerds.Entities
{
    public abstract class Entity : GameObject
    {
        public float Speed { get; set; }
        public float HitboxSize = 1f;
        public bool IsActive { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Facing { get; set; }
        public AnimationList Animations { get; set; }
        public abstract List<Rectangle> Hitbox();

        protected Entity(string file, bool cache)
        {
            Texture = string.IsNullOrWhiteSpace(file) ? null : cache ? TextureCacheFactory.Get(file) : TextureCacheFactory.GetOnce(file);
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
