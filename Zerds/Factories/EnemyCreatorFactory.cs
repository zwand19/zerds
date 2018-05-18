using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Entities.Enemies;
using Zerds.Enums;
using Zerds.GameObjects;

namespace Zerds.Factories
{
    /// <summary>
    /// Creates randomly spawned enemies in the section that a player is in
    /// </summary>
    public static class EnemyCreatorFactory
    {
        private const int MinSectionDifficulty = 20;
        private const int SectionDifficultyRange = 50;

        public static void Update(GameTime gameTime)
        {
            var enemyDifficulty = Globals.GameState.Enemies.Sum(e => e.Worth());
            float playerFactor;
            switch (Globals.GameState.Zerds.Count)
            {
                case 1:
                    playerFactor = 1f;
                    break;
                case 2:
                    playerFactor = 1.6f;
                    break;
                case 3:
                    playerFactor = 2.1f;
                    break;
                case 4:
                    playerFactor = 2.6f;
                    break;
                default:
                    throw new ArgumentException(nameof(playerFactor));
            }
            var targetDifficulty = Level.CurrentLevel * 10 * playerFactor;
            if (enemyDifficulty < targetDifficulty && Level.TimeRemaining > TimeSpan.Zero)
                Globals.GameState.Enemies.AddRange(CreateEnemyBatch());
        }

        public static void CreateMapSectionEnemies(MapSection section)
        {
            float playerFactor;
            switch (Globals.GameState.Zerds.Count)
            {
                case 1:
                    playerFactor = 1f;
                    break;
                case 2:
                    playerFactor = 1.6f;
                    break;
                case 3:
                    playerFactor = 2.1f;
                    break;
                case 4:
                    playerFactor = 2.6f;
                    break;
                default:
                    throw new ArgumentException(nameof(playerFactor));
            }

            // Calculate how hard this section should be
            var sectionPoints = (MinSectionDifficulty + Globals.Random.Next(SectionDifficultyRange)) * playerFactor;
            var currentWorth = 0;
            // Create enemies without going over the section difficulty
            while (true)
            {
                Enemy e = null;

                // If we can't create anymore enemies we're done
                if (currentWorth > sectionPoints - MinSectionDifficulty)
                    return;

                while (e == null) {
                    var type = Globals.Random.Next(5);
                    switch (type)
                    {
                        case 0:
                            if (currentWorth < sectionPoints - EnemyConstants.ZombieWorth)
                            {
                                e = new Zombie(section);
                                currentWorth += EnemyConstants.ZombieWorth;
                            }
                            break;
                        case 1:
                            if (currentWorth < sectionPoints - EnemyConstants.DogWorth)
                            {
                                e = new Dog(section);
                                currentWorth += EnemyConstants.DogWorth;
                            }
                            break;
                        case 2:
                            if (currentWorth < sectionPoints - EnemyConstants.DemonWorth)
                            {
                                e = new Demon(section);
                                currentWorth += EnemyConstants.DemonWorth;
                            }
                            break;
                        case 3:
                            if (currentWorth < sectionPoints - EnemyConstants.FrostDemonWorth)
                            {
                                e = new FrostDemon(section);
                                currentWorth += EnemyConstants.FrostDemonWorth;
                            }
                            break;
                        case 4:
                            if (currentWorth < sectionPoints - EnemyConstants.ArcherWorth)
                            {
                                e = new Archer(section);
                                currentWorth += EnemyConstants.ArcherWorth;
                            }
                            break;
                    }
                }

                Globals.GameState.Enemies.Add(e);
            }
        }

        private static Enemy CreateEnemy(EnemyTypes type)
        {
            // Pass a null section so that the enemy is created in the same map section as a random zerd
            switch (type)
            {
                case EnemyTypes.Zombie:
                    return new Zombie(null);
                case EnemyTypes.Dog:
                    return new Dog(null);
                case EnemyTypes.Demon:
                    return new Demon(null);
                case EnemyTypes.FrostDemon:
                    return new FrostDemon(null);
                case EnemyTypes.Archer:
                    return new Archer(null);
                case EnemyTypes.SkeletonKing:
                    return new SkeletonKing(null);
            }
            throw new ArgumentException("Unknown Enemy ItemType");
        }

        private static List<Enemy> CreateEnemyBatch()
        {
            var rand = Globals.Random.NextDouble();
            switch (Level.CurrentLevel)
            {
                case 1:
                    return new List<Enemy> { CreateEnemy(rand < 0.9 ? EnemyTypes.Zombie : EnemyTypes.Dog) };
                case 2:
                    return new List<Enemy> { CreateEnemy(rand < 0.25 ? EnemyTypes.Archer : rand < 0.82 ? EnemyTypes.Zombie : EnemyTypes.Dog) };
                case 3:
                    return new List<Enemy> { CreateEnemy(rand < 0.25 ? EnemyTypes.Archer : rand < 0.5 ? EnemyTypes.Zombie : rand < 0.9 ? EnemyTypes.Dog : rand < 0.95 ? EnemyTypes.Demon : EnemyTypes.FrostDemon) };
                case 4:
                    return new List<Enemy> { CreateEnemy(rand < 0.25 ? EnemyTypes.Archer : rand < 0.35 ? EnemyTypes.Zombie : rand < 0.5 ? EnemyTypes.Dog : rand < 0.75 ? EnemyTypes.Demon : EnemyTypes.FrostDemon) };
                case 5:
                    return new List<Enemy> { CreateEnemy(rand < 0.24 ? EnemyTypes.Archer : rand < 0.33 ? EnemyTypes.Zombie : rand < 0.44 ? EnemyTypes.Dog : rand < 0.78 ? EnemyTypes.Demon : EnemyTypes.FrostDemon) };
                case 6:
                    return new List<Enemy> { CreateEnemy(rand < 0.24 ? EnemyTypes.Archer : rand < 0.3 ? EnemyTypes.Zombie : rand < 0.4 ? EnemyTypes.Dog : rand < 0.7 ? EnemyTypes.Demon : EnemyTypes.FrostDemon) };
                case 7:
                    return new List<Enemy> { CreateEnemy(rand < 0.22 ? EnemyTypes.Archer : rand < 0.25 ? EnemyTypes.Dog : rand < 0.6 ? EnemyTypes.Demon : EnemyTypes.FrostDemon) };
            }
            return new List<Enemy>();
        }
    }
}
