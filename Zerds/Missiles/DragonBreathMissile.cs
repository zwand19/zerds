using System;
using Microsoft.Xna.Framework;
using Zerds.GameObjects;
using Zerds.Entities;
using Zerds.Graphics;
using Zerds.Enums;
using Zerds.Constants;
using System.Collections.Generic;
using Zerds.Factories;

namespace Zerds.Missiles
{
    public class DragonBreathMissile : Missile
    {
        public DragonBreathMissile(Zerd zerd, DamageInstance damageInstance, Point p) : base("Missiles/dragons_breath.png")
        {
            Damage = damageInstance;
            Width = 26f;
            Height = 26f;
            X = p.X;
            Y = p.Y;
            Creator = zerd;
            Origin = p;
            Distance = AbilityConstants.FireballDistance;
            Speed = AbilityConstants.FireballSpeed;
            Velocity = Creator.Facing.Normalized().Rotate(new Random().Next(11) - 5);

            Animations = new AnimationList();
            var moveAnimation = new Animation(AnimationTypes.Move);
            moveAnimation.AddFrame(new Rectangle(32 * 0, 0, 32, 32), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(32 * 1, 0, 32, 32), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(32 * 2, 0, 32, 32), TimeSpan.FromSeconds(0.1));
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
                new Rectangle((int)(X - Width * 0.5f), (int)(Y - Width * 0.5f), (int)Width, (int)Width)
            };
        }

        public override void OnHit(Being target)
        {
            var zerd = (Zerd)Creator;
            Damage.Damage += Origin.DistanceBetween(Position) * zerd.SkillValue(SkillType.Sniper, true) / 100f;
            Damage.DamageBeing(target);
            IsActive = false;
        }
    }
}
