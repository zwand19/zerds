using System;
using Zerds.Constants;
using Zerds.GameObjects;
using Zerds.Graphics;
using Zerds.Consumables;

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

        protected Enemy(EnemyConstants.EnemyProperties properties, string file, bool randomSpawn) : base(file, true)
        {
            MaxHealth = Helpers.RandomInRange(properties.MinHealth, properties.MaxHealth) * DifficultyConstants.HealthFactor *
                     (1 + Level.CurrentLevel * GameplayConstants.HealthFactorPerLevel);
            Health = MaxHealth;
            MaxMana = Helpers.RandomInRange(properties.MinMana, properties.MaxMana) * DifficultyConstants.ManaFactor;
            Mana = MaxMana;
            HealthRegen = properties.HealthRegen * DifficultyConstants.HealthFactor * (1 + Level.CurrentLevel * GameplayConstants.HealthFactorPerLevel);
            ManaRegen = properties.ManaRegen * DifficultyConstants.ManaFactor;
            BaseSpeed = Helpers.RandomInRange(properties.MinSpeed, properties.MaxSpeed) * DifficultyConstants.SpeedFactor;
            CriticalChance = properties.CritChance;
            var random = new Random();
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
            var chance = GameplayConstants.PotionDropChance;
            if (Killer is Zerd)
                chance += Killer.SkillValue(SkillType.Guzzler) / 100f;
            if (!Helpers.RandomChance(chance))
                return true;

            if (Helpers.RandomChance(0.5f))
                Globals.GameState.Items.Add(new HealthPotion(this));
            else
                Globals.GameState.Items.Add(new ManaPotion(this));
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
    }
}
