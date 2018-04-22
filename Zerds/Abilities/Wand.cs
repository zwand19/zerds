using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class Wand : Ability
    {
        public Wand(Zerd zerd) : base(AbilityTypes.Wand, zerd, AbilityConstants.WandCooldown, AbilityConstants.WandCastTime, 0f, "fairy-wand")
        {
            if (zerd.Player.ChargeAbilities)
            {
                zerd.AddCastingAnimation(AnimationTypes.Attack, TimeSpan.Zero, AbilityConstants.WandFollowThroughTime, null, Casted);
            }
            else
            {
                zerd.AddCastingAnimation(AnimationTypes.Attack, AbilityConstants.WandCastTime, AbilityConstants.WandFollowThroughTime, Execute, Casted);
            }
        }

        public override void Cast()
        {
            BasicMissileCast(AnimationTypes.Attack);
        }

        private bool Casted()
        {
            return Active = false;
        }

        public override void ReleaseCharge()
        {
            var knockback = new GameObjects.Knockback(Being.Facing, AbilityConstants.WandKnockbackLength,
                AbilityConstants.WandKnockback);
            var damage = AbilityConstants.WandDamage * Being.SkillValue(SkillType.ImprovedWand, true);
            Globals.GameState.Missiles.Add(new WandMissile(Being,
                new GameObjects.DamageInstance(knockback, damage, DamageTypes.Magic, Being, AbilityTypes.Wand),
                Being.Position));
        }

        protected override bool Execute()
        {
            var knockback = new GameObjects.Knockback(Being.Facing, AbilityConstants.WandKnockbackLength,
                AbilityConstants.WandKnockback);
            var damage = AbilityConstants.WandDamage * Being.SkillValue(SkillType.ImprovedWand, true);
            Globals.GameState.Missiles.Add(new WandMissile(Being,
                new GameObjects.DamageInstance(knockback, damage, DamageTypes.Magic, Being, AbilityTypes.Wand),
                Being.Position));
            return base.Execute();
        }
    }
}
