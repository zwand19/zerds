using System;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;

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
        public Being Target { get; set; }

        protected AI(Enemy enemy)
        {
            Enemy = enemy;
            State = EnemyStates.Sitting;
            BaseSpeed = enemy.BaseSpeed;
            WanderSpeed = enemy.BaseSpeed * GameplayConstants.WanderSpeedFactor;
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

        protected virtual void SetWanderTarget()
        {
            CurrentWanderTime = TimeSpan.Zero;
            var ran = new Random();
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
