using System;
using Microsoft.Xna.Framework;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public class BlazingSpeedBuff : Buff
    {
        public BlazingSpeedBuff(Being being, TimeSpan length, float speedIncrease) : base(null, being, length, true, movementSpeedFactor: speedIncrease)
        {
            Texture = TextureCacheFactory.Get("Buffs/burn.png");
            Animation = new Animation("");
            Animation.AddFrame(Texture.Bounds, TimeSpan.FromSeconds(0.15));
        }
        
        public override void Draw()
        {
            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: Animation.CurrentRectangle,
                color: new Color(Color.White, 0.2f),
                destinationRectangle: new Rectangle((int)Being.X, (int)Being.Y, (int)Being.Width, (int)Being.Height),
                origin: new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f));
        }
    }
}
