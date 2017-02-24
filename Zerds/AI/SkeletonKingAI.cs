using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.Enums;
using Zerds.Graphics;
using Zerds.Entities.Enemies;
using System;
using System.Collections.Generic;
using Zerds.Entities;

namespace Zerds.AI
{
    public class SkeletonKingAI : AI
    {
        private readonly Melee _meleeAbility;
        private readonly SkeletonKingBlast _blastAbility;
        private bool _enraged;

        public SkeletonKingAI(SkeletonKing skeletonKing, Melee melee, SkeletonKingBlast blast) : base(skeletonKing)
        {
            _meleeAbility = melee;
            _blastAbility = blast;
            _meleeAbility.GetHitboxes = GetMeleeHitboxes;
        }

        private List<Rectangle> GetMeleeHitboxes(Being sk)
        {
            return new List<Rectangle>
            {
                new Rectangle((int)sk.X - 70, (int)sk.Y - 70, 140, 140),
                Helpers.CreateRect(sk.X + sk.Facing.X * 95 - 40, sk.Y - sk.Facing.Y * 95 - 40, 80, 80)
            };
        }

        public override void Run(GameTime gameTime)
        {
            _blastAbility.Update(gameTime);
            if (Enemy.Stunned)
                return;
            if (!Enemy.IsAlive)
                State = EnemyStates.Dead;
            var target = Enemy.GetNearestEnemy();
            if (target == null)
            {
                Enemy.Velocity = Vector2.Zero;
                State = EnemyStates.Sitting;
                return;
            }
            switch (State)
            {
                case EnemyStates.Dead:
                    Enemy.Velocity = Vector2.Zero;
                    Enemy.BaseSpeed = 0;
                    return;
                case EnemyStates.Sitting:
                    Enemy.Velocity = Vector2.Zero;
                    if (Enemy.Spawned && Enemy.Enemies().Any(z => z.IsAlive))
                        State = EnemyStates.Chasing;
                    return;
                case EnemyStates.Chasing:
                    if (!Enemy.Enemies().Any(z => z.IsAlive))
                    {
                        State = EnemyStates.Sitting;
                        return;
                    }
                    if (Enemy.DistanceBetween(target) < Enemy.AttackRange)
                    {
                        Enemy.Velocity = Vector2.Zero;
                        State = EnemyStates.Attacking;
                    }
                    if (_blastAbility.Cooldown <= TimeSpan.Zero)
                    {
                        Enemy.Velocity = Vector2.Zero;
                        State = EnemyStates.Casting;
                    }
                    Enemy.Face(target);
                    Enemy.Velocity = Enemy.Facing.Normalized();
                    return;
                case EnemyStates.Casting:
                    Enemy.Velocity = Vector2.Zero;
                    return;
                case EnemyStates.Attacking:
                    Enemy.Velocity = Vector2.Zero;
                    return;
            }
        }

        public override Animation GetCurrentAnimation()
        {
            if (_enraged)
            {
                if (!Enemy.IsAlive)
                    return Enemy.Animations.Get(AnimationTypes.DeathEnraged);
                return Enemy.Animations.Get(State == EnemyStates.Attacking ? AnimationTypes.AttackEnraged : State == EnemyStates.Casting ? AnimationTypes.FireAttackEnraged : AnimationTypes.MoveEnraged);
            }
            if (!Enemy.IsAlive)
                return Enemy.Animations.Get(AnimationTypes.Death);
            return !Enemy.Spawned
                ? Enemy.Animations.Get(AnimationTypes.Spawn)
                : Enemy.Animations.Get(State == EnemyStates.Attacking ? AnimationTypes.Attack : State == EnemyStates.Casting ? AnimationTypes.FireAttack : AnimationTypes.Move);
        }

        public bool Casted()
        {
            return _blastAbility.Casted();
        }

        public bool Attacked()
        {
            return _meleeAbility.Attacked();
        }

        public bool FinishedAttacking()
        {
            State = EnemyStates.Chasing;
            return true;
        }

        public void Enrage()
        {
            _enraged = true;
        }
    }
}
