using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Zerds.Constants;
using Zerds.GameObjects;
using Zerds.Buffs;
using Zerds.Enums;

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
        public float CriticalChance { get; set; }
        public Knockback Knockback { get; set; }
        public List<Buff> Buffs { get; set; }
        public float HitboxSize = 1f;
        public Vector2 CastPoint { get; set; }

        public bool IsAlive => Health > 0;
        public bool Stunned => Buffs.Any(b => b.IsStunned);
        public bool Invulnerable => Buffs.Any(b => b.GrantsInvulnerability);
        public float HealthPercentage => Health / MaxHealth;
        public float ManaPercentage => Mana / MaxMana;
        public Color HealthColor => HealthPercentage > .7f ? new Color(0.1f, 0.54f, 0.08f) : HealthPercentage > .25f ? new Color(1.0f, 0.67f, 0f) : new Color(0.82f, 0.1f, 0.1f);

        protected Being(string file, bool cache) : base(file, cache)
        {
            Buffs = new List<Buff>();
            IsActive = true;
        }

        public void Initialize(float health, float mana, float healthRegen, float manaRegen, float speed, float critChance)
        {
            Health = health;
            MaxHealth = health;
            Mana = mana;
            MaxMana = mana;
            HealthRegen = healthRegen;
            ManaRegen = manaRegen;
            BaseSpeed = speed;
            CriticalChance = critChance;
        }

        public bool IsCritical(DamageTypes type)
        {
            return new Random().NextDouble() < CriticalChance;
        }

        public override void Draw()
        {
            if (IsAlive) Buffs.ForEach(b => b.Draw());
            var angle = -(float)Math.Atan2(Facing.Y, Facing.X) + SpriteRotation();
            var rect = GetCurrentAnimation().CurrentRectangle;
            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: rect,
                color: Color.White,
                position: new Vector2(X, Y),
                rotation: angle,
                origin: new Vector2(rect.Width / 2f, rect.Height / 2f));
            if (Globals.ShowHitboxes && IsAlive)
            {
                Hitbox().ForEach(r => Globals.SpriteDrawer.Draw(Globals.WhiteTexture, r, Color.Green));
            }
        }

        public override List<Rectangle> Hitbox()
        {
            var halfSize = Width * HitboxSize / 2;
            var rect = Invulnerable ? Rectangle.Empty : new Rectangle((int)(X - halfSize), (int)(Y - halfSize), (int)(halfSize * 2), (int)(halfSize * 2));
            return new List<Rectangle> { rect };
        }

        public virtual void DrawHealthbar()
        {
            if (!IsAlive) return;
            var height = MathHelper.Clamp((int)(Texture.Width * 0.1f), DisplayConstants.MinHealthBarHeight, DisplayConstants.MaxHealthBarHeight);
            var bord = DisplayConstants.HealthBarBorder;
            var left = (int)(X - Width * 0.25f);
            var top = (int)(Y - Height * 0.5f - height - 5f);
            Globals.SpriteDrawer.Draw(Globals.WhiteTexture, new Rectangle(left, top, (int)(Width * 0.5f), height), Color.Black);
            Globals.SpriteDrawer.Draw(Globals.WhiteTexture, new Rectangle(left + bord, top + bord, (int)(((Width * 0.5f) - bord * 2) * HealthPercentage), height - bord * 2), HealthColor);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsAlive)
            {
                Velocity = Vector2.Zero;
            }
            else
            {
                Speed = BaseSpeed;
                
                Buffs.ForEach(b =>
                {
                    Speed += b.MovementSpeedFactor;
                    Health -= b.DamagePerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    b.TimeRemaining -= gameTime.ElapsedGameTime;
                });
                Buffs = Buffs.Where(b => b.TimeRemaining > TimeSpan.Zero).ToList();

                Health += HealthRegen * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Mana += ManaRegen * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Knockback == null)
                {
                    var angle = Velocity.AngleBetween(Facing);
                    Speed *= angle < GameplayConstants.ZerdFrontFacingAngle ? 1 : angle > 180 - GameplayConstants.ZerdFrontFacingAngle ? GameplayConstants.BackpedalFactor : GameplayConstants.SideStepFactor;
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

                if (this is Zerd)
                {
                    X = MathHelper.Clamp(X, 32, Globals.ViewportBounds.Width - 32);
                    Y = MathHelper.Clamp(Y, 32, Globals.ViewportBounds.Height - 32);
                }
                Health = MathHelper.Clamp(Health, 0, MaxHealth);
                Mana = MathHelper.Clamp(Mana, 0, MaxMana);
            }

            GetCurrentAnimation().Update(gameTime);
            Buffs.ForEach(b => b.Update(gameTime));
        }
    }
}
