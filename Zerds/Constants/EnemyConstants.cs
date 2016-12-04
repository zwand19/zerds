using System;

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
        }

        #region Zombie

        public static EnemyProperties GetZombieProperties()
        {
            return new EnemyProperties
            {
                MinHealth = 10,
                HealthRegen = 0.2f,
                MinSpeed = 2.0f
                ,
                MaxHealth = 15,
                MaxSpeed = 3.0f
            };    
        }

        #endregion
    }
}
