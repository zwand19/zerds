using System;
using Microsoft.Xna.Framework;
using Zerds.GameObjects;
using Zerds.Entities;
using Zerds.Graphics;
using Zerds.Enums;
using Zerds.Constants;
using System.Collections.Generic;
using Zerds.Factories;
using Zerds.Buffs;

namespace Zerds.Missiles
{
    public class IcicleMissile : Missile
    {
        public IcicleMissile(Zerd zerd, DamageInstance damageInstance, Point p, int index) : base("Missiles/icicle.png")
        {
            Damage = damageInstance;
            Width = 40;
            Height = 40;
            X = p.X;
            Y = p.Y;
            Creator = zerd;
            Origin = p;
            Distance = AbilityConstants.IcicleDistance;
            Speed = AbilityConstants.IcicleSpeed;
            Velocity = Creator.Facing.Normalized().Rotate(360f * index / 8);
            // Move the icicle away from the caster a bit
            X += Velocity.X * 45;
            Y -= Velocity.Y * 45;

            Animations = new AnimationList();
            var moveAnimation = new Animation(AnimationTypes.Move);
            moveAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 1, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 2, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 3, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 4, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 5, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 6, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 7, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            Animations.Add(moveAnimation);
        }

        public override Animation GetCurrentAnimation()
        {
            return Animations.Get(AnimationTypes.Move);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsAlive) IsActive = false;
            base.Update(gameTime);
        }

        public override List<Rectangle> Hitbox()
        {
            return new List<Rectangle> {
                new Rectangle((int)(X - Width * 0.4f), (int)(Y - Width * 0.4f), (int)(Width * 0.8f), (int)(Width * 0.8f)),
            };
        }

        public override void OnHit(Being target)
        {
            if (target.HealthPercentage < PlayerSkills.IcicleKillPercent)
                target.Health = 0;
            Damage.DamageBeing(target);
            IsActive = false;
            target.AddBuff(new FrozenBuff(target, AbilityConstants.IcicleColdLength));
        }
    }
}
