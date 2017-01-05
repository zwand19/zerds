﻿using System;
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
    public class WandMissile : Missile
    {
        private bool _spawned;

        public WandMissile(Being being, DamageInstance damageInstance, Point p) : base("Missiles/orbs.png")
        {
            Damage = damageInstance;
            Width = 32;
            Height = 32;
            X = p.X;
            Y = p.Y;
            Origin = p;
            Creator = being;
            Distance = AbilityConstants.WandDistance * (1 + Helpers.GetPlayer(being as Zerd).Skills.ImprovedWand * SkillConstants.ImprovedWandStat / 100);
            Speed = AbilityConstants.WandSpeed;
            Velocity = Creator.Facing.Normalized();

            Animations = new AnimationList();
            var spawnAnimation = new Animation(AnimationTypes.Spawn);
            spawnAnimation.AddFrame(new Rectangle(32 * 3, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            spawnAnimation.AddFrame(new Rectangle(32 * 4, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            spawnAnimation.AddFrame(new Rectangle(32 * 5, 0, 32, 32), TimeSpan.FromSeconds(0.15), SpawnedFunc);
            Animations.Add(spawnAnimation);

            var moveAnimation = new Animation(AnimationTypes.Move);
            moveAnimation.AddFrame(new Rectangle(32 * 0, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 1, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 2, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 1, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            Animations.Add(moveAnimation);

            var dieAnimation = new Animation(AnimationTypes.Death);
            dieAnimation.AddFrame(new Rectangle(32 * 5, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            dieAnimation.AddFrame(new Rectangle(32 * 4, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            dieAnimation.AddFrame(new Rectangle(32 * 3, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            dieAnimation.AddFrame(new Rectangle(32 * 3, 0, 32, 32), TimeSpan.FromSeconds(0.15), DeathFunc);
            Animations.Add(dieAnimation);
        }

        private bool SpawnedFunc()
        {
            _spawned = true;
            return true;
        }

        public override Animation GetCurrentAnimation()
        {
            if (!_spawned)
                return Animations.Get(AnimationTypes.Spawn);
            return Animations.Get(Origin.DistanceBetween(Position) > Distance ? AnimationTypes.Death : AnimationTypes.Move);
        }

        public override List<Rectangle> Hitbox()
        {
            return new List<Rectangle> {
                new Rectangle((int)(X - Width * 0.65f), (int)(Y - Width * 0.65f), (int)(Width * 1.3f), (int)(Width * 1.3f))
            };
        }

        public override void OnHit(Being target)
        {
            Damage.DamageBeing(target);
            IsActive = false;
            var zerd = Creator as Zerd;
            if (zerd != null)
            {
                zerd.Mana += zerd.MaxMana * (Helpers.GetPlayer(zerd).Skills.Replenish * SkillConstants.ReplenishStat / 100);
                if (zerd.Mana > zerd.MaxMana)
                    zerd.Mana = zerd.MaxMana;
            }
        }
    }
}
