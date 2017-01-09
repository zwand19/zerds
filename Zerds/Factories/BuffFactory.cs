using System;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Buffs;
using Zerds.Constants;
using Zerds.GameObjects;

namespace Zerds.Factories
{
    public static class BuffFactory
    {
        public static void AddBuff(this Being being, BuffTypes type)
        {
            switch (type)
            {
                case BuffTypes.Dash:
                    being.Buffs.Add(new DashBuff(being, AbilityConstants.DashBonus * (1 + ((Zerd) being).Player.AbilityUpgrades[AbilityUpgradeType.DashDistance] / 100f)));
                    return;
                case BuffTypes.Sprint:
                    being.Buffs.Add(new SprintBuff(being));
                    return;
            }
            throw new Exception("Unhandled buff");
        }
        public static void AddBuff(this Being being, Buff buff)
        {
            being.Buffs.Add(buff);
        }
    }
}
