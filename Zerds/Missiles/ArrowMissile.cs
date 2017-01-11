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
    public class ArrowMissile : Missile
    {
        public ArrowMissile(Being being, DamageInstance damageInstance, Point p) : base("Missiles/arrow.png")
        {
            Damage = damageInstance;
            Width = 22;
            Height = 22;
            X = p.X;
            Y = p.Y;
            Creator = being;
            Origin = p;
            Distance = AbilityConstants.ArcherArrowLength;
            Speed = AbilityConstants.ArcherArrowSpeed;
            Velocity = Creator.Facing.Normalized().Rotate(Helpers.RandomInRange(-3, 3));

            Animations = new AnimationList();
            var moveAnimation = new Animation(AnimationTypes.Move);
            moveAnimation.AddFrame(new Rectangle(0, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            Animations.Add(moveAnimation);
        }

        public override Animation GetCurrentAnimation()
        {
            return Animations.Get(AnimationTypes.Move);
        }

        public override float SpriteRotation()
        {
            return (float)Math.PI / 2;
        }

        public override List<Rectangle> Hitbox()
        {
            return new List<Rectangle> {
                new Rectangle((int)(X - Width * 0.38f), (int)(Y - Width * 0.38f), (int)(Width * 0.76f), (int)(Width * 0.76f))
            };
        }

        public override void OnHit(Being target)
        {
            Damage.DamageBeing(target);
            IsActive = false;
        }
    }
}
