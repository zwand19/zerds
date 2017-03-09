using System;
using Zerds.Graphics;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.AI;
using Zerds.Constants;
using Zerds.Enums;

namespace Zerds.Entities.Enemies
{
    public class Archer : Enemy
    {
        private const int TextureSize = 64;
        private RangeAI _ai;

        public Archer() : base(EnemyTypes.Archer, EnemyConstants.GetArcherProperties(), "Entities/Archer.png", false)
        {
            _ai = new RangeAI(this, new RangeAttack(this, 12, 18, MissileTypes.Arrow, EnemyConstants.ArcherArrowCooldown));

            HitboxSize = 0.93f;
            Width = 64;
            Height = 64;
            AttackRange = 625;

            Animations = new AnimationList();
            var spawnAnimation = new Animation(AnimationTypes.Spawn);
            spawnAnimation.AddFrame(new Rectangle(0, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.05), SpawnedFunc);
            Animations.Add(spawnAnimation);

            var walkAnimation = new Animation(AnimationTypes.Move);
            walkAnimation.AddFrame(new Rectangle(TextureSize * 0, TextureSize * 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 1, TextureSize * 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 2, TextureSize * 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 3, TextureSize * 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            Animations.Add(walkAnimation);

            var attackAnimation = new Animation(AnimationTypes.Attack);
            attackAnimation.AddFrame(new Rectangle(TextureSize * 0, TextureSize * 1, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 1, TextureSize * 1, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 2, TextureSize * 1, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25), AttackedFunc);
            attackAnimation.AddFrame(new Rectangle(TextureSize * 2, TextureSize * 1, TextureSize, TextureSize), TimeSpan.FromSeconds(0.05), DoneAttackingFunc);
            Animations.Add(attackAnimation);
            
            var hitAnimation = new Animation(AnimationTypes.Damaged);
            hitAnimation.AddFrame(new Rectangle(TextureSize * 3, TextureSize * 1, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            Animations.Add(hitAnimation);

            var dieAnimation = new Animation(AnimationTypes.Death);
            dieAnimation.AddFrame(new Rectangle(TextureSize * 3, TextureSize * 1, TextureSize, TextureSize), TimeSpan.FromSeconds(0.35), OnDeath);
            dieAnimation.AddFrame(new Rectangle(TextureSize * 3, TextureSize * 1, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25), OnDeathFinished);
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
            return 1f * (float)Math.PI / 2f;
        }
    }
}
