using Microsoft.Xna.Framework;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class Fireball : Ability
    {
        public float FireballDamage { get; set; }

        public Fireball(Being being) : base(AbilityTypes.Fireball, being, AbilityConstants.FireballCooldown, 0f)
        {
            FireballDamage = 10;

            var fireballAnimation = new Animation(AnimationTypes.FireAttack);
            fireballAnimation.AddFrame(new Rectangle(64 * 1, 0, 64, 64), AbilityConstants.FireballCastTime);
            fireballAnimation.AddFrame(new Rectangle(64 * 3, 0, 64, 64), AbilityConstants.FireballFollowThroughTime, Execute);
            fireballAnimation.AddFrame(new Rectangle(64 * 3, 0, 64, 64), TimeSpan.FromSeconds(0.05), Casted);
            being.Animations.Add(fireballAnimation);
        }

        public override bool Cast()
        {
            return BasicMissileCast();
        }

        private bool Casted()
        {
            return Active = false;
        }

        protected override bool Execute()
        {
            Globals.GameState.Missiles.Add(new FireballMissile(Being, new GameObjects.DamageInstance
            {
                Creator = Being,
                Damage = FireballDamage,
                DamageType = DamageTypes.Fire,
                IsCritical = false,
                Knockback = new GameObjects.Knockback(Being.Facing, AbilityConstants.FireballKnockbackLength, AbilityConstants.FireballKnockback)
            }, Being.Position));
            return base.Execute();
        }
    }
}
