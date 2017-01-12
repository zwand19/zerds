﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;

namespace Zerds.GameObjects
{
    public class DamageText
    {
        public DamageInstance DamageInstance { get; set; }
        public Vector2 Position { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsActive => Duration > TimeSpan.Zero;
        private readonly Color _color;

        public DamageText(DamageInstance damageInstance, Being target)
        {
            DamageInstance = damageInstance;
            Position = new Vector2(target.X, target.Y - target.Height / 2 - 12);
            Duration = DisplayConstants.DamageTextDuration;
            switch (damageInstance.DamageType)
            {
                case Enums.DamageTypes.Fire:
                    _color = new Color(236, 112, 64);
                    break;
                case Enums.DamageTypes.Frost:
                    _color = new Color(64, 192, 236);
                    break;
                case Enums.DamageTypes.Lightning:
                    _color = new Color(125, 249, 255);
                    break;
                case Enums.DamageTypes.Magic:
                    _color = new Color(256, 246, 54);
                    break;
                default:
                    _color = new Color(220, 220, 220);
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            Duration = Duration.SubtractWithGameSpeed(gameTime.ElapsedGameTime);
            Position = new Vector2(Position.X, Position.Y - DisplayConstants.DamageTextSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed);
        }

        public void Draw()
        {
            Globals.SpriteDrawer.DrawText(((int) DamageInstance.Damage).ToString(), Position, DamageInstance.IsCritical ? 23f : 17f, color: _color);
        }
    }
}
