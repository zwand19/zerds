using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class Fireball : Ability
    {
        public Fireball(Zerd zerd) : base(AbilityTypes.Fireball, zerd, AbilityConstants.FireballCooldown, AbilityConstants.FireballManaCost, "fireball")
        {
            zerd.AddCastingAnimation(AnimationTypes.FireAttack, AbilityConstants.FireballCastTime, AbilityConstants.FireballFollowThroughTime, Execute, Casted);
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
            Being.AddBuff(BuffTypes.BlazingSpeed);
            return base.Execute();
        }
    }
}
