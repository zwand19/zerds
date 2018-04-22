using System;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;
using Zerds.Graphics;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class DragonBreath : Ability
    {
        public DragonBreath(Zerd zerd) : base(AbilityTypes.DragonsBreath, zerd, AbilityConstants.DragonBreathCooldown, TimeSpan.Zero, AbilityConstants.DragonBreathManaCost, "dragon-breath")
        {
            ZerdAnimationHelpers.AddDragonsBreathAnimation(zerd, Casted, Execute, MakeMissile);
        }

        public override void Cast()
        {
            BasicMissileCast(AnimationTypes.FireBreath);
        }

        private bool Casted()
        {
            return Active = false;
        }

        private bool MakeMissile()
        {
            var damage = AbilityConstants.DragonBreathDamage * Being.SkillValue(SkillType.FireMastery, true);
            Globals.GameState.Missiles.Add(new DragonBreathMissile(Being as Zerd, new DamageInstance(null, damage, DamageTypes.Fire, Being, AbilityTypes.DragonsBreath), Being.Position));
            return true;
        }

        protected override bool Execute()
        {
            Being.AddBuff(BuffTypes.BlazingSpeed);
            Being.Mana -= ManaCost;
            if (Being.Mana < 0)
                Being.Mana = 0;
            Cooldown = TimeSpan.FromSeconds(FullCooldown.TotalSeconds);
            return true;
        }
    }
}
