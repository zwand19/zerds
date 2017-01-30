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

        protected Missile(string file) : base(file, true)
        {
            IsAlive = true;
            Opacity = 1;
        }

        public override Animation GetCurrentAnimation()
        {
            return Animations.Get(AnimationTypes.Move);
        }

        public override void Update(GameTime gameTime)
        {
            CheckHit();
            if (Origin.DistanceBetween(Position) > Distance && IsAlive)
            {
                Speed *= 0.75f;
                IsAlive = false;
                if (Creator is Zerd)
                    ((Zerd) Creator).Combo = 0;
            }
            base.Update(gameTime);
        }

        private void CheckHit()
        {
            var creator = Creator as Zerd;
            if (creator != null && IsAlive)
            {
                foreach (var enemy in Globals.GameState.Enemies.Where(e => e.IsAlive))
                {
                    if (!enemy.Hitbox().Any(hitbox => Hitbox().Any(hitbox.Intersects))) continue;
                    creator.IncreaseCombo();
                    OnHit(enemy);
                    return;
                }
            }
            else if (IsAlive)
            {
                foreach (var zerd in Globals.GameState.Zerds.Where(e => e.IsAlive))
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
            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: rect,
                color: Color.White * Opacity,
                destinationRectangle: Helpers.CreateRect(X, Y, Width, Height),
                rotation: angle,
                origin: new Vector2(rect.Width / 2f, rect.Height / 2f));
            if (Globals.ShowHitboxes)
            {
                Hitbox().ForEach(r => Globals.SpriteDrawer.Draw(Globals.WhiteTexture, r, Color.Blue));
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
