﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;

namespace Zerds.Abilities
{
    public class Melee : Ability
    {
        private readonly float _minDamage;
        private readonly float _maxDamage;
        private readonly float _knockbackAmount;
        private readonly int _knockbackMillis;
        public Func<Being, List<Rectangle>> GetHitboxes { get; set; }

        public float Damage => Helpers.RandomInRange(_minDamage, _maxDamage);

        public Melee(Being being, float minDamage, float maxDamage, TimeSpan? cooldown = null, float knockback = 250f, int knockbackMillis = 250) : base(AbilityTypes.Melee, being, cooldown ?? TimeSpan.Zero, TimeSpan.Zero, 0, "")
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
            _knockbackAmount = knockback;
            _knockbackMillis = knockbackMillis;
        }

        public DamageInstance GetDamage()
        {
            var knockback = new Knockback(Being.Facing, TimeSpan.FromMilliseconds(_knockbackMillis), _knockbackAmount);
            return new DamageInstance(knockback, Helpers.RandomInRange(_minDamage, _maxDamage), DamageTypes.Physical, Being, AbilityTypes.Melee, false);
        }

        public bool Attacked()
        {
            var rect = Helpers.CreateRect(Being.X + Being.Facing.X * ((Enemy)Being).AttackRange * 0.8f - 10,
                Being.Y - Being.Facing.Y * ((Enemy)Being).AttackRange * 0.8f - 10, 20, 20);
            var rect2 = Helpers.CreateRect(Being.X + Being.Facing.X * ((Enemy)Being).AttackRange * 0.2f - 14,
                Being.Y - Being.Facing.Y * ((Enemy)Being).AttackRange * 0.2f - 14, 28, 28);
            var hitboxes = GetHitboxes == null ? new List<Rectangle> {rect, rect2} : GetHitboxes(Being);
            foreach (var zerd in Being.Enemies())
            {
                if (!zerd.Hitbox().Any(hitbox => hitboxes.Any(hitbox.Intersects))) continue;
                var damageInstance = GetDamage();
                damageInstance.DamageBeing(zerd);
                return true;
            }
            return false;
        }
    }
}
