using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Zerds.Entities;
using Zerds.Entities.Enemies;
using Zerds.Enums;
using Zerds.GameObjects;

namespace Zerds.Factories
{
    public static class EnemyCreatorFactory
    {
        public static Enemy CreateEnemy(EnemyTypes type)
        {
            switch (type)
            {
                case EnemyTypes.Zombie:
                    return new Zombie();
                case EnemyTypes.Dog:
                    return new Dog();
                case EnemyTypes.Demon:
                    return new Demon();
                case EnemyTypes.FrostDemon:
                    return new FrostDemon();
                case EnemyTypes.Archer:
                    return new Archer();
                case EnemyTypes.SkeletonKing:
                    return new SkeletonKing();
            }
            throw new ArgumentException("Unknown Enemy Type");
        }

        public static List<Enemy> CreateEnemyBatch()
        {
            var rand = new Random().NextDouble();
            switch (Level.CurrentLevel)
            {
                case 1:
                    //return new List<Enemy> { CreateEnemy(rand < 0.9 ? EnemyTypes.SkeletonKing : EnemyTypes.Dog) };
                    return new List<Enemy> { CreateEnemy(rand < 0.9 ? EnemyTypes.Zombie : EnemyTypes.Dog) };
                case 2:
                    return new List<Enemy> { CreateEnemy(rand < 0.25 ? EnemyTypes.Archer : rand < 0.82 ? EnemyTypes.Zombie : EnemyTypes.Dog) };
                case 3:
                    return new List<Enemy> { CreateEnemy(rand < 0.25 ? EnemyTypes.Archer : rand < 0.5 ? EnemyTypes.Zombie : rand < 0.9 ? EnemyTypes.Dog : rand < 0.95 ? EnemyTypes.Demon : EnemyTypes.FrostDemon)};
                case 4:
                    return new List<Enemy> { CreateEnemy(rand < 0.25 ? EnemyTypes.Archer : rand < 0.35 ? EnemyTypes.Zombie : rand < 0.5 ? EnemyTypes.Dog : rand < 0.75 ? EnemyTypes.Demon : EnemyTypes.FrostDemon) };
                case 5:
                    return new List<Enemy> { CreateEnemy(EnemyTypes.SkeletonKing) };
                case 6:
                    return new List<Enemy> { CreateEnemy(rand < 0.24 ? EnemyTypes.Archer : rand < 0.3 ? EnemyTypes.Zombie : rand < 0.4 ? EnemyTypes.Dog : rand < 0.7 ? EnemyTypes.Demon : EnemyTypes.FrostDemon) };
                case 7:
                    return new List<Enemy> { CreateEnemy(rand < 0.22 ? EnemyTypes.Archer : rand < 0.25 ? EnemyTypes.Dog : rand < 0.6 ? EnemyTypes.Demon : EnemyTypes.FrostDemon) };
            }
            return new List<Enemy>();
        }

        public static void Update(GameTime gameTime)
        {
            var enemyDifficulty = Globals.GameState.Enemies.Sum(e =>
            {
                if (e is Zombie)
                    return 5;
                if (e is Dog)
                    return 7;
                if (e is Archer)
                    return 8;
                if (e is Demon)
                    return 17;
                if (e is FrostDemon)
                    return 17;
                if (e is SkeletonKing)
                    return 50;
                return 0;
            });
            var targetDifficulty = Level.CurrentLevel * 10 * (0.6 + Globals.GameState.Players.Count(p => p.IsPlaying) * 0.4);
            if (enemyDifficulty < targetDifficulty && Level.TimeRemaining > TimeSpan.Zero)
                Globals.GameState.Enemies.AddRange(CreateEnemyBatch());
        }
    }
}
