using System.Collections.Generic;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.GameObjects.Buffs;

namespace Zerds.GameObjects
{
    public class DamageInstance
    {
        public Knockback Knockback { get; set; }
        public float Damage { get; set; }
        public bool IsCritical { get; set; }
        public DamageType DamageType { get; set; }
        public Being Creator { get; set; }
    }
}
