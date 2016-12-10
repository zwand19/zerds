using Microsoft.Xna.Framework;
using System;
using Zerds.Entities;
using Zerds.Enums;

namespace Zerds.Abilities
{
    public abstract class Ability
    {
        public Being Being { get; set; }
        public TimeSpan Cooldown { get; set; }
        public TimeSpan FullCooldown { get; set; }
        public AbilityTypes Type { get; set; }
        public Func<bool> Callback { get; set; }
        public bool Active { get; set; }
        public float ManaCost { get; set; }

        protected Ability(AbilityTypes type, Being being, TimeSpan cooldown, float manaCost)
        {
            Type = type;
            Being = being;
            FullCooldown = cooldown;
            ManaCost = manaCost;
        }

        public bool BasicMissileCast()
        {
            if (Cooldown > TimeSpan.Zero) return false;
            if (Being.GetCurrentAnimation().Name != AnimationTypes.Move && Being.GetCurrentAnimation().Name != AnimationTypes.Stand) return false;
            return Active = true;
        }

        public virtual bool Cast()
        {
            return Cooldown <= TimeSpan.Zero && Execute();
        }

        protected virtual bool Execute()
        {
            Cooldown = TimeSpan.FromSeconds(FullCooldown.TotalSeconds);
            return true;
        }

        public void Update(GameTime gameTime)
        {
            Cooldown -= gameTime.ElapsedGameTime;
            if (Cooldown < TimeSpan.Zero)
                Cooldown = TimeSpan.Zero;
        }

        public virtual void Cancel()
        {
            Active = false;
        }
    }
}
