using System;
using System.Linq;
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
            if (target is Zerd)
                damageInstance.Damage *= 1 - ((Zerd) target).Player.AbilityUpgrades[AbilityUpgradeType.DamageTaken] / 100f;
            target.Health -= damageInstance.Damage;
            if (damageInstance.Knockback != null)
                target.Knockback = new Knockback((target.PositionVector - damageInstance.Creator.PositionVector).Normalized(),
                    damageInstance.Knockback.MaxDuration, damageInstance.Knockback.Speed);
            if (target is Zerd)
                ControllerService.Controllers[((Zerd) target).Player.PlayerIndex].VibrateController(TimeSpan.FromMilliseconds(250), 1f);
            AddText(new DamageText(damageInstance, target));
            if (target.Health < 0 && target.Killer == null)
            {
                target.Killer = damageInstance.Creator;
                Globals.GameState.Players.FirstOrDefault(p => p.Zerd == damageInstance.Creator)?.Zerd.EnemyKilled(target as Enemy);
            }
        }
    }
}
