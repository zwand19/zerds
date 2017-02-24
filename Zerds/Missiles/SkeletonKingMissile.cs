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
    public class SkeletonKingMissile : Missile
    {
        private bool _spawned;

        public SkeletonKingMissile(Being being, DamageInstance damageInstance, Vector2 velocity) : base("Missiles/orbs.png")
        {
            Damage = damageInstance;
            Width = 96;
            Height = 96;
            X = being.X;
            Y = being.Y;
            Creator = being;
            Origin = being.Position;
            Distance = 1000f;
            Speed = 600f;
            Velocity = velocity;

            Animations = new AnimationList();
            var spawnAnimation = new Animation(AnimationTypes.Spawn);
            spawnAnimation.AddFrame(new Rectangle(32 * 3, 32 * 4, 32, 32), TimeSpan.FromSeconds(0.15));
            spawnAnimation.AddFrame(new Rectangle(32 * 4, 32 * 4, 32, 32), TimeSpan.FromSeconds(0.15));
            spawnAnimation.AddFrame(new Rectangle(32 * 5, 32 * 4, 32, 32), TimeSpan.FromSeconds(0.15), SpawnedFunc);
            Animations.Add(spawnAnimation);

            var moveAnimation = new Animation(AnimationTypes.Move);
            moveAnimation.AddFrame(new Rectangle(32 * 0, 32 * 4, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 1, 32 * 4, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 2, 32 * 4, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 1, 32 * 4, 32, 32), TimeSpan.FromSeconds(0.15));
            Animations.Add(moveAnimation);

            var dieAnimation = new Animation(AnimationTypes.Death);
            dieAnimation.AddFrame(new Rectangle(32 * 5, 32 * 4, 32, 32), TimeSpan.FromSeconds(0.15));
            dieAnimation.AddFrame(new Rectangle(32 * 4, 32 * 4, 32, 32), TimeSpan.FromSeconds(0.15));
            dieAnimation.AddFrame(new Rectangle(32 * 3, 32 * 4, 32, 32), TimeSpan.FromSeconds(0.15));
            dieAnimation.AddFrame(new Rectangle(32 * 3, 32 * 4, 32, 32), TimeSpan.FromSeconds(0.15), DeathFunc);
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
            target.AddBuff(new BurnBuff(Creator, target, TimeSpan.FromSeconds(2), Damage.Damage * 0.35f));
        }
    }
}
