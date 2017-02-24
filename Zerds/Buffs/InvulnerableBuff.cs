using System;
using Zerds.Entities;

namespace Zerds.Buffs
{
    public class InvulnerableBuff : Buff
    {
        public InvulnerableBuff(Being being, TimeSpan length) : base(null, being, length, true)
        {
            GrantsInvulnerability = true;
        }

        public override void Draw()
        {
        }
    }
}
