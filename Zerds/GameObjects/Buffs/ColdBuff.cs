using Microsoft.Xna.Framework;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.Graphics;

namespace Zerds.GameObjects.Buffs
{
    public class ColdBuff : Buff
    {
        private const int TextureSize = 128;

        public ColdBuff(Being being, TimeSpan length) : base(being, length, true, movementSpeedFactor: AbilityConstants.ColdSpeedFactor)
        {
            Texture = TextureCacheFactory.Get("Buffs/cold.png");
            Animation = new Animation("sprint");
            Animation.AddFrame(new Rectangle(0, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.15));
        }

        public override void Draw()
        {
            if (Texture == null)
                return;
            
            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: Animation.CurrentRectangle,
                color: Color.White,
                destinationRectangle: new Rectangle((int)Being.X, (int)Being.Y, (int)Being.Width, (int)Being.Height),
                origin: new Vector2(TextureSize / 2, TextureSize / 2 - 25));
        }
    }
}
