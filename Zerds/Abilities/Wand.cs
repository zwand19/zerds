using Microsoft.Xna.Framework;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class Wand : Ability
    {
        public float WandDamage { get; set; }

        public Wand(Being being) : base(AbilityTypes.Wand, being, AbilityConstants.WandCooldown, 0f)
        {
            WandDamage = 10;

            var wandAnimation = new Animation(AnimationTypes.Attack);
            wandAnimation.AddFrame(new Rectangle(64 * 2, 0, 64, 64), AbilityConstants.WandCastTime);
            wandAnimation.AddFrame(new Rectangle(64 * 4, 0, 64, 64), AbilityConstants.WandFollowThroughTime, Execute);
            wandAnimation.AddFrame(new Rectangle(64 * 4, 0, 64, 64), TimeSpan.FromSeconds(0.05), Casted);
            being.Animations.Add(wandAnimation);
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
            Globals.GameState.Missiles.Add(new WandMissile(Being, new GameObjects.DamageInstance
            {
                Creator = Being,
                Damage = WandDamage,
                DamageType = DamageTypes.Magic,
                IsCritical = false,
                Knockback = new GameObjects.Knockback(Being.Facing, AbilityConstants.WandKnockbackLength, AbilityConstants.WandKnockback)
            }, Being.Position));
            return base.Execute();
        }
    }
}
