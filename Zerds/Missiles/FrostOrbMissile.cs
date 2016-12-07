using System;
using Microsoft.Xna.Framework;
using Zerds.GameObjects;
using Zerds.Entities;
using Zerds.Graphics;
using Zerds.Enums;
using Zerds.Constants;
using System.Collections.Generic;
using Zerds.Factories;
using Zerds.GameObjects.Buffs;
using System.Linq;

namespace Zerds.Missiles
{
    public class FrostOrbMissile : Missile
    {
        public FrostOrbMissile(Zerd zerd, DamageInstance damageInstance, Point p)
        {
            Damage = damageInstance;
            Width = 64;
            Height = 64;
            X = p.X;
            Y = p.Y;
            Creator = zerd;
            Origin = p;
            Distance = AbilityConstants.FrostOrbDistance;
            Speed = AbilityConstants.FrostOrbSpeed;
            Velocity = Creator.Facing.Normalized();

            Animations = new AnimationList();
            var moveAnimation = new Animation(AnimationTypes.Move);
            moveAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 1, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 2, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 3, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 4, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 5, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 6, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(64 * 7, 0, 64, 64), TimeSpan.FromSeconds(0.1));
            Animations.Add(moveAnimation);
        }

        public override Animation GetCurrentAnimation()
        {
            return Animations.Get(AnimationTypes.Move);
        }

        public override void Update(GameTime gameTime)
        {
            if (Origin.DistanceBetween(Position) > Distance)
                IsActive = false;
            base.Update(gameTime);
        }

        public override void Draw()
        {
            var rect = GetCurrentAnimation().CurrentRectangle;
            var angle = -(float)Math.Atan2(Velocity.Y, Velocity.X) + SpriteRotation();
            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: rect,
                color: Color.White,
                position: new Vector2(X, Y),
                rotation: angle,
                origin: new Vector2(rect.Width / 2f, rect.Height / 2f));
            if (Globals.ShowHitboxes)
            {
                Hitbox().ForEach(r => Globals.SpriteDrawer.Draw(Globals.WhiteTexture, r, Color.Blue));
            }
        }

        public override float SpriteRotation()
        {
            return (float)Math.PI;
        }

        public override Tuple<string, bool> GetTextureInfo()
        {
            return new Tuple<string, bool>("Missiles/icicle.png", true);
        }

        public override List<Rectangle> Hitbox()
        {
            var rects = new List<Rectangle> {
                new Rectangle((int)(X - Width * 0.5f), (int)(Y - Width * 0.25f), (int)(Width * 0.5f), (int)(Width * 0.5f)),
                new Rectangle((int)X, (int)(Y - Width * 0.25f), (int)(Width * 0.5f), (int)(Width * 0.5f))
            };
            var angle = -(float)Math.Atan2(Velocity.Y, Velocity.X) + SpriteRotation();
            return rects.Select(r => r.RotateAround(Position, angle)).ToList();
        }

        public override void OnHit(Being target)
        {
            Damage.DamageBeing(target);
            IsActive = false;
            target.AddBuff(new ColdBuff(target, TimeSpan.FromMilliseconds(AbilityConstants.FrostOrbColdLength)));
        }
    }
}
