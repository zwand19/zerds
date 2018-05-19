using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.Graphics;
using System.Reflection;
using Zerds.Missiles;

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

            var origX = X;
            var origY = Y;
            if (Velocity != Vector2.Zero && Speed > 0)
            {
                X += Velocity.X * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;
                Y -= Velocity.Y * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;

                // Don't run this logic for missiles - let them hit walls and they can handle accordingly
                if (!GetType().GetTypeInfo().IsSubclassOf(typeof(Missile)))
                {
                    if (Globals.Map.CollidesWithWall(this))
                    {
                        var newX = X;
                        var newY = Y;
                        X = origX;
                        // Try just moving the Y
                        if (Globals.Map.CollidesWithWall(this))
                        {
                            X = newX;
                            Y = origY;
                            // Try just moving the X
                            if (Globals.Map.CollidesWithWall(this))
                            {
                                // Can't move either axis
                                X = origX;
                            }
                        }
                    }
                }
            }

            GetCurrentAnimation().Update(gameTime);
        }
    }
}
