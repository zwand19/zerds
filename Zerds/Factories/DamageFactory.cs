using System;
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
            target.Health -= damageInstance.Damage;
            if (damageInstance.Knockback != null)
                target.Knockback = new Knockback((target.PositionVector - damageInstance.Creator.PositionVector).Normalized(),
                    damageInstance.Knockback.MaxDuration, damageInstance.Knockback.Speed);
            if (target is Zerd)
                ControllerService.Controllers[((Zerd) target).PlayerIndex].VibrateController(TimeSpan.FromMilliseconds(250), 1f);
            AddText(new DamageText(damageInstance, target));
        }
    }
}
