using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.Buffs;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.Graphics;

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
