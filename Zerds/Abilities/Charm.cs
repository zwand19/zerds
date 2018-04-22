using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class Charm : Ability
    {
        public Charm(Zerd zerd) : base(AbilityTypes.Charm, zerd, AbilityConstants.CharmCooldown, AbilityConstants.CharmCastTime, AbilityConstants.CharmManaCost, "chained-heart")
        {
            zerd.AddCastingAnimation(AnimationTypes.Charm, AbilityConstants.CharmCastTime, AbilityConstants.CharmFollowThroughTime, Execute, Casted);
        }

        public override void Cast()
        {
            BasicMissileCast(AnimationTypes.Attack);
        }

        private bool Casted()
        {
            return Active = false;
        }

        protected override bool Execute()
        {
            Globals.GameState.Missiles.Add(new CharmMissile(Being, Being.Position));
            return base.Execute();
        }
    }
}
