namespace Zerds.Constants
{
    public static class EnemyConstants
    {
        public class EnemyProperties
        {
            public int MinHealth { get; set; }
            public int MinMana { get; set; }
            public int MaxHealth { get; set; }
            public int MaxMana { get; set; }
            public float HealthRegen { get; set; }
            public float ManaRegen { get; set; }
            public float MinSpeed { get; set; }
            public float MaxSpeed { get; set; }
            public float CritChance { get; set; }
        }
        
        public static EnemyProperties GetZombieProperties()
        {
            return new EnemyProperties
            {
                MinHealth = 15,
                HealthRegen = 0.2f,
                MinSpeed = 80.0f,
                MaxHealth = 21,
                MaxSpeed = 120.0f,
                CritChance = 0.06f
            };    
        }

        public static EnemyProperties GetDogProperties()
        {
            return new EnemyProperties
            {
                MinHealth = 6,
                HealthRegen = 0.2f,
                MinSpeed = 235.0f,
                MaxHealth = 11,
                MaxSpeed = 270.0f,
                CritChance = 0.25f
            };
        }

        public static EnemyProperties GetDemonProperties()
        {
            return new EnemyProperties
            {
                MinHealth = 26,
                HealthRegen = 1f,
                MinSpeed = 155.0f,
                MaxHealth = 32,
                MaxSpeed = 200.0f,
                CritChance = 0.12f
            };
        }

        public static EnemyProperties GetFrostDemonProperties()
        {
            return new EnemyProperties
            {
                MinHealth = 26,
                HealthRegen = 1f,
                MinSpeed = 170.0f,
                MaxHealth = 32,
                MaxSpeed = 210.0f,
                CritChance = 0.06f
            };
        }
    }
}
