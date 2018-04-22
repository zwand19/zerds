using System;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public class BlazingSpeedBuff : Buff
    {
        public BlazingSpeedBuff(Zerd zerd, TimeSpan length, float speedIncrease) : base(null, zerd, length, true, movementSpeedFactor: speedIncrease)
        {
            Applier = zerd;
            DamageType = DamageTypes.Fire;
            Texture = TextureCacheFactory.Get("Buffs/burn.png");
            Animation = new Animation("");
            Animation.AddFrame(Texture.Bounds, TimeSpan.FromSeconds(0.15));
            if (zerd.SkillPoints(SkillType.BlazingSpeed) > 0)
                DamagePerSecond = zerd.MaxHealth * PlayerSkills.BleedFireHealthPercent / (100 * (float)length.TotalSeconds);
        }
        
        public override void Draw()
        {
            Texture.Draw(
                sourceRectangle: Animation.CurrentRectangle,
                color: new Color(Color.White, 0.2f),
                destinationRectangle: new Rectangle((int) Being.X, (int) Being.Y, (int) Being.Width, (int) Being.Height),
                origin: new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f));
        }
    }
}
