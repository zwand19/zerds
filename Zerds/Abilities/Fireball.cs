using Microsoft.Xna.Framework;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class Fireball : Ability
    {
        public float FireballDamage { get; set; }

        public Fireball(Zerd zerd) : base(AbilityTypes.Fireball, zerd, AbilityConstants.FireballCooldown, 0f, "fire-zone.png")
        {
            FireballDamage = 10;

            var fireballAnimation = new Animation(AnimationTypes.FireAttack);
            fireballAnimation.AddFrame(new Rectangle(64 * 1, 0, 64, 64), AbilityConstants.FireballCastTime);
            fireballAnimation.AddFrame(new Rectangle(64 * 3, 0, 64, 64), AbilityConstants.FireballFollowThroughTime, Execute);
            fireballAnimation.AddFrame(new Rectangle(64 * 3, 0, 64, 64), TimeSpan.FromSeconds(0.05), Casted);
            zerd.Animations.Add(fireballAnimation);
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
            var knockback = new GameObjects.Knockback(Being.Facing, AbilityConstants.FireballKnockbackLength, AbilityConstants.FireballKnockback);
            var damage = FireballDamage * (1 + Helpers.GetPlayer(Being as Zerd).Skills.ImprovedFireball * SkillConstants.ImprovedFireballStat / 100) *
                         (1 + Helpers.GetPlayer(Being as Zerd).Skills.FireMastery * SkillConstants.FireMasteryStat / 100);
            Globals.GameState.Missiles.Add(new FireballMissile(Being as Zerd, new GameObjects.DamageInstance(knockback, damage, DamageTypes.Fire, Being), Being.Position));
            return base.Execute();
        }
    }
}
