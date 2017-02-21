using System;
using Zerds.Constants;
using Zerds.Entities;

namespace Zerds.Buffs
{
    public class RageBuff : Buff
    {
        public RageBuff(Being being, TimeSpan length) : base(null, being, length, true)
        {
            CritChanceFactor = being.SkillValue(SkillType.Rage, false) / 100;
        }

        public override void Draw()
        {
        }
    }
}
