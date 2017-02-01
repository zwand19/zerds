using System;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public class IceShieldBuff : Buff
    {
        public IceShieldBuff(Being being, TimeSpan length) : base(null, being, length, true)
        {
            Texture = TextureCacheFactory.Get("Buffs/ice-shield.png");
            GrantsInvulnerability = true;
            Animation = new Animation("");
            Animation.AddFrame(Texture.Bounds, TimeSpan.FromSeconds(0.15));
        }
    }
}
