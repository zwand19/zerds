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
    public class FrostDemonMissile : Missile
    {
        private bool _spawned;

        public FrostDemonMissile(Being being, DamageInstance damageInstance, Point p) : base("Missiles/orbs.png")
        {
            Damage = damageInstance;
            Width = 86;
            Height = 86;
            X = p.X;
            Y = p.Y;
            Creator = being;
            Origin = p;
            Distance = AbilityConstants.FrostDemonMissileLength;
            Speed = AbilityConstants.FrostDemonMissileSpeed;
            Velocity = Creator.Facing.Normalized().Rotate(Helpers.RandomInRange(-3, 3));

            Animations = new AnimationList();
            var spawnAnimation = new Animation(AnimationTypes.Spawn);
            spawnAnimation.AddFrame(new Rectangle(32 * 9, 32 * 6, 32, 32), TimeSpan.FromSeconds(0.15));
            spawnAnimation.AddFrame(new Rectangle(32 * 10, 32 * 6, 32, 32), TimeSpan.FromSeconds(0.15));
            spawnAnimation.AddFrame(new Rectangle(32 * 11, 32 * 6, 32, 32), TimeSpan.FromSeconds(0.15), SpawnedFunc);
            Animations.Add(spawnAnimation);

            var moveAnimation = new Animation(AnimationTypes.Move);
            moveAnimation.AddFrame(new Rectangle(32 * 6, 32 * 6, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 7, 32 * 6, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 8, 32 * 6, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 7, 32 * 6, 32, 32), TimeSpan.FromSeconds(0.15));
            Animations.Add(moveAnimation);

            var dieAnimation = new Animation(AnimationTypes.Death);
            dieAnimation.AddFrame(new Rectangle(32 * 11, 32 * 6, 32, 32), TimeSpan.FromSeconds(0.15));
            dieAnimation.AddFrame(new Rectangle(32 * 10, 32 * 6, 32, 32), TimeSpan.FromSeconds(0.15));
            dieAnimation.AddFrame(new Rectangle(32 * 9, 32 * 6, 32, 32), TimeSpan.FromSeconds(0.15));
            dieAnimation.AddFrame(new Rectangle(32 * 9, 32 * 6, 32, 32), TimeSpan.FromSeconds(0.15), DeathFunc);
            Animations.Add(dieAnimation);
        }


        private bool SpawnedFunc()
        {
            _spawned = true;
            return true;
        }

        public override Animation GetCurrentAnimation()
        {
            return !_spawned
                ? Animations.Get(AnimationTypes.Spawn)
                : Animations.Get(Origin.DistanceBetween(Position) > Distance || !IsAlive ? AnimationTypes.Death : AnimationTypes.Move);
        }

        public override void Update(GameTime gameTime)
        {
            if (Origin.DistanceBetween(Position) > Distance && IsAlive)
            {
                Speed *= 0.75f;
                IsAlive = false;
            }
            base.Update(gameTime);
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
            target.AddBuff(new ColdBuff(target, AbilityConstants.FrostDemonColdLength, AbilityConstants.FrostDemonSlowAmount));
        }
    }
}
