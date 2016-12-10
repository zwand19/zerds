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

        public void TryCritical()
        {
            var variance = (float)new Random().NextDouble() * GameplayConstants.DamageVariance + 1.0f - GameplayConstants.DamageVariance / 2;
            Damage *= variance;
            if (Creator.IsCritical(DamageType))
            {
                IsCritical = true;
                Damage *= GameplayConstants.CriticalDamageBonus;
                Knockback.Duration = TimeSpan.FromMilliseconds((float)Knockback.Duration.TotalMilliseconds * GameplayConstants.CriticalDamageKnockbackDurationBonus);
                Knockback.Speed *= GameplayConstants.CriticalDamageKnockbackSpeedBonus;
            }
        }
    }
}
