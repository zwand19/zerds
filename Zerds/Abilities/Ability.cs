using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;

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
        public float Range { get; set; }
        public Texture2D Texture { get; set; }

        protected Ability(AbilityTypes type, Being being, TimeSpan cooldown, float manaCost, string iconFile)
        {
            Type = type;
            Being = being;
            FullCooldown = cooldown;
            ManaCost = manaCost;
            Texture = iconFile == "" ? null : TextureCacheFactory.GetOnce($"Icons/{iconFile}");
        }

        public void BasicMissileCast()
        {
            if (Cooldown > TimeSpan.Zero) return;
            if (Being.GetCurrentAnimation().Name != AnimationTypes.Move && Being.GetCurrentAnimation().Name != AnimationTypes.Stand) return;
            Active = true;
        }

        public virtual void Cast()
        {
            Being.Animations.ResetAll();
            if (Cooldown <= TimeSpan.Zero) Execute();
        }

        protected virtual bool Execute()
        {
            Being.Mana -= ManaCost;
            if (Being.Mana < 0)
                Being.Mana = 0;
            Cooldown = TimeSpan.FromSeconds(FullCooldown.TotalSeconds);
            return true;
        }

        public void Update(GameTime gameTime)
        {
            Cooldown = Cooldown.SubtractWithGameSpeed(gameTime.ElapsedGameTime);
            if (Cooldown < TimeSpan.Zero)
                Cooldown = TimeSpan.Zero;
        }

        public virtual void Cancel()
        {
            Active = false;
        }
    }
}
