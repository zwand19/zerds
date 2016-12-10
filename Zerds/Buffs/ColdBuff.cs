using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public class ColdBuff : Buff
    {
        public ColdBuff(Being being, TimeSpan length) : base(being, length, true, movementSpeedFactor: AbilityConstants.ColdSpeedFactor)
        {
            Texture = TextureCacheFactory.Get("Buffs/cold.png");
            Animation = new Animation("");
            Animation.AddFrame(Texture.Bounds, TimeSpan.FromSeconds(0.15));
        }
    }
}
