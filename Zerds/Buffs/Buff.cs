using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Zerds.Entities;
using Zerds.Enums;
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
        public float CritChanceFactor { get; set; }
        public bool IsPositive { get; set; }
        public bool IsStunned { get; set; }
        public Animation Animation { get; set; }
        public TimeSpan TimeRemaining { get; set; }
        public Being Being { get; set; }
        public Texture2D Texture { get; set; }
        public bool GrantsInvulnerability { get; internal set; }
        public Being Applier { get; set; }
        public bool Frozen { get; set; }
        public AbilityTypes AbilityType { get; set; }
        public DamageTypes DamageType { get; set; }

        protected Buff(Being applier, Being being, TimeSpan length, bool isPositive, float movementSpeedFactor = 0, float cooldownReductionFactor = 0, float healthRegenFactor = 0, float damagePerSecond = 0,
            bool frozen = false)
        {
            Length = length;
            MovementSpeedFactor = movementSpeedFactor;
            CooldownReductionFactor = cooldownReductionFactor;
            HealthRegenFactor = healthRegenFactor;
            DamagePerSecond = damagePerSecond;
            IsPositive = isPositive;
            TimeRemaining = length;
            Being = being;
            Applier = applier;
            Frozen = frozen;
        }

        public virtual void Draw()
        {
            Texture?.Draw(
                sourceRectangle: Animation.CurrentRectangle,
                color: Color.White,
                destinationRectangle: new Rectangle((int) Being.X, (int) Being.Y, (int) Being.Width, (int) Being.Height),
                origin: new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f));
        }

        public void Update(GameTime gameTime)
        {
            Animation?.Update(gameTime);
        }
    }
}
