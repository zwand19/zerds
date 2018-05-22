using System;
using Zerds.Graphics;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.AI;
using Zerds.Constants;
using Zerds.Enums;
using Zerds.GameObjects;

namespace Zerds.Entities.Enemies
{
    public class Dog : Enemy
    {
        private const int TextureSize = 64;
        private MeleeAI _ai;

        public Dog(MapSection section) : base(EnemyTypes.Dog, EnemyConstants.GetDogProperties(), "Entities/Dog.png", section)
        {
            _ai = new MeleeAI(this, EnemyConstants.DogAggroRange, new Melee(this, 6, 8), EnemyConstants.DogWanderLength);

            HitboxSize = 0.8f;
            Width = 64;
            Height = 64;
            AttackRange = 48;

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
            attackAnimation.AddFrame(new Rectangle(TextureSize * 0, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.35));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 1, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.4), AttackedFunc);
            attackAnimation.AddFrame(new Rectangle(TextureSize * 1, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1), DoneAttackingFunc);
            Animations.Add(attackAnimation);

            var dieAnimation = new Animation(AnimationTypes.Death);
            dieAnimation.AddFrame(new Rectangle(TextureSize * 7, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.5), OnDeath);
            dieAnimation.AddFrame(new Rectangle(TextureSize * 6, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.5));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 6, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1), OnDeathFinished);
            Animations.Add(dieAnimation);
        }
        
        public override AI.AI GetAI() => _ai;

        private bool SpawnedFunc()
        {
            Spawned = true;
            return true;
        }

        private bool AttackedFunc()
        {
            return _ai.Attacked();
        }

        private bool DoneAttackingFunc()
        {
            return _ai.FinishedAttacking();
        }

        public override float SpriteRotation()
        {
            return 3f * (float)Math.PI / 2f;
        }
    }
}
