using Microsoft.Xna.Framework;
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

        public override void Update(GameTime gameTime)
        {
            if (Creator is Zerd)
            {
                foreach (var enemy in Globals.GameState.Enemies)
                {
                    if (Hitbox().Intersects(enemy.Hitbox()))
                    {
                        Damage.DamageBeing(enemy);
                        IsActive = false;
                        return;
                    }
                }
            }
        }
    }
}
