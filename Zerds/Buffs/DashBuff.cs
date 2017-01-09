using Microsoft.Xna.Framework;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public class DashBuff : Buff
    {
        private const int TextureSize = 64;
        private Vector2 _initialPosition;

        public DashBuff(Being being, float factor) : base(null, being, AbilityConstants.DashLength, true, movementSpeedFactor: factor)
        {
            Texture = being.Texture;
            IsStunned = true;
            Animation = new Animation("sprint");
            Animation.AddFrame(new Rectangle(TextureSize * 8, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.3));
            _initialPosition = new Vector2(being.X, being.Y);
        }

        public override void Draw()
        {
            if (Texture == null)
                return;

            var angle = -(float)Math.Atan2(Being.Facing.Y, Being.Facing.X) + (float)Math.PI / 2f;
            
            drawSprite(angle, 12.5f, 0.38f);
            drawSprite(angle, 25f, 0.29f);
            drawSprite(angle, 37.5f, 0.2f);
            drawSprite(angle, 50f, 0.1f);
        }

        private void drawSprite(float angle, float offset, float alpha)
        {
            if (_initialPosition.DistanceBetween(Being.PositionVector) < offset)
                return;

            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: Animation.CurrentRectangle,
                color: Color.White * alpha,
                position: new Vector2(Being.X, Being.Y),
                rotation: angle,
                origin: new Vector2(TextureSize / 2, TextureSize / 2 - offset));
        }
    }
}
