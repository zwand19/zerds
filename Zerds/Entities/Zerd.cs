using Zerds.Graphics;
using System;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Factories;
using Zerds.Enums;
using Zerds.Missiles;

namespace Zerds.Entities
{
    public class Zerd : Being
    {
        public string Name { get; set; }
        public float DashCooldown { get; set; }
        public float SprintCooldown { get; set; }
        public float WandDamage { get; set; }
        public float FrostOrbDamage { get; set; }
        public TimeSpan WandCooldown { get; set; }
        public TimeSpan FrostOrbCooldown { get; set; }
        public PlayerIndex PlayerIndex { get; private set; }

        private bool _frosting = false;
        private bool _wanding = false;

        public Zerd(PlayerIndex playerIndex)
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
            WandDamage = 10;
            FrostOrbDamage = 11;
            HitboxSize = 0.7f;
            BaseSpeed = GameplayConstants.MaxZerdSpeed;

            Animations = new AnimationList();
            var attackedAnimation = new Animation(AnimationTypes.Damaged);
            attackedAnimation.AddFrame(new Rectangle(64 * 7, 0, 64, 64), TimeSpan.FromSeconds(0.25));
            Animations.Add(attackedAnimation);
            var standAnimation = new Animation(AnimationTypes.Stand);
            standAnimation.AddFrame(new Rectangle(64 * 8, 0, 64, 64), TimeSpan.FromSeconds(0.3));
            standAnimation.AddFrame(new Rectangle(64 * 9, 0, 64, 64), TimeSpan.FromSeconds(0.3));
            Animations.Add(standAnimation);
            var wandAnimation = new Animation(AnimationTypes.Attack);
            wandAnimation.AddFrame(new Rectangle(64 * 0, 0, 64, 64), AbilityConstants.WandCastTime);
            wandAnimation.AddFrame(new Rectangle(64 * 1, 0, 64, 64), AbilityConstants.WandFollowThroughTime);
            wandAnimation.AddFrame(new Rectangle(64 * 1, 0, 64, 64), TimeSpan.FromSeconds(0.05), WandAttacked);
            Animations.Add(wandAnimation);
            var frostAnimation = new Animation(AnimationTypes.FrostAttack);
            frostAnimation.AddFrame(new Rectangle(64 * 5, 0, 64, 64), AbilityConstants.FrostOrbCastTime);
            frostAnimation.AddFrame(new Rectangle(64 * 6, 0, 64, 64), AbilityConstants.FrostOrbFollowThroughTime);
            frostAnimation.AddFrame(new Rectangle(64 * 6, 0, 64, 64), TimeSpan.FromSeconds(0.05), FrostOrbAttacked);
            Animations.Add(frostAnimation);
        }

        public override Animation GetCurrentAnimation()
        {
            if (Knockback != null)
                return Animations.Get(AnimationTypes.Damaged);
            if (_wanding)
                return Animations.Get(AnimationTypes.Attack);
            if (_frosting)
                return Animations.Get(AnimationTypes.FrostAttack);
            return Animations.Get(AnimationTypes.Stand);
        }

        public void ControllerUpdate(float leftTrigger, float rightTrigger, Vector2 leftStickDirection, Vector2 rightStickDirection)
        {
            if (Stunned)
                return;

            if (leftStickDirection.Length() != 0)
            {
                Facing = leftStickDirection;
            }
            if (leftTrigger > 0.25f)
            {
                Facing = Facing.Rotate(180);
            }
            Velocity = leftStickDirection;
        }

        public bool FrostAttack()
        {
            if (FrostOrbCooldown > TimeSpan.Zero || Mana < AbilityConstants.FrostOrbManaCost || (GetCurrentAnimation().Name != AnimationTypes.Move && GetCurrentAnimation().Name != AnimationTypes.Stand))
                return false;
            _frosting = true;
            return true;
        }

        private bool FrostOrbAttacked()
        {
            Mana -= AbilityConstants.FrostOrbManaCost;
            FrostOrbCooldown = AbilityConstants.FrostOrbCooldown;
            _frosting = false;
            Globals.GameState.Missiles.Add(new FrostOrbMissile(this, new GameObjects.DamageInstance
            {
                Creator = this,
                Damage = FrostOrbDamage,
                DamageType = DamageType.Frost,
                IsCritical = false,
                Knockback = new GameObjects.Knockback(Facing, TimeSpan.FromMilliseconds(250), AbilityConstants.FrostOrbKnockback)
            }, Position));
            return true;
        }

        public bool WandAttack()
        {
            if (WandCooldown > TimeSpan.Zero || (GetCurrentAnimation().Name != AnimationTypes.Move && GetCurrentAnimation().Name != AnimationTypes.Stand))
                return false;
            _wanding = true;
            return true;
        }

        private bool WandAttacked()
        {
            WandCooldown = AbilityConstants.WandCooldown;
            _wanding = false;
            Globals.GameState.Missiles.Add(new WandMissile(this, new GameObjects.DamageInstance
            {
                Creator = this,
                Damage = WandDamage,
                DamageType = DamageType.Magic,
                IsCritical = false,
                Knockback = new GameObjects.Knockback(Facing, TimeSpan.FromMilliseconds(250), AbilityConstants.WandKnockback)
            }, Position));
            return true;
        }

        public bool Dashed()
        {
            if (DashCooldown > 0)
                return false;

            this.AddBuff(BuffTypes.Dash);
            DashCooldown = AbilityConstants.DashCooldown;

            return true;
        }

        public bool Sprinted()
        {
            if (SprintCooldown > 0)
                return false;
            this.AddBuff(BuffTypes.Sprint);
            SprintCooldown = AbilityConstants.SprintCooldown;
            return true;
        }

        public override Tuple<string, bool> GetTextureInfo()
        {
            return new Tuple<string, bool>("Entities/Zerd.png", false);
        }

        public override void Update(GameTime gameTime)
        {
            if (Knockback != null)
            {
                _wanding = false;
                _frosting = false;
            }
            DashCooldown = Math.Max(0, DashCooldown - (float)gameTime.ElapsedGameTime.Milliseconds / 1000);
            SprintCooldown = Math.Max(0, SprintCooldown - (float)gameTime.ElapsedGameTime.Milliseconds / 1000);
            WandCooldown -= gameTime.ElapsedGameTime;
            FrostOrbCooldown -= gameTime.ElapsedGameTime;
            base.Update(gameTime);
        }

        
        public override float SpriteRotation()
        {
            return 1f * (float)Math.PI / 2f;
        }
    }
}
