﻿using Microsoft.Xna.Framework;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;

namespace Zerds.AI
{
    public abstract class AI
    {
        protected Enemy Enemy;
        protected EnemyStates State;

        protected AI(Enemy enemy)
        {
            Enemy = enemy;
            State = EnemyStates.Sitting;
        }

        public abstract void Run(GameTime gameTime);
        public abstract Animation GetCurrentAnimation();
    }
}
