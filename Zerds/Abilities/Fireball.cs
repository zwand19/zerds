using Microsoft.Xna.Framework;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.GameObjects;
using Zerds.Graphics;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class Fireball : Ability
    {
        public Fireball(Zerd zerd) : base(AbilityTypes.Fireball, zerd, AbilityConstants.FireballCooldown, AbilityConstants.FireballManaCost, "fire-zone.png")
        {
            var fireballAnimation = new Animation(AnimationTypes.FireAttack);
            fireballAnimation.AddFrame(new Rectangle(64 * 1, 0, 64, 64), AbilityConstants.FireballCastTime);
            fireballAnimation.AddFrame(new Rectangle(64 * 3, 0, 64, 64), AbilityConstants.FireballFollowThroughTime, Execute);
            fireballAnimation.AddFrame(new Rectangle(64 * 3, 0, 64, 64), TimeSpan.FromSeconds(0.05), Casted);
            zerd.Animations.Add(fireballAnimation);
        }

        public override void Cast()
        {
            BasicMissileCast(AnimationTypes.FireAttack);
        }

        private bool Casted()
        {
            return Active = false;
        }

        protected override bool Execute()
        {
            var knockback = new Knockback(Being.Facing, AbilityConstants.FireballKnockbackLength, AbilityConstants.FireballKnockback);
            var damage = AbilityConstants.FireballDamage * Being.SkillValue(SkillType.ImprovedFireball, true) *
                         Being.SkillValue(SkillType.FireMastery, true) *
                         Being.AbilityValue(AbilityUpgradeType.FireballDamage, true);
            Globals.GameState.Missiles.Add(new FireballMissile(Being as Zerd,
                new DamageInstance(knockback, damage, DamageTypes.Fire, Being, AbilityTypes.Fireball), Being.Position));
            // replenish mana based on bonuses
            Being.Mana += AbilityConstants.FireballManaCost *
                          (Being.AbilityValue(AbilityUpgradeType.FireballMana) / 100f);
            return base.Execute();
        }
    }
}
