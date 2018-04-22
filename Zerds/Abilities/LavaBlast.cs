using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class LavaBlast : Ability
    {
        public LavaBlast(Zerd zerd) : base(AbilityTypes.LavaBlast, zerd, AbilityConstants.LavaBlastCooldown, AbilityConstants.LavaBlastCastTime, AbilityConstants.LavaBlastManaCost, "lava_blast")
        {
            zerd.AddCastingAnimation(AnimationTypes.LavaBlastAttack, AbilityConstants.LavaBlastCastTime, AbilityConstants.LavaBlastFollowThroughTime, Execute, Casted);
        }

        public override void Cast()
        {
            BasicMissileCast(AnimationTypes.LavaBlastAttack);
        }

        private bool Casted()
        {
            return Active = false;
        }

        protected override bool Execute()
        {
            var knockback = new GameObjects.Knockback(Being.Facing, AbilityConstants.LavaBlastKnockbackLength, AbilityConstants.LavaBlastKnockback);
            var damage = AbilityConstants.LavaBlastDamage * Being.SkillValue(SkillType.FireMastery, true);
            Globals.GameState.Missiles.Add(new LavaBlastMissile(Being as Zerd, new GameObjects.DamageInstance(knockback, damage, DamageTypes.Fire, Being, AbilityTypes.LavaBlast),
                Being.Position));
            Being.AddBuff(BuffTypes.BlazingSpeed);
            return base.Execute();
        }
    }
}
