using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Zerds.Entities;
using Zerds.Graphics;

namespace Zerds.GameObjects.Buffs
{
    public abstract class Buff
    {
        public TimeSpan Length { get; set; }
        public float MovementSpeedFactor { get; set; }
        public float CooldownReductionFactor { get; set; }
        public float HealthRegenFactor { get; set; }
        public bool IsPositive { get; set; }
        public bool IsStunned { get; set; }
        public Animation Animation { get; set; }
        public TimeSpan TimeRemaining { get; set; }
        public Being Being { get; set; }
        public Texture2D Texture { get; set; }
        public bool GrantsInvulnerability { get; internal set; }

        public abstract void Draw();

        public Buff(Being being, TimeSpan length, bool isPositive, float movementSpeedFactor = 0, float cooldownReductionFactor = 0, float healthRegenFactor = 0)
        {
            Length = length;
            MovementSpeedFactor = movementSpeedFactor;
            CooldownReductionFactor = cooldownReductionFactor;
            HealthRegenFactor = healthRegenFactor;
            IsPositive = isPositive;
            TimeRemaining = length;
            Being = being;
        }

        public void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);
        }

        public void StandardDraw()
        {
            if (Texture == null)
                return;

            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: Animation.CurrentRectangle,
                color: Color.White,
                position: new Vector2(Being.X, Being.Y),
                origin: new Vector2(Being.Texture.Width / 2f, Being.Texture.Height / 2f));
        }
    }
}
