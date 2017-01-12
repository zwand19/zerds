using System;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public class FrozenBuff : Buff
    {
        public FrozenBuff(Being being, TimeSpan length) : base(null, being, length, true, frozen: true)
        {
            Texture = TextureCacheFactory.Get("Buffs/cold.png");
            Animation = new Animation("");
            Animation.AddFrame(Texture.Bounds, TimeSpan.FromSeconds(0.15));
        }
    }
}
