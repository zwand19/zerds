using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.GameObjects;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class Icicle : Ability
    {
        public Icicle(Being being) : base(AbilityTypes.Icicle, being, AbilityConstants.IcicleCooldown, AbilityConstants.IcicleManaCost, "icicle")
        {
        }

        public override void Cast()
        {
            if (Cooldown > TimeSpan.Zero || Being.Mana < ManaCost) return;
            if (Being.GetCurrentAnimation().Name != AnimationTypes.Move && Being.GetCurrentAnimation().Name != AnimationTypes.Stand) return;
            Globals.GameState.Missiles.Add(new IcicleMissile((Zerd)Being, new DamageInstance(null, AbilityConstants.IcicleDamage, DamageTypes.Frost, Being, AbilityTypes.Iceball), Being.Position, 0));
            Globals.GameState.Missiles.Add(new IcicleMissile((Zerd)Being, new DamageInstance(null, AbilityConstants.IcicleDamage, DamageTypes.Frost, Being, AbilityTypes.Iceball), Being.Position, 1));
            Globals.GameState.Missiles.Add(new IcicleMissile((Zerd)Being, new DamageInstance(null, AbilityConstants.IcicleDamage, DamageTypes.Frost, Being, AbilityTypes.Iceball), Being.Position, 2));
            Globals.GameState.Missiles.Add(new IcicleMissile((Zerd)Being, new DamageInstance(null, AbilityConstants.IcicleDamage, DamageTypes.Frost, Being, AbilityTypes.Iceball), Being.Position, 3));
            Globals.GameState.Missiles.Add(new IcicleMissile((Zerd)Being, new DamageInstance(null, AbilityConstants.IcicleDamage, DamageTypes.Frost, Being, AbilityTypes.Iceball), Being.Position, 4));
            Globals.GameState.Missiles.Add(new IcicleMissile((Zerd)Being, new DamageInstance(null, AbilityConstants.IcicleDamage, DamageTypes.Frost, Being, AbilityTypes.Iceball), Being.Position, 5));
            Globals.GameState.Missiles.Add(new IcicleMissile((Zerd)Being, new DamageInstance(null, AbilityConstants.IcicleDamage, DamageTypes.Frost, Being, AbilityTypes.Iceball), Being.Position, 6));
            Globals.GameState.Missiles.Add(new IcicleMissile((Zerd)Being, new DamageInstance(null, AbilityConstants.IcicleDamage, DamageTypes.Frost, Being, AbilityTypes.Iceball), Being.Position, 7));
            Execute();
            // replenish mana based on bonuses
            Being.Mana += AbilityConstants.IcicleManaCost * (Being.SkillValue(SkillType.FrozenSoul, false) / 100f);
        }
    }
}
