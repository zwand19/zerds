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
        public TimeSpan WandCooldown { get; set; }
        public PlayerIndex PlayerIndex { get; private set; }

        private bool _wanding = false;

        public Zerd(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
            X = 650;
            Y = 300;
            Width = 64;
            Height = 64;
            WandDamage = 10;
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
        }

        public override Animation GetCurrentAnimation()
        {
            return Knockback == null ? Animations.Get(AnimationTypes.Stand) : Animations.Get(AnimationTypes.Damaged);
        }

        public void ControllerUpdate(Vector2 leftStickDirection, Vector2 rightStickDirection)
        {
            if (Stunned)
                return;

            if (rightStickDirection.Length() != 0)
            {
                Facing = rightStickDirection;
            }
            else if (leftStickDirection.Length() != 0)
            {
                Facing = leftStickDirection;
            }
            Velocity = leftStickDirection;
            var angle = Velocity.AngleBetween(Facing);
            Speed *= angle < GameplayConstants.ZerdFrontFacingAngle ? 1 : angle > 180 - GameplayConstants.ZerdFrontFacingAngle ? GameplayConstants.BackpedalFactor : GameplayConstants.SideStepFactor;
        }

        public bool WandAttack()
        {
            _wanding = true;
            return true;
        }

        private bool WandAttacked()
        {
            if (WandCooldown <= TimeSpan.Zero && GetCurrentAnimation().Name != AnimationTypes.Move && GetCurrentAnimation().Name != AnimationTypes.Stand)
                return false;
            WandCooldown = GameplayConstants.WandCooldown;
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
            DashCooldown = Math.Max(0, DashCooldown - (float)gameTime.ElapsedGameTime.Milliseconds / 1000);
            SprintCooldown = Math.Max(0, SprintCooldown - (float)gameTime.ElapsedGameTime.Milliseconds / 1000);
            base.Update(gameTime);
        }

        
        public override float SpriteRotation()
        {
            return 1f * (float)Math.PI / 2f;
        }
    }
}
