﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Zerds.Constants;
using Zerds.GameObjects;
using Zerds.Buffs;
using Zerds.Enums;
using Zerds.Factories;
using System.Reflection;
using Zerds.Missiles;

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
        public Vector2 CastPoint { get; set; }
        public Being Killer { get; set; }
        public float Opacity { get; set; }

        public bool IsAlive => Health > 0;
        public bool Stunned => Buffs.Any(b => b.IsStunned);
        public bool Invulnerable => Buffs.Any(b => b.GrantsInvulnerability);
        public float HealthPercentage => Health / MaxHealth;
        public float ManaPercentage => Mana / MaxMana;
        public Color HealthColor => HealthPercentage > .7f ? new Color(0.1f, 0.54f, 0.08f) : HealthPercentage > .25f ? new Color(1.0f, 0.67f, 0f) : new Color(0.82f, 0.1f, 0.1f);

        protected Being(string file, bool cache) : base(file, cache)
        {
            Buffs = new List<Buff>();
            Opacity = 1;
        }

        public virtual void LevelEnded()
        {
            
        }

        public virtual bool IsCritical(DamageTypes type, AbilityTypes ability)
        {
            return Globals.Random.NextDouble() < CriticalChance;
        }

        public override void Draw()
        {
            if (IsAlive) Buffs.ForEach(b => b.Draw());
            var angle = -(float)Math.Atan2(Facing.Y, Facing.X) + SpriteRotation();
            var rect = GetCurrentAnimation().CurrentRectangle;
            var scale = Width / rect.Width;
            this.DrawGameObject(
                sourceRectangle: rect,
                color: Color.White * Opacity,
                scale: new Vector2(scale),
                rotation: angle,
                origin: GetCurrentAnimation().CurrentOrigin);
            if (Globals.ShowHitboxes && IsAlive)
            {
                Hitbox().ForEach(r => Globals.WhiteTexture.Draw(r, Color.Green));
            }
        }

        public override List<Rectangle> Hitbox()
        {
            return Buffs.Any(b => b.GrantsInvulnerability) ? new List<Rectangle>() : new List<Rectangle> {this.BasicHitbox()};
        }

        public virtual void DrawHealthbar()
        {
            if (!IsAlive) return;
            var height = MathHelper.Clamp((int)(Texture.Width * 0.1f), DisplayConstants.MinHealthBarHeight, DisplayConstants.MaxHealthBarHeight);
            var bord = DisplayConstants.HealthBarBorder;
            var left = (int)(X - Width * 0.25f);
            var top = (int)(Y - Height * 0.5f - height - 5f);
            Globals.WhiteTexture.Draw(new Rectangle(left, top, (int)(Width * 0.5f), height), Color.Black);
            Globals.WhiteTexture.Draw(new Rectangle(left + bord, top + bord, (int)(((Width * 0.5f) - bord * 2) * HealthPercentage), height - bord * 2), HealthColor);
        }

        public override void Update(GameTime gameTime)
        {
            var origX = X;
            var origY = Y;

            if (!IsAlive)
            {
                Velocity = Vector2.Zero;
                Speed = 0;
                Knockback = null;
            }
            else
            {
                Speed = BaseSpeed;

                if (this is Zerd)
                    Speed *= this.SkillValue(SkillType.Swiftness, true);

                Buffs.ForEach(b =>
                {
                    Speed += b.MovementSpeedFactor;
                    var damage = b.DamagePerSecond * (float) gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;
                    if (damage > 0)
                        new DamageInstance(null, damage, b.DamageType, b.Applier, b.AbilityType, false).DamageBeing(this);
                    b.TimeRemaining = b.TimeRemaining.SubtractWithGameSpeed(gameTime.ElapsedGameTime);
                    if (b is DashBuff && this is Zerd && ((Zerd)this).SkillPoints(SkillType.ColdWinds) > 0)
                    {
                        foreach (var e in this.Enemies().Where(e => e.Buffs.All(b2 => !(b2 is FrozenBuff)) && e.Hitbox().Any(h => Hitbox().Any(h.Intersects))))
                        {
                            e.AddBuff(new FrozenBuff(e, TimeSpan.FromSeconds(((Zerd)this).SkillValue(SkillType.ColdWinds, false))));
                        }
                    }
                });

                if (Buffs.Any(b => b is SprintBuff) && this is Zerd)
                    Speed *= 1 + ((Zerd) this).Player.AbilityUpgrades[AbilityUpgradeType.SprintSpeed] / 100;
                else if (this is Zerd)
                    Speed *= 1 + ((Zerd) this).Player.AbilityUpgrades[AbilityUpgradeType.MovementSpeed] / 100;

                Speed = MathHelper.Clamp(Speed, GameplayConstants.MinSpeed, GameplayConstants.MaxSpeed);

                Buffs.ForEach(b => Speed = b.Frozen ? 0 : Speed);

                Buffs = Buffs.Where(b => b.TimeRemaining > TimeSpan.Zero).ToList();

                Health += HealthRegen * (float)gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;
                if (this is Zerd)
                    Health += HealthRegen * (float) gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed *
                              ((Zerd) this).Player.AbilityUpgrades[AbilityUpgradeType.HealthRegen] / 100;
                Mana += ManaRegen * (float)gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;
                if (this is Zerd)
                    Mana += ManaRegen * (float)gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed *
                              ((Zerd)this).Player.AbilityUpgrades[AbilityUpgradeType.ManaRegen] / 100;

                if (Knockback == null)
                {
                    var angle = Velocity.AngleBetween(Facing);
                    Speed *= angle < GameplayConstants.ZerdFrontFacingAngle ? 1 : angle > 180 - GameplayConstants.ZerdFrontFacingAngle ? GameplayConstants.BackpedalFactor : GameplayConstants.SideStepFactor;
                    if (Velocity.Length() > 0.01)
                    {
                        X += Velocity.X * Speed * (float) gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;
                        Y -= Velocity.Y * Speed * (float) gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;
                    }
                }
                else
                {
                    if (Knockback.Speed > 0 && Knockback.MaxDuration > TimeSpan.Zero)
                    {
                        X += (int) (Knockback.Direction.X * Knockback.Speed * gameTime.ElapsedGameTime.TotalSeconds *
                                    (float) Math.Pow(Knockback.Duration.TotalMilliseconds / Knockback.MaxDuration.TotalMilliseconds, GameplayConstants.KnockbackDecay)) *
                             Globals.GameState.GameSpeed;
                        Y += (int) (Knockback.Direction.Y * Knockback.Speed * gameTime.ElapsedGameTime.TotalSeconds *
                                    (float) Math.Pow(Knockback.Duration.TotalMilliseconds / Knockback.MaxDuration.TotalMilliseconds, GameplayConstants.KnockbackDecay)) *
                             Globals.GameState.GameSpeed;
                    }
                    Knockback.Duration = Knockback.Duration.SubtractWithGameSpeed(gameTime.ElapsedGameTime);
                    if (Knockback.Duration < TimeSpan.Zero)
                        Knockback = null;
                }
                
                Health = MathHelper.Clamp(Health, 0, MaxHealth);
                Mana = MathHelper.Clamp(Mana, 0, MaxMana);
            }
            
            // If we tried to move make sure we aren't hitting anything, else reject the move (only reject one dimension if needed)
            if (X != origX || Y != origY)
            {
                if (Globals.Map.CollidesWithWall(this))
                {
                    var newX = X;
                    var newY = Y;
                    X = origX;
                    // Try just moving the Y
                    if (Globals.Map.CollidesWithWall(this))
                    {
                        X = newX;
                        Y = origY;
                        // Try just moving the X
                        if (Globals.Map.CollidesWithWall(this))
                        {
                            // Can't move either axis
                            X = origX;
                        }
                    } 
                }
            }

            GetCurrentAnimation().Update(gameTime);
            Buffs.ForEach(b => b.Update(gameTime));
        }
    }
}
