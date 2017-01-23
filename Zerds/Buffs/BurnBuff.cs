using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public class BurnBuff : Buff
    {
        public BurnBuff(Being creator, Being being, TimeSpan length, float burnDamage) : base(creator, being, length, true, damagePerSecond: burnDamage)
        {
            Texture = TextureCacheFactory.Get("Buffs/burn.png");
            Animation = new Animation("");
            Animation.AddFrame(Texture.Bounds, TimeSpan.FromSeconds(0.15));
            var zerd = creator as Zerd;
            if (zerd == null) return;
            DamagePerSecond *= zerd.SkillValue(SkillType.DeepBurn, true);
            Length = TimeSpan.FromMilliseconds(TimeRemaining.TotalMilliseconds * zerd.SkillValue(SkillType.DeepBurn, true));
            TimeRemaining = Length;
        }
    }
}
