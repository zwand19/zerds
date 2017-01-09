using Microsoft.Xna.Framework;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.GameObjects;
using Zerds.Graphics;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class Iceball : Ability
    {
        public float IceballDamage { get; set; }

        public Iceball(Being being) : base(AbilityTypes.Iceball, being, AbilityConstants.IceballCooldown, AbilityConstants.IceballManaCost, "ice-bolt.png")
        {
            IceballDamage = 10;

            var iceballAnimation = new Animation(AnimationTypes.FrostAttack);
            iceballAnimation.AddFrame(new Rectangle(64 * 8, 0, 64, 64), AbilityConstants.IceballCastTime);
            iceballAnimation.AddFrame(new Rectangle(64 * 9, 0, 64, 64), AbilityConstants.IceballFollowThroughTime, Execute);
            iceballAnimation.AddFrame(new Rectangle(64 * 9, 0, 64, 64), TimeSpan.FromSeconds(0.05), Casted);
            being.Animations.Add(iceballAnimation);
        }

        public override void Cast()
        {
            BasicMissileCast();
        }

        private bool Casted()
        {
            return Active = false;
        }

        protected override bool Execute()
        {
            var knockback = new Knockback(Being.Facing, AbilityConstants.IceballKnockbackLength, AbilityConstants.IceballKnockback);
            var damage = IceballDamage * (1 + ((Zerd)Being).Player.Skills.ImprovedIceball * SkillConstants.ImprovedIceballStat / 100);
            Globals.GameState.Missiles.Add(new IceballMissile((Zerd)Being, new DamageInstance(knockback, damage, DamageTypes.Frost, Being, AbilityTypes.Iceball), Being.Position));
            // replenish mana based on bonuses
            var zerd = (Zerd) Being;
            if (zerd != null)
                zerd.Mana += AbilityConstants.IceballManaCost * ((zerd.Player.Skills.FrozenSoul * SkillConstants.FrozenSoulStat + zerd.Player.AbilityUpgrades[AbilityUpgradeType.IceballMana]) / 100f);
            return base.Execute();
        }
    }
}
