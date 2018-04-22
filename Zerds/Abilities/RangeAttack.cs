using Microsoft.Xna.Framework;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.GameObjects;
using Zerds.Missiles;

namespace Zerds.Abilities
{
    public class RangeAttack : Ability
    {
        private readonly float _minDamage;
        private readonly float _maxDamage;
        private readonly MissileTypes _missileType;
        
        public float Damage => Helpers.RandomInRange(_minDamage, _maxDamage);

        public RangeAttack(Enemy being, float minDamage, float maxDamage, MissileTypes missileType, TimeSpan? cooldown = null) : base(AbilityTypes.Melee, being, cooldown ?? TimeSpan.Zero, TimeSpan.Zero, 0, "")
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
            _missileType = missileType;
        }

        public DamageInstance GetDamage()
        {
            switch (_missileType)
            {
                case MissileTypes.DemonMissile:
                    var knockback = new Knockback(Being.Facing, EnemyConstants.DemonMissileKnockbackLength, EnemyConstants.DemonMissileKnockback);
                    return new DamageInstance(knockback, Helpers.RandomInRange(_minDamage, _maxDamage), DamageTypes.Fire, Being, AbilityTypes.Range);
                case MissileTypes.FrostDemonMissile:
                    return new DamageInstance(null, Helpers.RandomInRange(_minDamage, _maxDamage), DamageTypes.Frost, Being, AbilityTypes.Range);
                case MissileTypes.Arrow:
                    var arrowKnockback = new Knockback(Being.Facing, EnemyConstants.ArcherArrowKnockbackLength, EnemyConstants.ArcherArrowKnockback);
                    return new DamageInstance(arrowKnockback, Helpers.RandomInRange(_minDamage, _maxDamage), DamageTypes.Physical, Being, AbilityTypes.Range);
            }
            throw new Exception("Unknown missile type");
        }

        public bool Attacked()
        {
            Cooldown = TimeSpan.FromMilliseconds(FullCooldown.TotalMilliseconds);
            switch (_missileType)
            {
                case MissileTypes.DemonMissile:
                    Globals.GameState.Missiles.Add(new DemonMissile(Being, GetDamage(), Being.Position));
                    return true;
                case MissileTypes.FrostDemonMissile:
                    Globals.GameState.Missiles.Add(new FrostDemonMissile(Being, GetDamage(), Being.Position));
                    return true;
                case MissileTypes.Arrow:
                    Globals.GameState.Missiles.Add(new ArrowMissile(Being, GetDamage(), Being.Position));
                    return true;
            }
            return false;
        }
    }
}
