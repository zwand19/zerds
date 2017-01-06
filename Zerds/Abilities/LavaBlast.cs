﻿using Microsoft.Xna.Framework;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class LavaBlast : Ability
    {
        public float LavaBlastDamage { get; set; }

        public LavaBlast(Zerd zerd) : base(AbilityTypes.LavaBlast, zerd, AbilityConstants.LavaBlastCooldown, 0f, "volcano.png")
        {
            LavaBlastDamage = 14;

            var anim = new Animation(AnimationTypes.LavaBlastAttack);
            anim.AddFrame(new Rectangle(64 * 1, 0, 64, 64), AbilityConstants.LavaBlastCastTime);
            anim.AddFrame(new Rectangle(64 * 3, 0, 64, 64), AbilityConstants.LavaBlastFollowThroughTime, Execute);
            anim.AddFrame(new Rectangle(64 * 3, 0, 64, 64), TimeSpan.FromSeconds(0.05), Casted);
            zerd.Animations.Add(anim);
        }

        public override void Cast()
        {
            BasicMissileCast();
        }

        private bool Casted()
        {
            return Active = false;
        }

        protected override bool Execute()
        {
            var knockback = new GameObjects.Knockback(Being.Facing, AbilityConstants.LavaBlastKnockbackLength, AbilityConstants.LavaBlastKnockback);
            var damage = LavaBlastDamage * (1 + Helpers.GetPlayer(Being as Zerd).Skills.FireMastery * SkillConstants.FireMasteryStat / 100);
            Globals.GameState.Missiles.Add(new LavaBlastMissile(Being as Zerd, new GameObjects.DamageInstance(knockback, damage, DamageTypes.Fire, Being), Being.Position));
            return base.Execute();
        }
    }
}