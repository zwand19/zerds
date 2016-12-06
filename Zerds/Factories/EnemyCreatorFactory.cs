using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Entities.Enemies;
using Zerds.Enums;
using Zerds.Graphics;

namespace Zerds.Factories
{
    public static class EnemyCreatorFactory
    {
        public static Enemy CreateEnemy(EnemyTypes type)
        {
            switch (type)
            {
                case EnemyTypes.Zombie:
                    return InitializeEnemy(new Zombie(), EnemyConstants.GetZombieProperties());
            }
            throw new ArgumentException("Unknown Enemy Type");
        }

        public static List<Enemy> CreateEnemyBatch()
        {
            switch (Globals.GameState.Level)
            {
                case 1:
                    return new List<Enemy> { CreateEnemy(EnemyTypes.Zombie) };
            }
            return new List<Enemy>();
        }

        public static Enemy InitializeEnemy(Enemy enemy, EnemyConstants.EnemyProperties properties)
        {
            float health = new Random().Next(properties.MaxHealth - properties.MinHealth) + properties.MinHealth;
            float mana = new Random().Next(properties.MaxMana- properties.MinMana) + properties.MinMana;
            float speed = (float)new Random().NextDouble() * (properties.MaxSpeed - properties.MinSpeed) + properties.MinSpeed;
            health *= DifficultyConstants.HealthFactor;
            mana *= DifficultyConstants.ManaFactor;
            speed *= DifficultyConstants.SpeedFactor;
            properties.HealthRegen *= DifficultyConstants.HealthFactor;
            properties.ManaRegen *= DifficultyConstants.ManaFactor;
            enemy.Initialize(health, mana, properties.HealthRegen, properties.ManaRegen, speed);
            enemy.InitializeEnemy();
            enemy.Spawn();
            return enemy;
        }

        public static void Update(GameTime gameTime)
        {
            if (Globals.GameState.Enemies.Count() < 2)
                Globals.GameState.Enemies.AddRange(CreateEnemyBatch());
        }
    }
}
