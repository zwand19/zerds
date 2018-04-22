using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.GameObjects;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class SkeletonKingBlast : Ability
    {
        public SkeletonKingBlast(Enemy being) : base(AbilityTypes.SkeletonKingBlast, being, EnemyConstants.SkeletonKingBlastCooldown, TimeSpan.Zero, 0, "")
        {
            Cooldown = EnemyConstants.SkeletonKingBlastCooldown;
        }

        public DamageInstance GetDamage()
        {
            var knockback = new Knockback(Being.Facing, EnemyConstants.SkeletonKingBlastMissileKnockbackLength, EnemyConstants.SkeletonKingBlastMissileKnockback);
            return new DamageInstance(knockback, EnemyConstants.SkeletonKingBlastMissileDamage, DamageTypes.Fire, Being, AbilityTypes.SkeletonKingBlast);
        }

        public bool Casted()
        {
            Cooldown = TimeSpan.FromMilliseconds(FullCooldown.TotalMilliseconds);
            var numMissiles = 10;
            for (var i = 0; i < numMissiles; i++)
            {
                var direction = Being.Facing.Rotate(i * 360f / numMissiles);
                var knockback = new Knockback(direction, TimeSpan.FromMilliseconds(250), 500f);
                var damageInstance = new DamageInstance(knockback, 45f, DamageTypes.Fire, Being, AbilityTypes.SkeletonKingBlast);
                Globals.GameState.Missiles.Add(new SkeletonKingMissile(Being, damageInstance, direction));
            }
            return false;
        }
    }
}
