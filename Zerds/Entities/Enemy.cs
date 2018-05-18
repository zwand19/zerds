using System;
using System.Linq;
using Zerds.Buffs;
using Zerds.Constants;
using Zerds.GameObjects;
using Zerds.Graphics;
using Zerds.Consumables;
using Zerds.Enums;
using Zerds.Items;
using Microsoft.Xna.Framework;

namespace Zerds.Entities
{
    public abstract class Enemy : Being
    {
        public int MinCreatedHealth { get; set; }
        public int MaxCreatedHealth { get; set; }
        public int MinCreatedMana { get; set; }
        public int MaxCreatedMana { get; set; }
        public bool Spawned { get; set; }
        public float AttackRange { get; set; }
        public float AttackDamage { get; set; }
        public bool Charmed => Buffs.Any(b => b is CharmBuff);
        public EnemyTypes Type { get; set; }
        public bool IsSpawnedEnemy { get; set; } // Spawned enemies are constantly created and not created on initial map load

        protected Enemy(EnemyTypes type, EnemyConstants.EnemyProperties properties, string file, MapSection mapSection) : base(file, true)
        {
            Initialize(type, properties, file);

            if (mapSection == null)
            {
                // Find a random zerd to spawn near
                var zerdTarget = Globals.GameState.Zerds.RandomElement();
                mapSection = Globals.Map.GetSection(zerdTarget);
                IsSpawnedEnemy = true;
            }

            var spawn = mapSection.GetSpawnSpot(this);
            X = spawn.X;
            Y = spawn.Y;
        }

        protected Enemy(EnemyTypes type, EnemyConstants.EnemyProperties properties, string file, int x, int y, bool isSpawned) : base(file, true)
        {
            Initialize(type, properties, file);

            X = x;
            Y = y;
            IsSpawnedEnemy = isSpawned;
        }

        private void Initialize(EnemyTypes type, EnemyConstants.EnemyProperties properties, string file)
        {
            Type = type;
            MaxHealth = Helpers.RandomInRange(properties.MinHealth, properties.MaxHealth) * DifficultyConstants.HealthFactor *
                     (1 + Level.CurrentLevel * GameplayConstants.HealthFactorPerLevel);
            Health = MaxHealth;
            MaxMana = Helpers.RandomInRange(properties.MinMana, properties.MaxMana) * DifficultyConstants.ManaFactor;
            Mana = MaxMana;
            HealthRegen = properties.HealthRegen * DifficultyConstants.HealthFactor * (1 + Level.CurrentLevel * GameplayConstants.HealthFactorPerLevel);
            ManaRegen = properties.ManaRegen * DifficultyConstants.ManaFactor;
            BaseSpeed = Helpers.RandomInRange(properties.MinSpeed, properties.MaxSpeed) * DifficultyConstants.SpeedFactor;
            CriticalChance = properties.CritChance;
        }

        protected bool OnDeath()
        {
            LootHelper.TryCreatePotion(this);
            LootHelper.TryCreateTreasureChest(this);
            LootHelper.TryCreateKey(this);
            return true;
        }

        public override Animation GetCurrentAnimation()
        {
            return GetAI().GetCurrentAnimation();
        }

        public abstract AI.AI GetAI();

        protected bool OnDeathFinished()
        {
            return IsActive = false;
        }

        /// <summary>
        /// The minimum worth of an enemy we can create
        /// </summary>
        public static int MinWorth = 5;
        public int Worth()
        {
            switch (Type)
            {
                case EnemyTypes.Zombie:
                    return EnemyConstants.ZombieWorth;
                case EnemyTypes.Dog:
                    return EnemyConstants.DogWorth;
                case EnemyTypes.Demon:
                    return EnemyConstants.DemonWorth;
                case EnemyTypes.SkeletonKing:
                    return EnemyConstants.SkeletonKingWorth;
                case EnemyTypes.FrostDemon:
                    return EnemyConstants.FrostDemonWorth;
                case EnemyTypes.Archer:
                    return EnemyConstants.ArcherWorth;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
