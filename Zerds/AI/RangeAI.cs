using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Constants;

namespace Zerds.AI
{
    public class RangeAI : AI
    {
        private readonly RangeAttack _rangeAttack;

        public RangeAI(Enemy enemy, float aggroRange, TimeSpan wanderInterval, RangeAttack attack) : base(enemy, aggroRange, wanderInterval, attack)
        {
            _rangeAttack = attack;
            AggroRange = aggroRange;
        }

        public override void Run(GameTime gameTime)
        {
            _rangeAttack.Update(gameTime);
            if (Enemy.Stunned)
                return;
            if (!Enemy.IsAlive)
                State = EnemyStates.Dead;
            Target = Target?.IsAlive == true ? Target : Enemy.GetNearestEnemy();
            switch (State)
            {
                case EnemyStates.Dead:
                    Enemy.Velocity = Vector2.Zero;
                    Enemy.BaseSpeed = 0;
                    return;
                case EnemyStates.Sitting:
                    State = EnemyStates.Wandering;
                    return;
                case EnemyStates.Wandering:
                    Wander(gameTime);
                    return;
                case EnemyStates.Chasing:
                    Chase();
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
