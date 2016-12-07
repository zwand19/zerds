using System;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.GameObjects.Buffs;

namespace Zerds.Factories
{
    public static class BuffFactory
    {
        public static void AddBuff(this Being being, BuffTypes type)
        {
            switch (type)
            {
                case BuffTypes.Dash:
                    being.Buffs.Add(new DashBuff(being));
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
