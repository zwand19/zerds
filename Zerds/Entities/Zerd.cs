using Zerds.Graphics;
using System;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Enums;
using System.Collections.Generic;
using Zerds.Abilities;
using System.Linq;

namespace Zerds.Entities
{
    public class Zerd : Being
    {
        public string Name { get; set; }
        public List<Ability> Abilities { get; set; }
        public PlayerIndex PlayerIndex { get; private set; }
        

        public Zerd(PlayerIndex playerIndex, string zerdFile) : base(zerdFile, false)
        {
            PlayerIndex = playerIndex;
            X = 650;
            Y = 300;
            Health = GameplayConstants.ZerdStartingHealth;
            MaxHealth = Health;
            Mana = GameplayConstants.ZerdStartingMana;
            MaxMana = Mana;
            HealthRegen = GameplayConstants.ZerdStartingHealthRegen;
            ManaRegen = GameplayConstants.ZerdStartingManaRegen;
            Width = 64;
            Height = 64;
            HitboxSize = 0.7f;
            BaseSpeed = GameplayConstants.MaxZerdSpeed;
            CriticalChance = GameplayConstants.ZerdCritChance;

            Animations = new AnimationList();
            Abilities = new List<Ability>
            {
                new Dash(this),
                new Fireball(this),
                new Wand(this),
                new Iceball(this)
            };

            var attackedAnimation = new Animation(AnimationTypes.Damaged);
            attackedAnimation.AddFrame(new Rectangle(64 * 10, 0, 64, 64), TimeSpan.FromSeconds(0.25));
            Animations.Add(attackedAnimation);
            var standAnimation = new Animation(AnimationTypes.Stand);
            standAnimation.AddFrame(new Rectangle(64 * 11, 0, 64, 64), TimeSpan.FromSeconds(0.3));
            standAnimation.AddFrame(new Rectangle(64 * 12, 0, 64, 64), TimeSpan.FromSeconds(0.3));
            Animations.Add(standAnimation);
            var walkAnimation = new Animation(AnimationTypes.Move);
            walkAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), TimeSpan.FromSeconds(0.3));
            walkAnimation.AddFrame(new Rectangle(64 * 12, 0, 64, 64), TimeSpan.FromSeconds(0.3));
            Animations.Add(walkAnimation);
        }

        public override Animation GetCurrentAnimation()
        {
            if (Knockback != null)
                return Animations.Get(AnimationTypes.Damaged);
            if (Abilities.First(a => a.Type == AbilityTypes.Wand).Active)
                return Animations.Get(AnimationTypes.Attack);
            if (Abilities.First(a => a.Type == AbilityTypes.Iceball).Active)
                return Animations.Get(AnimationTypes.FrostAttack);
            if (Abilities.First(a => a.Type == AbilityTypes.Fireball).Active)
                return Animations.Get(AnimationTypes.FireAttack);
            if (Velocity.Length() > Vector2.Zero.Length())
                return Animations.Get(AnimationTypes.Move);
            return Animations.Get(AnimationTypes.Stand);
        }

        public override bool IsCritical(DamageTypes type)
        {
            var chance = CriticalChance;
            if (type == DamageTypes.Fire)
                chance += Helpers.GetPlayer(this).Skills.Devastation * SkillConstants.DevastationStat / 100;
            return new Random().NextDouble() < chance;
        }

        public void ControllerUpdate(float leftTrigger, float rightTrigger, Vector2 leftStickDirection, Vector2 rightStickDirection)
        {
            if (Stunned)
                return;

            if (Math.Abs(leftStickDirection.Length()) > CodingConstants.JoystickTolerance)
            {
                Facing = leftStickDirection;
            }
            if (leftTrigger > CodingConstants.TriggerPress)
            {
                Facing = Facing.Rotate(180);
            }
            Velocity = leftStickDirection;
        }

        public override void Update(GameTime gameTime)
        {
            if (Knockback != null)
            {
                Abilities.ForEach(a => a.Cancel());
            }
            Abilities.ForEach(a => a.Update(gameTime));
            base.Update(gameTime);
        }

        
        public override float SpriteRotation()
        {
            return 1f * (float)Math.PI / 2f;
        }
    }
}
