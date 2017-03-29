using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.Entities;
using Zerds.Enums;

namespace Zerds.AI
{
    public class RangeAI : AI
    {
        private readonly RangeAttack _rangeAttack;

        public RangeAI(Enemy enemy, RangeAttack range) : base(enemy)
        {
            _rangeAttack = range;
        }

        public override void Run(GameTime gameTime)
        {
            _rangeAttack.Update(gameTime);
            if (Enemy.Stunned)
                return;
            if (!Enemy.IsAlive)
                State = EnemyStates.Dead;
            var target = Target?.IsAlive == true ? Target : Enemy.GetNearestEnemy();
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
                    if (_rangeAttack.Cooldown <= TimeSpan.Zero && Enemy.DistanceBetween(target) < Enemy.AttackRange)
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
            return _rangeAttack.Attacked();
        }

        public bool FinishedAttacking()
        {
            State = EnemyStates.Chasing;
            return true;
        }
    }
}
