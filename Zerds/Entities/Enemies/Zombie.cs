using System;
using Zerds.Graphics;
using Microsoft.Xna.Framework;
using Zerds.Enums;
using Zerds.AI;
using System.Collections.Generic;
using Zerds.Factories;
using Zerds.GameObjects;

namespace Zerds.Entities.Enemies
{
    public class Zombie : Enemy
    {
        private const int TextureSize = 64;
        private const int AttackRange = 50;
        private const int AttackDamage = 8;
        private bool _attacking = false;

        public override void InitializeEnemy()
        {
            BaseSpeed = 75f;

            Animations = new AnimationList();
            var spawnAnimation = new Animation(AnimationTypes.Spawn);
            spawnAnimation.AddFrame(new Rectangle(TextureSize * 0, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.5));
            spawnAnimation.AddFrame(new Rectangle(TextureSize * 1, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.5));
            spawnAnimation.AddFrame(new Rectangle(TextureSize * 1, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.05), SpawnedFunc);
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
        }

        private bool SpawnedFunc()
        {
            Spawned = true;
            return true;
        }

        private bool AttackedFunc()
        {
            var rect = new Rectangle((int)(X + Facing.X * AttackRange * 0.8f) - 10, (int)(Y - Facing.Y * AttackRange * 0.8f) - 10, 20, 20);
            var rect2 = new Rectangle((int)(X + Facing.X * AttackRange * 0.2f) - 14, (int)(Y - Facing.Y * AttackRange * 0.2f) - 14, 28, 28);
            foreach (var zerd in Globals.GameState.Zerds)
            {
                var zerdBox = zerd.Hitbox();
                if (zerdBox.Intersects(rect) || zerdBox.Intersects(rect2))
                {
                    var damageInstance = new DamageInstance
                    {
                        Creator = this,
                        Damage = AttackDamage,
                        Knockback = new Knockback(Facing, new TimeSpan(0, 0, 0, 0, 250), 250f),
                        DamageType = DamageType.Physical,
                        IsCritical = false
                    };
                    if (new Random().Next(100) < 8)
                    {
                        damageInstance.Damage *= 2;
                        damageInstance.IsCritical = true;
                    }
                    damageInstance.DamageBeing(zerd);
                }
            }
            return true;
        }

        private bool DoneAttackingFunc()
        {
            _attacking = false;
            return true;
        }

        public override Animation GetCurrentAnimation()
        {
            if (!Spawned)
                return Animations.Get(AnimationTypes.Spawn);
            return Animations.Get(_attacking ? AnimationTypes.Attack : AnimationTypes.Move);
        }

        public override Tuple<string, bool> GetTextureInfo()
        {
            return new Tuple<string, bool>("Entities/Zombie.png", false);
        }

        public override void RunAI()
        {
            if (_attacking || Stunned)
                return;
            var target = this.GetNearestZerd();
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
