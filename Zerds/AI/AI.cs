using System;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;
using Zerds.Abilities;

namespace Zerds.AI
{
    public abstract class AI
    {
        protected Enemy Enemy;
        protected EnemyStates State;
        protected TimeSpan? WanderIntervalTime;
        protected TimeSpan CurrentWanderTime;
        protected float BaseSpeed;
        protected float WanderSpeed;
        protected float AggroRange;
        protected Ability AttackAbility;

        public Being Target { get; set; }

        protected AI(Enemy enemy, float aggroRange, TimeSpan wanderIntervalTime, Ability attackAbility)
        {
            Enemy = enemy;
            AggroRange = aggroRange;
            AttackAbility = attackAbility;
            State = enemy.IsBoss ? EnemyStates.Sitting : EnemyStates.Wandering;
            BaseSpeed = enemy.BaseSpeed;
            WanderSpeed = enemy.BaseSpeed * GameplayConstants.WanderSpeedFactor;
            SetWanderTarget();
            WanderIntervalTime = wanderIntervalTime;
        }

        public abstract void Run(GameTime gameTime);

        public virtual Animation GetCurrentAnimation()
        {
            if (!Enemy.IsAlive)
                return Enemy.Animations.Get(AnimationTypes.Death);
            return !Enemy.Spawned
                ? Enemy.Animations.Get(AnimationTypes.Spawn)
                : Enemy.Animations.Get(State == EnemyStates.Attacking ? AnimationTypes.Attack : AnimationTypes.Move);
        }

        protected virtual void Chase()
        {
            if (Target == null)
            {
                State = EnemyStates.Wandering;
                return;
            }
            if (Enemy.DistanceBetween(Target) > AggroRange + GameplayConstants.AggroRangeBuffer)
                State = EnemyStates.Wandering;
            CurrentWanderTime = TimeSpan.Zero;
            if ((AttackAbility?.Cooldown ?? TimeSpan.Zero) <= TimeSpan.Zero && Enemy.DistanceBetween(Target) < Enemy.AttackRange)
            {
                Enemy.Velocity = Vector2.Zero;
                State = EnemyStates.Attacking;
            }
            Enemy.Face(Target);
            Enemy.Velocity = Enemy.Facing.Normalized();
        }

        protected virtual void Wander(GameTime gameTime)
        {
            CurrentWanderTime = CurrentWanderTime.AddWithGameSpeed(gameTime.ElapsedGameTime);
            Enemy.BaseSpeed = WanderSpeed;
            if (CurrentWanderTime > (WanderIntervalTime ?? GameplayConstants.DefaultEnemyWanderInterval))
                SetWanderTarget();
            if (Target != null && Enemy.DistanceBetween(Target) < AggroRange - GameplayConstants.AggroRangeBuffer)
                State = EnemyStates.Chasing;
            Enemy.Velocity = Enemy.Facing.Normalized();
        }

        protected virtual void SetWanderTarget()
        {
            CurrentWanderTime = TimeSpan.Zero;
            var ran = Globals.Random;
            var x = Enemy.Position.X + ran.Next(-50, 50);
            if (x < 150)
                x = 150;
            if (x > Globals.ViewportBounds.Width - 150)
                x = Globals.ViewportBounds.Width - 150;
            var y = Enemy.Position.Y + ran.Next(-50, 50);
            if (y < 150)
                y = 150;
            if (y > Globals.ViewportBounds.Height - 150)
                y = Globals.ViewportBounds.Height - 150;
            Enemy.Face(new Point(x, y));
        }
    }
}
