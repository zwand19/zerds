using System;
using Zerds.Graphics;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.Enums;
using Zerds.AI;
using Zerds.Constants;

namespace Zerds.Entities.Enemies
{
    public class Zombie : Enemy
    {
        private const int TextureSize = 64;
        private WanderAI _ai;

        public Zombie() : base(EnemyTypes.Zombie, EnemyConstants.GetZombieProperties(), "Entities/Zombie.png", true)
        {
            _ai = new WanderAI(this, new Melee(this, EnemyConstants.ZombieMinDamage, EnemyConstants.ZombieMaxDamage), EnemyConstants.ZombieAggroRange, EnemyConstants.ZombieWanderLength);
            
            HitboxSize = 0.8f;
            Width = 64;
            Height = 64;
            AttackRange = EnemyConstants.ZombieAttackRange;

            Animations = new AnimationList();
            var spawnAnimation = new Animation(AnimationTypes.Spawn);
            spawnAnimation.AddFrame(new Rectangle(TextureSize * 13, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.5));
            spawnAnimation.AddFrame(new Rectangle(TextureSize * 14, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.5));
            spawnAnimation.AddFrame(new Rectangle(TextureSize * 14, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.05), SpawnedFunc);
            Animations.Add(spawnAnimation);

            var walkAnimation = new Animation(AnimationTypes.Move);
            walkAnimation.AddFrame(new Rectangle(TextureSize * 9, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.4));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 10, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.4));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 11, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.4));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 12, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.4));
            Animations.Add(walkAnimation);

            var attackAnimation = new Animation(AnimationTypes.Attack);
            attackAnimation.AddFrame(new Rectangle(TextureSize * 7, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.35));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 8, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.4), AttackedFunc);
            attackAnimation.AddFrame(new Rectangle(TextureSize * 8, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1), DoneAttackingFunc);
            Animations.Add(attackAnimation);

            var dieAnimation = new Animation(AnimationTypes.Death);
            dieAnimation.AddFrame(new Rectangle(TextureSize * 0, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1), OnDeath);
            dieAnimation.AddFrame(new Rectangle(TextureSize * 0, 1, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 0, 2, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 0, 3, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 0, 4, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 0, 5, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 0, 6, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 6, 6, TextureSize, TextureSize), TimeSpan.FromSeconds(0.1), OnDeathFinished);
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
