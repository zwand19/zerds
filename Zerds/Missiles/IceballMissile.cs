using System;
using Microsoft.Xna.Framework;
using Zerds.GameObjects;
using Zerds.Entities;
using Zerds.Graphics;
using Zerds.Enums;
using Zerds.Constants;
using System.Collections.Generic;
using System.Linq;
using Zerds.Factories;
using Zerds.Buffs;

namespace Zerds.Missiles
{
    public class IceballMissile : Missile
    {
        private float _rotation;

        public IceballMissile(Zerd zerd, DamageInstance damageInstance, Point p) : base("Missiles/iceball.png")
        {
            Damage = damageInstance;
            Width = 64;
            Height = 64;
            X = p.X;
            Y = p.Y;
            Creator = zerd;
            Origin = p;
            Distance = AbilityConstants.IceballDistance;
            Speed = AbilityConstants.IceballSpeed;
            Velocity = Creator.Facing.Normalized();

            Animations = new AnimationList();
            var moveAnimation = new Animation(AnimationTypes.Move);
            moveAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            Animations.Add(moveAnimation);
            var deathAnimation = new Animation(AnimationTypes.Death);
            deathAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), TimeSpan.FromSeconds(0.08));
            deathAnimation.AddFrame(new Rectangle(64 * 1, 0, 64, 64), TimeSpan.FromSeconds(0.08), () => { Opacity = 0.93f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 2, 0, 64, 64), TimeSpan.FromSeconds(0.08), () => { Opacity = 0.85f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 3, 0, 64, 64), TimeSpan.FromSeconds(0.08), () => { Opacity = 0.78f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 4, 0, 64, 64), TimeSpan.FromSeconds(0.08), () => { Opacity = 0.71f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 5, 0, 64, 64), TimeSpan.FromSeconds(0.08), () => { Opacity = 0.64f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 6, 0, 64, 64), TimeSpan.FromSeconds(0.08), () => { Opacity = 0.58f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 7, 0, 64, 64), TimeSpan.FromSeconds(0.08), () => { Opacity = 0.51f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 8, 0, 64, 64), TimeSpan.FromSeconds(0.08), () => { Opacity = 0.46f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 9, 0, 64, 64), TimeSpan.FromSeconds(0.08), () => { Opacity = 0.38f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 10, 0, 64, 64), TimeSpan.FromSeconds(0.08), () => { Opacity = 0.3f; return true; });
            deathAnimation.AddFrame(new Rectangle(64 * 11, 0, 64, 64), TimeSpan.FromSeconds(0.08), DeathFunc);
            Animations.Add(deathAnimation);
        }

        public override Animation GetCurrentAnimation()
        {
            return Animations.Get(IsAlive ? AnimationTypes.Move : AnimationTypes.Death);
        }

        public override void Update(GameTime gameTime)
        {
            _rotation += (float)Math.PI * 2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        public override float SpriteRotation()
        {
            return _rotation;
        }

        public override List<Rectangle> Hitbox()
        {
            return new List<Rectangle> {
                new Rectangle((int)(X - Width * 0.4f), (int)(Y - Width * 0.4f), (int)(Width * 0.8f), (int)(Width * 0.8f)),
            };
        }

        public override void OnHit(Being target)
        {
            var zerd = (Zerd)Creator;
            Damage.DamageBeing(target);
            IsAlive = false;
            Speed *= 0.15f;
            var slowLevel = AbilityConstants.ColdSpeedFactor * zerd.SkillValue(SkillType.ImprovedIceball, true);
            var coldLength =
                TimeSpan.FromMilliseconds(AbilityConstants.IceballColdLength.TotalMilliseconds *
                                          zerd.SkillValue(SkillType.BitterCold, true));
            target.AddBuff(new ColdBuff(target, coldLength, slowLevel));
            if (zerd.SkillPoints(SkillType.ColdExplosion) > 0)
            {
                var explosionSlowLevel = zerd.SkillValue(SkillType.ColdExplosion) * AbilityConstants.ColdSpeedFactor *
                                         zerd.SkillValue(SkillType.ImprovedIceball, true);
                Damage.Damage *= zerd.SkillValue(SkillType.ColdExplosion) / 100f;
                foreach (
                    var e in
                    Globals.GameState.Enemies.Where(
                        e => target.Position.DistanceBetween(e.Position) < AbilityConstants.IceballExplosionDistance && e != target))
                {
                    e.AddBuff(new ColdBuff(target, coldLength, explosionSlowLevel));
                    Damage.DamageBeing(e);
                }
            }
        }
    }
}
