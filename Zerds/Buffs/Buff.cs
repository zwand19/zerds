using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Zerds.Entities;
using Zerds.Graphics;

namespace Zerds.Buffs
{
    public abstract class Buff
    {
        public TimeSpan Length { get; set; }
        public float MovementSpeedFactor { get; set; }
        public float CooldownReductionFactor { get; set; }
        public float HealthRegenFactor { get; set; }
        public float DamagePerSecond { get; set; }
        public bool IsPositive { get; set; }
        public bool IsStunned { get; set; }
        public Animation Animation { get; set; }
        public TimeSpan TimeRemaining { get; set; }
        public Being Being { get; set; }
        public Texture2D Texture { get; set; }
        public bool GrantsInvulnerability { get; internal set; }

        protected Buff(Being being, TimeSpan length, bool isPositive, float movementSpeedFactor = 0, float cooldownReductionFactor = 0, float healthRegenFactor = 0, float damagePerSecond = 0)
        {
            Length = length;
            MovementSpeedFactor = movementSpeedFactor;
            CooldownReductionFactor = cooldownReductionFactor;
            HealthRegenFactor = healthRegenFactor;
            DamagePerSecond = damagePerSecond;
            IsPositive = isPositive;
            TimeRemaining = length;
            Being = being;
        }

        public virtual void Draw()
        {
            if (Texture == null)
                return;

            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: Animation.CurrentRectangle,
                color: Color.White,
                destinationRectangle: new Rectangle((int)Being.X, (int)Being.Y, (int)Being.Width, (int)Being.Height),
                origin: new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f));
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
