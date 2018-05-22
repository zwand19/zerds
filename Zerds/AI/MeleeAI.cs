using System;
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

        public MeleeAI(Enemy enemy, float aggroRange, Melee melee, TimeSpan wanderInterval) : base(enemy, aggroRange, wanderInterval, melee)
        {
            _meleeAbility = melee;
        }

        public override void Run(GameTime gameTime)
        {
            if (Enemy.Stunned)
                return;
            if (!Enemy.IsAlive)
                State = EnemyStates.Dead;
            Target = Target?.IsAlive == true ? Target : Enemy.GetNearestEnemy();
            switch (State)
            {
                case EnemyStates.Sitting:
                    State = EnemyStates.Wandering;
                    return;
                case EnemyStates.Dead:
                    Enemy.Velocity = Vector2.Zero;
                    Enemy.BaseSpeed = 0;
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
            return _meleeAbility.Attacked();
        }

        public bool FinishedAttacking()
        {
            State = EnemyStates.Chasing;
            return true;
        }
    }
}
