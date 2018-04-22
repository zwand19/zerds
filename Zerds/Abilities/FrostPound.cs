using System.Linq;
using Zerds.Buffs;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;

namespace Zerds.Abilities
{
    public class FrostPound : Ability
    {
        public FrostPound(Zerd zerd) : base(AbilityTypes.FrostPound, zerd, AbilityConstants.FrostPoundCooldown, AbilityConstants.FrostPoundCastTime, AbilityConstants.FrostPoundManaCost, "ice-punch")
        {
            zerd.AddCastingAnimation(AnimationTypes.FrostPoundAttack, AbilityConstants.FrostPoundCastTime, AbilityConstants.FrostPoundFollowThroughTime, Execute, Casted);
        }

        public override void Cast()
        {
            BasicMissileCast(AnimationTypes.FrostPoundAttack);
        }

        private bool Casted()
        {
            return Active = false;
        }

        protected override bool Execute()
        {
            var damage = new DamageInstance(null, AbilityConstants.FrostPoundDamage, DamageTypes.Frost, Being,
                AbilityTypes.FrostPound);
            foreach (var enemy in Being.Enemies().Where(e => e.Position.DistanceBetween(Being.Position) < AbilityConstants.FrostPoundRange))
            {
                damage.DamageBeing(enemy);
                enemy.Buffs.Add(new FrozenBuff(enemy, AbilityConstants.FrostPoundFrozenLength));
            }
            // replenish mana based on bonuses
            Being.Mana += AbilityConstants.FrostPoundManaCost * (Being.SkillValue(SkillType.FrozenSoul, false) / 100f);
            return base.Execute();
        }
    }
}
