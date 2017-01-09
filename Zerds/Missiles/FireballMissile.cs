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
    public class FireballMissile : Missile
    {
        public FireballMissile(Zerd zerd, DamageInstance damageInstance, Point p) : base("Missiles/fireball.png")
        {
            Damage = damageInstance;
            var size = 64f * (1 + zerd.Player.Skills.ImprovedFireball * SkillConstants.ImprovedFireballStat / 100);
            Width = (int) size;
            Height = (int) size;
            X = p.X;
            Y = p.Y;
            Creator = zerd;
            Origin = p;
            Distance = AbilityConstants.FireballDistance;
            Speed = AbilityConstants.FireballSpeed;
            Velocity = Creator.Facing.Normalized();

            Animations = new AnimationList();
            var moveAnimation = new Animation(AnimationTypes.Move);
            moveAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 1, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 2, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 3, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 4, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 5, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            Animations.Add(moveAnimation);

            var deathAnimation = new Animation(AnimationTypes.Death);
            deathAnimation.AddFrame(new Rectangle(64 * 6, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            deathAnimation.AddFrame(new Rectangle(64 * 7, 0, 64, 64), TimeSpan.FromSeconds(0.1), () => { Opacity = 0.92f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 8, 0, 64, 64), TimeSpan.FromSeconds(0.1), () => { Opacity = 0.84f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 9, 0, 64, 64), TimeSpan.FromSeconds(0.1), () => { Opacity = 0.76f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 10, 0, 64, 64), TimeSpan.FromSeconds(0.1), () => { Opacity = 0.68f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 11, 0, 64, 64), TimeSpan.FromSeconds(0.1), () => { Opacity = 0.6f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 12, 0, 64, 64), TimeSpan.FromSeconds(0.1), () => { Opacity = 0.5f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 13, 0, 64, 64), TimeSpan.FromSeconds(0.1), () => { Opacity = 0.4f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 14, 0, 64, 64), TimeSpan.FromSeconds(0.1), () => { Opacity = 0.3f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 15, 0, 64, 64), TimeSpan.FromSeconds(0.1), DeathFunc);
            Animations.Add(deathAnimation);
        }

        public override Animation GetCurrentAnimation()
        {
            return Animations.Get(IsAlive ? AnimationTypes.Move : AnimationTypes.Death);
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
            IsAlive = false;
            Speed *= 0.15f;
            target.AddBuff(new BurnBuff(Creator, target, TimeSpan.FromMilliseconds(AbilityConstants.FireballBurnLength), Damage.Damage * AbilityConstants.FireballBurnDamagePercentage));
        }
    }
}
