using System;
using System.Linq;
using Zerds.Buffs;
using Zerds.Constants;
using Zerds.GameObjects;
using Zerds.Graphics;
using Zerds.Consumables;
using Zerds.Enums;
using Zerds.Items;

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

        protected Enemy(EnemyTypes type, EnemyConstants.EnemyProperties properties, string file, bool randomSpawn) : base(file, true)
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
            var random = Globals.Random;
            if (randomSpawn)
            {
                X = random.Next(Globals.ViewportBounds.Width);
                Y = random.Next(Globals.ViewportBounds.Height);
            }
            else
            {
                var side = random.Next(4);
                switch (side)
                {
                    case 0:
                        X = random.Next(Globals.ViewportBounds.Width);
                        Y = -50;
                        return;
                    case 1:
                        X = random.Next(Globals.ViewportBounds.Width);
                        Y = Globals.ViewportBounds.Height + 50;
                        return;
                    case 2:
                        X = -50;
                        Y = random.Next(Globals.ViewportBounds.Height);
                        return;
                    case 3:
                        X = Globals.ViewportBounds.Width + 50;
                        Y = random.Next(Globals.ViewportBounds.Height);
                        return;
                }
            }
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

        public int Worth()
        {
            switch (Type)
            {
                case EnemyTypes.Zombie:
                    return 5;
                case EnemyTypes.Dog:
                    return 7;
                case EnemyTypes.Demon:
                    return 17;
                case EnemyTypes.SkeletonKing:
                    return 50;
                case EnemyTypes.FrostDemon:
                    return 17;
                case EnemyTypes.Archer:
                    return 8;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
