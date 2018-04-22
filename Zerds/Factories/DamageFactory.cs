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

        public static void DamageBeing(this DamageInstance damageInstance, Being target, bool firstCall = true)
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

            // Gloves
            if (zerdCreator != null && zerdCreator.GloveItem.SpellDamage > 0)
                damageInstance.Damage *= 1 + zerdCreator.GloveItem.SpellDamage / 100f;

            // Deaths
            if (zerdCreator != null)
            {
                for (var i = 0; i < zerdCreator.Deaths; i++)
                    damageInstance.Damage *= 1 - DifficultyConstants.RevivalSelfPenalty;
                for (var i = 0; i < zerdCreator.TeammateDeaths; i++)
                    damageInstance.Damage *= 1 - DifficultyConstants.RevivalTeammatePenalty;
            }

            zerdCreator?.Stats.DealtDamage(damageInstance);
            zerdTarget?.Stats.TookDamage(damageInstance);

            target.Health -= damageInstance.Damage;
            if (damageInstance.Knockback != null)
            {
                // Reduce knockback based on robe item
                if (zerdTarget?.RobeItem.KnockbackReduction > 0.01)
                {
                    damageInstance.Knockback.Duration = TimeSpan.FromMilliseconds(damageInstance.Knockback.MaxDuration.TotalMilliseconds * (1 - zerdTarget.RobeItem.KnockbackReduction));
                    damageInstance.Knockback.Speed *= 1 - zerdTarget.RobeItem.KnockbackReduction;
                }
                target.Knockback = new Knockback((target.PositionVector - damageInstance.Creator.PositionVector).Normalized(), damageInstance.Knockback.Duration, damageInstance.Knockback.Speed);
            }
            if (zerdTarget != null)
                InputService.Vibrate(zerdTarget.Player.PlayerIndex, TimeSpan.FromMilliseconds(250), 1f);
            if (damageInstance.Damage >= 1) AddText(new DamageText(damageInstance, target));
            if (target.Health < 0 && target.Killer == null)
            {
                target.Killer = damageInstance.Creator;
                zerdCreator?.Stats.EnemyKilled(target as Enemy);
            }

            // Return Damage
            if (firstCall && zerdTarget != null && damageInstance.Damage > 1 && zerdTarget.RobeItem.Thorns > 0 && damageInstance.Creator != target)
            {
                var dmg = new DamageInstance(null, zerdTarget.RobeItem.Thorns, DamageTypes.Physical, zerdTarget, AbilityTypes.Thorns, false);
                dmg.DamageBeing(damageInstance.Creator, false);
            }
        }
    }
}