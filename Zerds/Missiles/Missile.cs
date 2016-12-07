using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Zerds.Entities;
using Zerds.Factories;

namespace Zerds.GameObjects
{
    public abstract class Missile : Entity
    {
        public DamageInstance Damage { get; set; }
        public Point Origin { get; set; }
        public float Distance { get; set; }
        public Being Creator { get; set; }
        public abstract void OnHit(Being target);

        public Missile()
        {
            IsActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Creator is Zerd)
            {
                foreach (var enemy in Globals.GameState.Enemies)
                {
                    foreach (var hitbox in enemy.Hitbox())
                    {
                        foreach (var hitbox2 in Hitbox())
                        {
                            if (hitbox.Intersects(hitbox2))
                            {
                                OnHit(enemy);
                                return;
                            }
                        }
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
