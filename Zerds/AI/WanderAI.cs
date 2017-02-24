using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.Buffs;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;

namespace Zerds.AI
{
    public class WanderAI : AI
    {
        private readonly Melee _meleeAbility;
        private readonly float _aggroRange;
        private readonly int _wanderRange;
        private readonly TimeSpan _wanderLength;
        private Point _wanderTarget;
        private TimeSpan _wanderTime;
        private readonly float _baseSpeed;
        private readonly float _wanderSpeed;

        public WanderAI(Enemy enemy, Melee melee, float aggroRange, int wanderRange, TimeSpan wanderLength) : base(enemy)
        {
            _meleeAbility = melee;
            _aggroRange = aggroRange;
            _wanderLength = wanderLength;
            _wanderRange = wanderRange;
            SetWanderTarget();
            State = EnemyStates.Wandering;
            _baseSpeed = enemy.Speed;
            _wanderSpeed = enemy.Speed * 0.25f;
        }

        public override void Run(GameTime gameTime)
        {
            if (Enemy.Stunned)
                return;
            if (!Enemy.IsAlive)
                State = EnemyStates.Dead;
            var target = Target ?? Enemy.GetNearestEnemy();
            if (target == null)
                State = EnemyStates.Wandering;
            switch (State)
            {
                case EnemyStates.Dead:
                    Enemy.Velocity = Vector2.Zero;
                    Enemy.BaseSpeed = 0;
                    return;
                case EnemyStates.Wandering:
                    _wanderTime += gameTime.ElapsedGameTime;
                    Enemy.BaseSpeed = _wanderSpeed;
                    if (_wanderTime > _wanderLength)
                        SetWanderTarget();
                    if (target != null && Enemy.DistanceBetween(target) < _aggroRange - GameplayConstants.AggroRangeBuffer)
                        State = EnemyStates.Chasing;
                    Enemy.Velocity = Enemy.Facing.Normalized();
                    return;
                case EnemyStates.Chasing:
                    Enemy.BaseSpeed = _baseSpeed;
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

        private void SetWanderTarget()
        {
            _wanderTime = TimeSpan.Zero;
            var ran = new Random();
            var x = Enemy.Position.X + ran.Next(-_wanderRange / 2, _wanderRange / 2);
            if (x < 150)
                x = 150;
            if (x > Globals.ViewportBounds.Width - 150)
                x = Globals.ViewportBounds.Width - 150;
            var y = Enemy.Position.Y + ran.Next(-_wanderRange / 2, _wanderRange / 2);
            if (y < 150)
                y = 150;
            if (y > Globals.ViewportBounds.Height - 150)
                y = Globals.ViewportBounds.Height - 150;
            _wanderTarget = new Point(x, y);
            Enemy.Face(_wanderTarget);
        }
    }
}
