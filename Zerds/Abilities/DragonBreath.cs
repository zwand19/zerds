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
        public DragonBreath(Zerd zerd) : base(AbilityTypes.DragonsBreath, zerd, AbilityConstants.DragonBreathCooldown, AbilityConstants.DragonBreathManaCost, "dragon-breath")
        {
            var walkAnimation = new Animation(AnimationTypes.FireBreath);
            walkAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), TimeSpan.FromSeconds(0.04));
            walkAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), TimeSpan.FromSeconds(0.04), Execute);
            for (var i = 0; i < 2 / 0.08; i++)
            {
                walkAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), TimeSpan.FromSeconds(0.04));
                walkAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), TimeSpan.FromSeconds(0.04), MakeMissile);
            }
            walkAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), TimeSpan.FromSeconds(0.04), Casted);
            zerd.Animations.Add(walkAnimation);
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
