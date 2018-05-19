using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.GameObjects;
using Zerds.Graphics;

namespace Zerds.Missiles
{
    public abstract class Missile : Entity
    {
        public DamageInstance Damage { get; set; }
        public Point Origin { get; set; }
        public float Distance { get; set; }
        public float Opacity { get; set; }
        public bool IsAlive { get; set; }
        public Being Creator { get; set; }
        public abstract void OnHit(Being target);

        protected bool HitWall { get; set; }

        protected Missile(string file) : base(file, true)
        {
            IsAlive = true;
            Opacity = 1;
        }

        // Don't collide with walls as easily as entities
        public Rectangle WallCollisionHitbox => Hitbox().First().Scale(0.65f);

        public override Animation GetCurrentAnimation()
        {
            return Animations.Get(AnimationTypes.Move);
        }

        public override void Update(GameTime gameTime)
        {
            // TBD: is just using one hitbox fine?
            if (Globals.Map.CollidesWithWall(WallCollisionHitbox))
            {
                if (Creator is Zerd)
                    ((Zerd)Creator).Stats.Missed();
                Speed = 0;
                Distance = 0; // Treat this missile as expired (gone past it's max distance)
                IsAlive = false;
                HitWall = true;
            }
            if (IsAlive)
            {
                CheckHit();
                if (Origin.DistanceBetween(Position) > Distance && IsAlive)
                {
                    Speed *= 0.75f;
                    IsAlive = false;
                    if (Creator is Zerd)
                        ((Zerd)Creator).Stats.Missed();
                }
            }
            base.Update(gameTime);
        }

        private void CheckHit()
        {
            if (Creator is Zerd creator)
            {
                foreach (var enemy in Creator.Enemies().Where(e => e.IsAlive))
                {
                    if (!enemy.Hitbox().Any(hitbox => Hitbox().Any(hitbox.Intersects))) continue;
                    creator.Stats.IncreaseCombo();
                    OnHit(enemy);
                    return;
                }
            }
            else
            {
                foreach (var zerd in Globals.GameState.Friendlies.Where(e => e.IsAlive))
                {
                    if (!zerd.Hitbox().Any(hitbox => Hitbox().Any(hitbox.Intersects))) continue;
                    OnHit(zerd);
                    return;
                }
            }
        }

        public override void Draw()
        {
            var rect = GetCurrentAnimation().CurrentRectangle;
            var angle = -(float)Math.Atan2(Velocity.Y, Velocity.X) + SpriteRotation();
            this.DrawGameObject(
                sourceRectangle: rect,
                color: Color.White * Opacity,
                destinationRectangle: Helpers.CreateRect(X, Y, Width, Height),
                rotation: angle,
                origin: new Vector2(rect.Width / 2f, rect.Height / 2f));
            if (Globals.ShowHitboxes)
            {
                Hitbox().ForEach(r => Globals.WhiteTexture.Draw(r, Color.Blue));
            }
        }

        public bool DeathFunc()
        {
            IsActive = false;
            IsAlive = false;
            return true;
        }
    }
}
