using Microsoft.Xna.Framework;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class LavaBlast : Ability
    {
        public LavaBlast(Zerd zerd) : base(AbilityTypes.LavaBlast, zerd, AbilityConstants.LavaBlastCooldown, AbilityConstants.LavaBlastManaCost, "lava_blast.png")
        {
            var anim = new Animation(AnimationTypes.LavaBlastAttack);
            anim.AddFrame(new Rectangle(64 * 1, 0, 64, 64), AbilityConstants.LavaBlastCastTime);
            anim.AddFrame(new Rectangle(64 * 3, 0, 64, 64), AbilityConstants.LavaBlastFollowThroughTime, Execute);
            anim.AddFrame(new Rectangle(64 * 3, 0, 64, 64), TimeSpan.FromSeconds(0.05), Casted);
            zerd.Animations.Add(anim);
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
            var damage = AbilityConstants.LavaBlastDamage * Being.SkillValue(SkillType.FireMastery);
            Globals.GameState.Missiles.Add(new LavaBlastMissile(Being as Zerd, new GameObjects.DamageInstance(knockback, damage, DamageTypes.Fire, Being, AbilityTypes.LavaBlast),
                Being.Position));
            return base.Execute();
        }
    }
}
