using System.Collections.Generic;

namespace Zerds.Constants
{
    public enum SkillType
    {
        ImprovedFireball,
        FireMastery,
        Devastation,
        ImprovedIceball,
        FrozenSoul,
        ColdExplosion,
        BitterCold,
        LavaBlast,
        FrostPound,
        ImprovedWand,
        Dancer,
        Swiftness,
        Replenish,
        Sprinter,
        Guzzler,
        Rewind
    }

    public static class SkillConstants
    {
        public static Dictionary<SkillType, SkillInfo> Values { get; private set; }

        public struct SkillInfo
        {
            public float Stat { get; set; }
            public int MaxPoints { get; set; }
            public int DecimalPlaces { get; set; }

            public SkillInfo(float stat, int decimalPlaces = 0, int maxPoints = 5)
            {
                Stat = stat;
                DecimalPlaces = decimalPlaces;
                MaxPoints = maxPoints;
            }
        }

        public static void Initialize()
        {
            Values = new Dictionary<SkillType, SkillInfo>
            {
                {SkillType.BitterCold, new SkillInfo(13f)},
                {SkillType.ColdExplosion, new SkillInfo(6f)},
                {SkillType.Dancer, new SkillInfo(0.5f, 1)},
                {SkillType.Devastation, new SkillInfo(2.5f, 1)},
                {SkillType.FireMastery, new SkillInfo(3f)},
                {SkillType.FrozenSoul, new SkillInfo(2f)},
                {SkillType.Guzzler, new SkillInfo(2.2f, 1)},
                {SkillType.ImprovedFireball, new SkillInfo(4f)},
                {SkillType.ImprovedIceball, new SkillInfo(4f)},
                {SkillType.ImprovedWand, new SkillInfo(4f)},
                {SkillType.Replenish, new SkillInfo(0.5f, 1)},
                {SkillType.Rewind, new SkillInfo(0.25f, 2)},
                {SkillType.Sprinter, new SkillInfo(5f)},
                {SkillType.Swiftness, new SkillInfo(1.5f, 1)}
            };
        }
    }
}
