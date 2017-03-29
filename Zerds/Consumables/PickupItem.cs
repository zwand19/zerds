using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;

namespace Zerds.Consumables
{
    public abstract class PickupItem : Entity
    {
        public TimeSpan Duration { get; set; }

        public abstract void OnPickup(Zerd zerd);

        protected PickupItem(string file, Enemy dropper,  bool cache = true) : base(file, cache)
        {
            X = dropper.X;
            Y = dropper.Y;
            Velocity = new Vector2(1, 0).Rotate(Globals.Random.Next(360));
            Speed = GameplayConstants.PickupItemSpeed;
            Duration = TimeSpan.Zero;
            if (Math.Abs(Width) < CodingConstants.Tolerance) Width = GameplayConstants.DefaultItemSize;
            if (Math.Abs(Height) < CodingConstants.Tolerance) Height = GameplayConstants.DefaultItemSize;
        }

        public override void Update(GameTime gameTime)
        {
            Duration = Duration.AddWithGameSpeed(gameTime.ElapsedGameTime);
            Speed -= GameplayConstants.PickupItemSpeedDecay * (float) gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;
            if (Duration > GameplayConstants.PickupItemLength)
                IsActive = false;
            if (Speed <= 20f) // cant pick items up for a short bit after dropping
            {
                Globals.GameState.Zerds.ForEach(z =>
                {
                    if (this.Intersects(z))
                        OnPickup(z);
                });
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {
            Globals.SpriteDrawer.Draw(
                texture: Texture,
                sourceRectangle: GetCurrentAnimation().CurrentRectangle,
                color: Color.White,
                destinationRectangle: Helpers.CreateRect(X, Y, Width, Height),
                origin: new Vector2(Width / 2f, Height / 2f));
            if (Globals.ShowHitboxes)
            {
                Hitbox().ForEach(r => Globals.SpriteDrawer.Draw(Globals.WhiteTexture, r, Color.Pink));
            }
        }

        public override List<Rectangle> Hitbox()
        {
            return new List<Rectangle> {this.BasicHitbox()};
        }
    }
}
