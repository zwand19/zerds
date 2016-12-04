using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Zerds.Constants;
using Zerds.GameObjects;
using Zerds.GameObjects.Buffs;

namespace Zerds.Entities
{
    public abstract class Being : Entity
    {
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public float Mana { get; set; }
        public float MaxMana { get; set; }
        public float HealthRegen { get; set; }
        public float ManaRegen { get; set; }
        public float BaseSpeed { get; set; }
        public Knockback Knockback { get; set; }
        public List<Buff> Buffs { get; set; }
        public float HitboxSize = 1f;
        public Vector2 CastPoint { get; set; }

        public bool IsAlive => Health > 0;
        public bool Stunned => Buffs.Any(b => b.IsStunned);
        public bool Invulnerable => Buffs.Any(b => b.GrantsInvulnerability);
        public double HealthPercentage => Health / (double)MaxHealth;
        public Color HealthColor => HealthPercentage > .7 ? new Color(0.0f, 0.7f, 0.0f) : HealthPercentage > .25 ? new Color(0.3f, 0.7f, 0.0f) : new Color(0.7f, 0.0f, 0.0f);

        public Being()
        {
            Buffs = new List<Buff>();
        }

        public void Initialize(float health, float mana, float healthRegen, float manaRegen, float speed)
        {
            Health = health;
            MaxHealth = health;
            Mana = mana;
            MaxMana = mana;
            HealthRegen = healthRegen;
            ManaRegen = manaRegen;
            BaseSpeed = speed;
        }

        public override void Draw()
        {
            Buffs.ForEach(b => b.Draw());
            var angle = -(float)Math.Atan2(Facing.Y, Facing.X) + SpriteRotation();
            var rect = GetCurrentAnimation().CurrentRectangle;
            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: rect,
                color: Color.White,
                position: new Vector2(X, Y),
                rotation: angle,
                origin: new Vector2(rect.Width / 2f, rect.Height / 2f));
        }

        public override Rectangle Hitbox()
        {
            var inflateSize = Width * (HitboxSize - 1f);
            return Invulnerable ? Rectangle.Empty : new Rectangle((int)(X - Width / 2 + inflateSize / 2), (int)(Y - Width / 2 + inflateSize / 2), (int)(Width + inflateSize), (int)(Height + inflateSize));
        }

        public override void Update(GameTime gameTime)
        {
            Speed = BaseSpeed;

            Buffs.ForEach(b => b.TimeRemaining -= gameTime.ElapsedGameTime);
            Buffs = Buffs.Where(b => b.TimeRemaining > TimeSpan.Zero).ToList();

            if (Knockback == null)
            {
                Buffs.Where(b => b.MovementSpeedFactor > 0).ToList().ForEach(b => Speed += b.MovementSpeedFactor);
                X += Velocity.X * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Y -= Velocity.Y * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                X += (int)(Knockback.Direction.X * Knockback.Speed * gameTime.ElapsedGameTime.TotalSeconds *
                        (float)Math.Pow(Knockback.Duration.TotalMilliseconds / Knockback.MaxDuration.TotalMilliseconds, GameplayConstants.KnockbackDecay));
                Y += (int)(Knockback.Direction.Y * Knockback.Speed * gameTime.ElapsedGameTime.TotalSeconds *
                        (float)Math.Pow(Knockback.Duration.TotalMilliseconds / Knockback.MaxDuration.TotalMilliseconds, GameplayConstants.KnockbackDecay));
                Knockback.Duration -= gameTime.ElapsedGameTime;
                Facing = Knockback.Direction.Rotate(180);
                if (Knockback.Duration < TimeSpan.Zero)
                    Knockback = null;
            }

            GetCurrentAnimation().Update(gameTime);
            Buffs.ForEach(b => b.Update(gameTime));
        }
    }
}
