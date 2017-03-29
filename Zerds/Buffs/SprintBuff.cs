using Microsoft.Xna.Framework;
using System;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public class SprintBuff : Buff
    {
        private const int TextureSize = 36;

        public SprintBuff(Being being, float bonus) : base(null, being, TimeSpan.MaxValue, true, movementSpeedFactor: bonus)
        {
            Texture = TextureCacheFactory.Get("Buffs/dash.png");
            Animation = new Animation("sprint");    
            Animation.AddFrame(new Rectangle(0, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.15));
            Animation.AddFrame(new Rectangle(TextureSize, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.15));
            Animation.AddFrame(new Rectangle(TextureSize * 2, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.15));
            Animation.AddFrame(new Rectangle(TextureSize * 3, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.15));
        }

        public override void Draw()
        {
            if (Texture == null || Being.Velocity == Vector2.Zero)
                return;

            var angle = -(float)Math.Atan2(Being.Facing.Y, Being.Facing.X) + (float)Math.PI / 2f;

            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: Animation.CurrentRectangle,
                color: Color.White,
                position: new Vector2(Being.X, Being.Y),
                rotation: angle,
                origin: new Vector2(TextureSize / 2, TextureSize / 2 - 25));
        }
    }
}
