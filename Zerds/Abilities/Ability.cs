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
        public bool DrawIcon { get; set; }
        public float ManaCost { get; set; }
        public float Range { get; set; }
        public Texture2D Texture { get; set; }
        public Texture2D DisabledTexture { get; set; }

        protected Ability(AbilityTypes type, Being being, TimeSpan cooldown, float manaCost, string iconFile)
        {
            Type = type;
            Being = being;
            FullCooldown = cooldown;
            ManaCost = manaCost;
            DrawIcon = iconFile != "";
            Texture = DrawIcon ? TextureCacheFactory.GetOnce($"Icons/{iconFile}.png") : null;
            DisabledTexture = DrawIcon ? TextureCacheFactory.GetOnce($"Icons/{iconFile}-grey.png") : null;
        }

        public void BasicMissileCast(string animation)
        {
            if (Cooldown > TimeSpan.Zero || Being.Mana < ManaCost) return;
            if (Being.GetCurrentAnimation().Name != AnimationTypes.Move && Being.GetCurrentAnimation().Name != AnimationTypes.Stand) return;
            (Being as Zerd).ZerdAnimations.Reset(animation);
            Active = true;
        }

        public virtual void Cast()
        {
            if (Cooldown > TimeSpan.Zero) return;
            Being.Animations.ResetAll();
            Execute();
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

        public Texture2D IconTexture()
        {
            return Cooldown > TimeSpan.Zero || Being.Mana < ManaCost ? DisabledTexture : Texture;
        }
    }
}
