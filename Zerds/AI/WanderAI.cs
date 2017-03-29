using System;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;

namespace Zerds.AI
{
    public sealed class WanderAI : AI
    {
        private readonly Melee _meleeAbility;
        private readonly float _aggroRange;

        public WanderAI(Enemy enemy, Melee melee, float aggroRange, TimeSpan wanderIntervalTime) : base(enemy)
        {
            _meleeAbility = melee;
            _aggroRange = aggroRange;
            WanderIntervalTime = wanderIntervalTime;
            State = EnemyStates.Wandering;
            SetWanderTarget();
        }

        public override void Run(GameTime gameTime)
        {
            if (Enemy.Stunned)
                return;
            if (!Enemy.IsAlive)
                State = EnemyStates.Dead;
            var target = Target?.IsAlive == true ? Target : Enemy.GetNearestEnemy();
            if (target == null)
                State = EnemyStates.Wandering;
            switch (State)
            {
                case EnemyStates.Dead:
                    Enemy.Velocity = Vector2.Zero;
                    Enemy.BaseSpeed = 0;
                    return;
                case EnemyStates.Wandering:
                    CurrentWanderTime = CurrentWanderTime.AddWithGameSpeed(gameTime.ElapsedGameTime);
                    Enemy.BaseSpeed = WanderSpeed;
                    if (CurrentWanderTime > (WanderIntervalTime ?? GameplayConstants.DefaultEnemyWanderInterval))
                        SetWanderTarget();
                    if (target != null && Enemy.DistanceBetween(target) < _aggroRange - GameplayConstants.AggroRangeBuffer)
                        State = EnemyStates.Chasing;
                    Enemy.Velocity = Enemy.Facing.Normalized();
                    return;
                case EnemyStates.Chasing:
                    CurrentWanderTime = TimeSpan.Zero;
                    Enemy.BaseSpeed = BaseSpeed;
                    if (Enemy.DistanceBetween(target) > _aggroRange + GameplayConstants.AggroRangeBuffer)
                        State = EnemyStates.Wandering;
                    if (Enemy.DistanceBetween(target) < Enemy.AttackRange)
                    {
                        Enemy.Velocity = Vector2.Zero;
                        State = EnemyStates.Attacking;
                    }
                    Enemy.Face(target);
                    Enemy.Velocity = Enemy.Facing.Normalized();
                    return;
                case EnemyStates.Attacking:
                    Enemy.Velocity = Vector2.Zero;
                    return;
            }
        }

        public override Animation GetCurrentAnimation()
        {
            if (!Enemy.IsAlive)
                return Enemy.Animations.Get(AnimationTypes.Death);
            return !Enemy.Spawned
                ? Enemy.Animations.Get(AnimationTypes.Spawn)
                : Enemy.Animations.Get(State == EnemyStates.Attacking ? AnimationTypes.Attack : AnimationTypes.Move);
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
    }
}
