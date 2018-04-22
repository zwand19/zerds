using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.GameObjects;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class Iceball : Ability
    {
        public Iceball(Zerd zerd) : base(AbilityTypes.Iceball, zerd, AbilityConstants.IceballCooldown, AbilityConstants.IceballCastTime, AbilityConstants.IceballManaCost, "ice-bolt")
        {
            zerd.AddCastingAnimation(AnimationTypes.FrostAttack, AbilityConstants.IceballCastTime, AbilityConstants.IceballFollowThroughTime, Execute, Casted);
        }

        public override void Cast()
        {
            BasicMissileCast(AnimationTypes.FrostAttack);
        }

        private bool Casted()
        {
            return Active = false;
        }

        protected override bool Execute()
        {
            var knockback = new Knockback(Being.Facing, AbilityConstants.IceballKnockbackLength, AbilityConstants.IceballKnockback);
            var damage = AbilityConstants.IceballDamage * Being.SkillValue(SkillType.ImprovedIceball, true);
            Globals.GameState.Missiles.Add(new IceballMissile((Zerd) Being,
                new DamageInstance(knockback, damage, DamageTypes.Frost, Being, AbilityTypes.Iceball), Being.Position));
            // replenish mana based on bonuses
            Being.Mana += AbilityConstants.IceballManaCost *
                          ((Being.SkillValue(SkillType.FrozenSoul, false) + Being.AbilityValue(AbilityUpgradeType.IceballMana)) /
                           100f);
            return base.Execute();
        }
    }
}
