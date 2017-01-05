using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public class ColdBuff : Buff
    {
        public ColdBuff(Being being, TimeSpan length, float factor) : base(being, length, true, movementSpeedFactor: factor)
        {
            Texture = TextureCacheFactory.Get("Buffs/cold.png");
            Animation = new Animation("");
            Animation.AddFrame(Texture.Bounds, TimeSpan.FromSeconds(0.15));
        }
    }
}
