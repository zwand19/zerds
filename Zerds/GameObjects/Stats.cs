using System.Collections.Generic;
using Zerds.Entities;
using Zerds.Enums;

namespace Zerds.GameObjects
{
    public class Stats
    {
        public List<Enemy> LevelKillingBlows { get; set; }
        public int Combo { get; set; }
        public int MaxCombo { get; set; }
        public int MaxLevelCombo { get; set; }
        public int KillingBlows { get; set; }
        public float DamageDealt { get; set; }
        public float DamageTaken { get; set; }
        public int GoldEarned { get; set; }
        public int AbilityUpgradesPurchased { get; set; }
        public int SkillPointsSpent { get; set; }
        public bool CleanRound { get; set; }
        public bool NoMisses { get; set; }

        public void ResetLevel()
        {
            MaxLevelCombo = 0;
            LevelKillingBlows = new List<Enemy>();
            CleanRound = true;
            NoMisses = true;
        }

        public bool PerfectRound => CleanRound && NoMisses && MaxLevelCombo >= 10;
        public bool NoMissRound => NoMisses && MaxLevelCombo >= 10;

        public void IncreaseCombo()
        {
            Combo++;
            MaxCombo = Combo > MaxCombo ? Combo : MaxCombo;
            MaxLevelCombo = Combo > MaxLevelCombo ? Combo : MaxLevelCombo;
        }

        public void EnemyKilled(Enemy enemy)
        {
            KillingBlows++;
            LevelKillingBlows.Add(enemy);
        }

        public void GameOver()
        {
            KillingBlows += LevelKillingBlows.Count;
            LevelKillingBlows = new List<Enemy>();
        }

        public void DealtDamage(DamageInstance damageInstance)
        {
            if (damageInstance.DamageType == DamageTypes.Unknown) return;
            DamageDealt += damageInstance.Damage;
        }

        public void TookDamage(DamageInstance damageInstance)
        {
            if (damageInstance.DamageType == DamageTypes.Unknown) return;
            DamageTaken += damageInstance.Damage;
            CleanRound = false;
        }

        public void Missed()
        {
            NoMisses = false;
            Combo = 0;
        }
    }
}
