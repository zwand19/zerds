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
    public class Demon : Enemy
    {
        private const int TextureSize = 64;
        private RangeAI _ai;

        public Demon(MapSection section) : base(EnemyTypes.Demon, EnemyConstants.GetDemonProperties(), "Entities/Demon.png", section)
        {
            _ai = new RangeAI(this, new RangeAttack(this, 12, 18, MissileTypes.DemonMissile, EnemyConstants.FrostDemonMissileCooldown));

            HitboxSize = 0.93f;
            Width = 64;
            Height = 64;
            AttackRange = 400;

            Animations = new AnimationList();
            var spawnAnimation = new Animation(AnimationTypes.Spawn);
            spawnAnimation.AddFrame(new Rectangle(0, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.05), SpawnedFunc);
            Animations.Add(spawnAnimation);

            var walkAnimation = new Animation(AnimationTypes.Move);
            walkAnimation.AddFrame(new Rectangle(TextureSize * 0, TextureSize * 2, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 1, TextureSize * 2, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 2, TextureSize * 2, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 3, TextureSize * 2, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 4, TextureSize * 2, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 5, TextureSize * 2, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 6, TextureSize * 2, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            walkAnimation.AddFrame(new Rectangle(TextureSize * 7, TextureSize * 2, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            Animations.Add(walkAnimation);

            var attackAnimation = new Animation(AnimationTypes.Attack);
            attackAnimation.AddFrame(new Rectangle(TextureSize * 0, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 1, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 2, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 3, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 4, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 5, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25), AttackedFunc);
            attackAnimation.AddFrame(new Rectangle(TextureSize * 6, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 7, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            attackAnimation.AddFrame(new Rectangle(TextureSize * 7, 0, TextureSize, TextureSize), TimeSpan.FromSeconds(0.05), DoneAttackingFunc);
            Animations.Add(attackAnimation);

            var dieAnimation = new Animation(AnimationTypes.Death);
            dieAnimation.AddFrame(new Rectangle(TextureSize * 0, TextureSize * 4, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25), OnDeath);
            dieAnimation.AddFrame(new Rectangle(TextureSize * 1, TextureSize * 4, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 2, TextureSize * 4, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 3, TextureSize * 4, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 4, TextureSize * 4, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 5, TextureSize * 4, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 6, TextureSize * 4, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 7, TextureSize * 4, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 0, TextureSize * 5, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 1, TextureSize * 5, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 2, TextureSize * 5, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 3, TextureSize * 5, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 4, TextureSize * 5, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 5, TextureSize * 5, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 6, TextureSize * 5, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25));
            dieAnimation.AddFrame(new Rectangle(TextureSize * 7, TextureSize * 5, TextureSize, TextureSize), TimeSpan.FromSeconds(0.25), OnDeathFinished);
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
