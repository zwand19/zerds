using System;
using Microsoft.Xna.Framework;
using Zerds.Entities;
using Zerds.Graphics;
using Zerds.Enums;
using Zerds.Constants;
using System.Collections.Generic;
using Zerds.Buffs;
using Zerds.Entities.Enemies;

namespace Zerds.Missiles
{
    public class CharmMissile : Missile
    {
        public CharmMissile(Being being, Point p) : base("Missiles/charm.png")
        {
            Width = 44;
            Height = 44;
            X = p.X;
            Y = p.Y;
            Origin = p;
            Creator = being;
            Distance = AbilityConstants.CharmDistance;
            Speed = AbilityConstants.CharmSpeed;
            Velocity = Creator.Facing.Normalized();
            
            Animations = new AnimationList();
            var moveAnimation = new Animation(AnimationTypes.Move);
            moveAnimation.AddFrame(new Rectangle(32 * 0, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 2, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 1, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 2, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 4, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 3, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            moveAnimation.AddFrame(new Rectangle(32 * 4, 0, 32, 32), TimeSpan.FromSeconds(0.15));
            Animations.Add(moveAnimation);
        }

        public override void Update(GameTime gameTime)
        {
            if (Origin.DistanceBetween(Position) > Distance && IsAlive)
            {
                IsActive = false;
            }
            base.Update(gameTime);
        }

        public override float SpriteRotation()
        {
            return (float)Math.PI / 2;
        }

        public override List<Rectangle> Hitbox()
        {
            return new List<Rectangle> {
                new Rectangle((int)(X - Width * 0.6f), (int)(Y - Width * 0.6f), (int)(Width * 1.2f), (int)(Width * 1.2f))
            };
        }

        public override void OnHit(Being target)
        {
            if (target is SkeletonKing) return;
            target.Buffs.Add(new CharmBuff(Creator, target));
            Globals.GameState.Enemies.Remove(target as Enemy);
            Globals.GameState.Allies.Add(target as Enemy);
            IsActive = false;
        }
    }
}
