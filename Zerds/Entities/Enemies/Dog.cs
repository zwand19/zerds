using System;
using Zerds.Graphics;
using Microsoft.Xna.Framework;
using Zerds.AI;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;

namespace Zerds.Entities.Enemies
{
    public class Dog : Enemy
    {
        private const int TextureSize = 64;
        private const int AttackRange = 50;
        private const int AttackDamage = 6;
        private bool _attacking;

        public Dog() : base("Entities/Dog.png")
        {

        }

        public override void InitializeEnemy()
        {
            HitboxSize = 0.8f;
            Width = 64;
            Height = 64;

            Animations = new AnimationList();
            var spawnAnimation = new Animation(AnimationTypes.Spawn);
            spawnAnimation.AddFrame(new Rectangle(TextureSize * 6, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.5));
            spawnAnimation.AddFrame(new Rectangle(TextureSize * 7, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.5));
            spawnAnimation.AddFrame(new Rectangle(TextureSize * 7, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.05), SpawnedFunc);
            Animations.Add(spawnAnimation);

            var walkAnimation = new Animation(AnimationTypes.Move);
            walkAnimation.AddFrame(new Rectangle(TextureSize * 2, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.4));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 3, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.4));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 4, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.4));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 5, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.4));
            Animations.Add(walkAnimation);

            var attackAnimation = new Animation(AnimationTypes.Attack);
            attackAnimation.AddFrame(new Rectangle(TextureSize * 0, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.5));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 1, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.4), AttackedFunc);
            attackAnimation.AddFrame(new Rectangle(TextureSize * 1, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1), DoneAttackingFunc);
            Animations.Add(attackAnimation);

            var dieAnimation = new Animation(AnimationTypes.Death);
            dieAnimation.AddFrame(new Rectangle(TextureSize * 7, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.5));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 6, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.5));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 6, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1), DeathFunc);
            Animations.Add(dieAnimation);
        }

        private bool SpawnedFunc()
        {
            Spawned = true;
            return true;
        }

        private bool DeathFunc()
        {
            IsActive = false;
            return true;
        }

        private bool AttackedFunc()
        {
            var rect = new Rectangle((int)(X + Facing.X * AttackRange * 0.8f) - 10, (int)(Y - Facing.Y * AttackRange * 0.8f) - 10, 20, 20);
            var rect2 = new Rectangle((int)(X + Facing.X * AttackRange * 0.2f) - 14, (int)(Y - Facing.Y * AttackRange * 0.2f) - 14, 28, 28);
            foreach (var zerd in Globals.GameState.Zerds)
            {
                foreach (var hitbox in zerd.Hitbox())
                {
                    if (hitbox.Intersects(rect) || hitbox.Intersects(rect2))
                    {
                        var damageInstance = new DamageInstance
                        {
                            Creator = this,
                            Damage = AttackDamage,
                            Knockback = new Knockback(Facing, new TimeSpan(0, 0, 0, 0, 250), 250f),
                            DamageType = DamageTypes.Physical,
                            IsCritical = false
                        };
                        if (new Random().Next(100) < 8)
                        {
                            damageInstance.Damage *= 2;
                            damageInstance.IsCritical = true;
                        }
                        damageInstance.DamageBeing(zerd);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DoneAttackingFunc()
        {
            _attacking = false;
            return true;
        }

        public override Animation GetCurrentAnimation()
        {
            if (!IsAlive)
                return Animations.Get(AnimationTypes.Death);
            if (!Spawned)
                return Animations.Get(AnimationTypes.Spawn);
            return Animations.Get(_attacking ? AnimationTypes.Attack : AnimationTypes.Move);
        }

        public override void RunAI()
        {
            if (_attacking || Stunned)
                return;
            var target = this.GetNearestZerd();
            if (target == null)
            {
                Velocity = Vector2.Zero;
                _attacking = false;
                return;
            }
            if (GetCurrentAnimation().Name == AnimationTypes.Move)
            {
                this.Face(target);
                Velocity = Facing.Normalized();
            }
            if (this.DistanceBetween(target) < AttackRange)
            {
                Velocity = Vector2.Zero;
                _attacking = true;
            }
        }

        public override void Spawn()
        {
            var random = new Random();
            X = random.Next(Globals.ViewportBounds.Width);
            Y = random.Next(Globals.ViewportBounds.Height);
        }

        public override float SpriteRotation()
        {
            return 3f * (float)Math.PI / 2f;
        }
    }
}
