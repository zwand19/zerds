using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Zerds.Buffs;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;
using Zerds.Graphics;

namespace Zerds.Abilities
{
    public class FrostPound : Ability
    {
        public FrostPound(Being being) : base(AbilityTypes.FrostPound, being, AbilityConstants.FrostPoundCooldown, AbilityConstants.FrostPoundManaCost, "ice-punch")
        {
            var anim = new Animation(AnimationTypes.FrostPoundAttack);
            anim.AddFrame(new Rectangle(64 * 8, 0, 64, 64), AbilityConstants.FrostPoundCastTime);
            anim.AddFrame(new Rectangle(64 * 9, 0, 64, 64), AbilityConstants.FrostPoundFollowThroughTime, Execute);
            anim.AddFrame(new Rectangle(64 * 9, 0, 64, 64), TimeSpan.FromSeconds(0.05), Casted);
            being.Animations.Add(anim);
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
            foreach (var enemy in
                Globals.GameState.Enemies.Where(
                    e => e.Position.DistanceBetween(Being.Position) < AbilityConstants.FrostPoundRange))
            {
                damage.DamageBeing(enemy);
                enemy.Buffs.Add(new FrozenBuff(enemy, AbilityConstants.FrostPoundFrozenLength));
            }
            // replenish mana based on bonuses
            Being.Mana += AbilityConstants.IceballManaCost * (Being.SkillValue(SkillType.FrozenSoul, false) / 100f);
            return base.Execute();
        }
    }
}
