﻿using System;
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
            IsActive = true;
            IsAlive = true;
            Opacity = 1;
        }

        public override Animation GetCurrentAnimation()
        {
            return Animations.Get(AnimationTypes.Move);
        }

        public override void Update(GameTime gameTime)
        {
            if (Creator is Zerd && IsAlive)
            {
                foreach (var enemy in Globals.GameState.Enemies.Where(e => e.IsAlive))
                {
                    if (enemy.Hitbox().Any(hitbox => Hitbox().Any(hitbox.Intersects)))
                    {
                        OnHit(enemy);
                        return;
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {
            var rect = GetCurrentAnimation().CurrentRectangle;
            var angle = -(float)Math.Atan2(Velocity.Y, Velocity.X) + SpriteRotation();
            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: rect,
                color: Color.White * Opacity,
                position: new Vector2(X, Y),
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
