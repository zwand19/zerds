using Zerds.Entities;
using Zerds.Enums;
using Zerds.Constants;
using System;

namespace Zerds.GameObjects
{
    public class DamageInstance
    {
        public Knockback Knockback { get; set; }
        public float Damage { get; set; }
        public bool IsCritical { get; set; }
        public DamageTypes DamageType { get; set; }
        public Being Creator { get; set; }
        public bool IsRanged { get; set; }

        public DamageInstance(Knockback knockback, float damage, DamageTypes type, Being creator, AbilityTypes ability, bool isRanged = true)
        {
            IsRanged = isRanged;
            Knockback = knockback;
            Damage = damage * (1 + GameplayConstants.DamageFactorPerLevel * Level.CurrentLevel);
            DamageType = type;
            Creator = creator;
            var variance = (float)Globals.Random.NextDouble() * GameplayConstants.DamageVariance + 1.0f - GameplayConstants.DamageVariance / 2;
            Damage *= variance;
            if (Creator.IsCritical(DamageType, ability))
            {
                IsCritical = true;
                Damage *= GameplayConstants.CriticalDamageBonus;
                if (Knockback != null)
                {
                    Knockback.Duration = TimeSpan.FromMilliseconds((float) Knockback.Duration.TotalMilliseconds * GameplayConstants.CriticalDamageKnockbackDurationBonus);
                    Knockback.Speed *= GameplayConstants.CriticalDamageKnockbackSpeedBonus;
                }
            }
        }
    }
}
