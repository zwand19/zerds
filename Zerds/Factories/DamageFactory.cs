using System;
using System.Linq;
using Zerds.Buffs;
using Zerds.Constants;
using Zerds.Entities;
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
            // Exposure Skill
            if (zerdCreator != null && target.Buffs.Any(b => b is BurnBuff) && zerdCreator.SkillPoints(SkillType.Exposure) > 0)
                damageInstance.Damage *= zerdCreator.SkillValue(SkillType.Exposure, true);

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