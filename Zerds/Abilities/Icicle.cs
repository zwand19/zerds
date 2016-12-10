﻿using Microsoft.Xna.Framework;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class Iceball : Ability
    {
        public float IceballDamage { get; set; }

        public Iceball(Being being) : base(AbilityTypes.Iceball, being, AbilityConstants.IceballCooldown, 0f)
        {
            IceballDamage = 10;

            var iceballAnimation = new Animation(AnimationTypes.FrostAttack);
            iceballAnimation.AddFrame(new Rectangle(64 * 8, 0, 64, 64), AbilityConstants.IceballCastTime);
            iceballAnimation.AddFrame(new Rectangle(64 * 9, 0, 64, 64), AbilityConstants.IceballFollowThroughTime, Execute);
            iceballAnimation.AddFrame(new Rectangle(64 * 9, 0, 64, 64), TimeSpan.FromSeconds(0.05), Casted);
            being.Animations.Add(iceballAnimation);
        }

        public override bool Cast()
        {
            return BasicMissileCast();
        }

        private bool Casted()
        {
            return Active = false;
        }

        protected override bool Execute()
        {
            Globals.GameState.Missiles.Add(new IceballMissile(Being, new GameObjects.DamageInstance
            {
                Creator = Being,
                Damage = IceballDamage,
                DamageType = DamageTypes.Frost,
                IsCritical = false,
                Knockback = new GameObjects.Knockback(Being.Facing, AbilityConstants.IceballKnockbackLength, AbilityConstants.IceballKnockback)
            }, Being.Position));
            return base.Execute();
        }
    }
}
