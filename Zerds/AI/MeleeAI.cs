using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;

namespace Zerds.AI
{
    public class MeleeAI : AI
    {
        private readonly Melee _meleeAbility;

        public MeleeAI(Enemy enemy, Melee melee) : base(enemy)
        {
            _meleeAbility = melee;
        }

        public override void Run(GameTime gameTime)
        {
            if (Enemy.Stunned)
                return;
            if (!Enemy.IsAlive)
                State = EnemyStates.Dead;
            var target = Target?.IsAlive == true ? Target : Enemy.GetNearestEnemy();
            switch (State)
            {
                case EnemyStates.Sitting:
                    State = EnemyStates.Chasing;
                    return;
                case EnemyStates.Dead:
                    Enemy.Velocity = Vector2.Zero;
                    Enemy.BaseSpeed = 0;
                    return;
                case EnemyStates.Wandering:
                    CurrentWanderTime = CurrentWanderTime.AddWithGameSpeed(gameTime.ElapsedGameTime);
                    Enemy.BaseSpeed = WanderSpeed;
                    if (CurrentWanderTime > (WanderIntervalTime ?? GameplayConstants.DefaultEnemyWanderInterval))
                        SetWanderTarget();
                    Enemy.Velocity = Enemy.Facing.Normalized();
                    return;
                case EnemyStates.Chasing:
                    if (target == null)
                    {
                        State = EnemyStates.Wandering;
                        return;
                    }
                    CurrentWanderTime = TimeSpan.Zero;
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
