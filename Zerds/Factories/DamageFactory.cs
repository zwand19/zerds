using Zerds.Entities;
using Zerds.GameObjects;
using Zerds.Services;

namespace Zerds.Factories
{
    public static class DamageFactory
    {
        public static void DamageBeing(this DamageInstance damageInstance, Being target)
        {
            target.Health -= damageInstance.Damage;
            target.Knockback = new Knockback((target.PositionVector - damageInstance.Creator.PositionVector).Normalized(), new System.TimeSpan(0, 0, 1), 250);
            if (target is Zerd)
                ControllerService.GetService(((Zerd)target).PlayerIndex).VibrateController(new System.TimeSpan(0, 0, 0, 0, 250), 1f);
        }
    }
}
