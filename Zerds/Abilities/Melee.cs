using System;
using System.Linq;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;

namespace Zerds.Abilities
{
    public class Melee : Ability
    {
        private readonly float _minDamage;
        private readonly float _maxDamage;

        public float Damage => Helpers.RandomInRange(_minDamage, _maxDamage);

        public Melee(Being being, float minDamage, float maxDamage, TimeSpan? cooldown = null) : base(AbilityTypes.Melee, being, cooldown ?? TimeSpan.Zero, 0, "")
        {
            _minDamage = minDamage;
            _maxDamage = maxDamage;
        }

        public DamageInstance GetDamage()
        {
            var knockback = new Knockback(Being.Facing, TimeSpan.FromMilliseconds(250), 250f);
            return new DamageInstance(knockback, Helpers.RandomInRange(_minDamage, _maxDamage), DamageTypes.Physical, Being, AbilityTypes.Melee);
        }

        public bool Attacked()
        {
            var rect = Helpers.CreateRect(Being.X + Being.Facing.X * ((Enemy)Being).AttackRange * 0.8f - 10,
                Being.Y - Being.Facing.Y * ((Enemy)Being).AttackRange * 0.8f - 10, 20, 20);
            var rect2 = Helpers.CreateRect(Being.X + Being.Facing.X * ((Enemy)Being).AttackRange * 0.2f - 14,
                Being.Y - Being.Facing.Y * ((Enemy)Being).AttackRange * 0.2f - 14, 28, 28);
            foreach (var zerd in Globals.GameState.Zerds)
            {
                if (!zerd.Hitbox().Any(hitbox => hitbox.Intersects(rect) || hitbox.Intersects(rect2))) continue;
                var damageInstance = GetDamage();
                damageInstance.DamageBeing(zerd);
                return true;
            }
            return false;
        }
    }
}
