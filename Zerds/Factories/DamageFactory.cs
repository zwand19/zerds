using System;
using System.Linq;
using Zerds.Buffs;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.GameObjects;
using Zerds.Input;

namespace Zerds.Factories
{
    public static class DamageFactory
    {
        public static Func<DamageText, bool> AddText { get; set; }

        public static void DamageBeing(this DamageInstance damageInstance, Being target)
        {
            var zerdCreator = damageInstance.Creator as Zerd;
            var zerdTarget = target as Zerd;
            // Damage Taken Ability Upgrade
            if (zerdTarget != null)
                damageInstance.Damage *= 1 - zerdTarget.AbilityValue(AbilityUpgradeType.DamageTaken) / 100f;
            // Hardened Skill
            if (zerdTarget != null && damageInstance.IsRanged)
                damageInstance.Damage *= 1 - zerdTarget.SkillValue(SkillType.Hardened, false) / 100f;
            // Exposure Skill
            if (zerdCreator != null && target.Buffs.Any(b => b is BurnBuff) && zerdCreator.SkillPoints(SkillType.Exposure) > 0)
                damageInstance.Damage *= zerdCreator.SkillValue(SkillType.Exposure, true);
            // Maniac Skill
            if (zerdCreator != null && zerdCreator.SkillPoints(SkillType.Maniac) > 0)
                damageInstance.Damage += zerdCreator.SkillValue(SkillType.Maniac, false) * (1 - zerdCreator.HealthPercentage) * damageInstance.Damage / 100;
            // Bleed Fire Skill
            if (zerdCreator != null && zerdCreator.SkillPoints(SkillType.BleedFire) > 0)
                zerdCreator.AddHealth(zerdCreator.SkillValue(SkillType.BleedFire, false) * damageInstance.Damage / 100);
            // Deep Cold Skill
            if (zerdCreator != null && zerdCreator.SkillPoints(SkillType.DeepCold) > 0 && target.Buffs.Any(b => b is ColdBuff) && damageInstance.DamageType == DamageTypes.Frost)
                damageInstance.Damage *= zerdCreator.SkillValue(SkillType.DeepCold, true);
            // Shatter Skill
            if (zerdCreator != null && zerdCreator.SkillPoints(SkillType.Shatter) > 0 && target.Buffs.Any(b => b is FrozenBuff))
                damageInstance.Damage *= zerdCreator.SkillValue(SkillType.Shatter, true);
            // Ice Shield Skill
            if (zerdCreator != null && damageInstance.DamageType == DamageTypes.Frost && damageInstance.IsCritical && zerdCreator.Buffs.All(b => !b.GrantsInvulnerability))
                zerdCreator.AddBuff(new IceShieldBuff(zerdCreator, TimeSpan.FromSeconds(zerdCreator.SkillValue(SkillType.IceShield, false))));
            // Frost Aura Skill
            if (zerdCreator != null && zerdCreator.SkillPoints(SkillType.FrostAura) > 0 && target.DistanceBetween(zerdCreator) < PlayerSkills.FrostAuraRange)
                damageInstance.Damage *= zerdCreator.SkillValue(SkillType.FrostAura, true);

            target.Health -= damageInstance.Damage;
            if (damageInstance.Knockback != null)
                target.Knockback = new Knockback((target.PositionVector - damageInstance.Creator.PositionVector).Normalized(), damageInstance.Knockback.MaxDuration, damageInstance.Knockback.Speed);
            if (zerdTarget != null)
                ControllerService.Controllers[zerdTarget.Player.PlayerIndex].VibrateController(TimeSpan.FromMilliseconds(250), 1f);
            AddText(new DamageText(damageInstance, target));
            if (target.Health < 0 && target.Killer == null)
            {
                target.Killer = damageInstance.Creator;
                zerdCreator?.EnemyKilled(target as Enemy);
            }
        }
    }
}