using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public class CharmBuff : Buff
    {
        public CharmBuff(Being creator, Being being) : base(creator, being, TimeSpan.MaxValue, true, damagePerSecond: AbilityConstants.CharmDegeneration)
        {
            Texture = TextureCacheFactory.Get("Buffs/charm.png");
            Animation = new Animation("");
            Animation.AddFrame(Texture.Bounds, TimeSpan.FromSeconds(0.15));
        }
    }
}
