using System;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public class BurnBuff : Buff
    {
        public BurnBuff(Being being, TimeSpan length, float burnDamage) : base(being, length, true, damagePerSecond: burnDamage)
        {
            Texture = TextureCacheFactory.Get("Buffs/burn.png");
            Animation = new Animation("");
            Animation.AddFrame(Texture.Bounds, TimeSpan.FromSeconds(0.15));
        }
    }
}
