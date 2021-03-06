﻿using System;
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
        Rewind,
        DeepBurn,
        FireballExplosion,
        Exposure,
        Sniper,
        ColdWinds,
        BlazingSpeed,
        DragonsBreath,
        BleedFire,
        Maniac,
        Shatter,
        DeepCold,
        IceShield,
        FrostAura,
        Icicle,
        ComboMaster,
        Rage,
        Charm,
        Hardened
    }

    public static class SkillConstants
    {
        public static Dictionary<SkillType, SkillInfo> Values { get; private set; }

        public struct SkillInfo
        {
            public float Stat { get; set; }
            public int MaxPoints { get; set; }
            public int DecimalPlaces { get; set; }

            public SkillInfo(float stat)
            {
                Stat = stat;
                DecimalPlaces = Math.Abs(stat * 10 % 10) < CodingConstants.Tolerance ? 0 : 1;
                MaxPoints = 5;
            }
        }

        public static void Initialize()
        {
            Values = new Dictionary<SkillType, SkillInfo>
            {
                {SkillType.BitterCold, new SkillInfo(13f)},
                {SkillType.ColdExplosion, new SkillInfo(6f)},
                {SkillType.Dancer, new SkillInfo(0.6f)},
                {SkillType.Devastation, new SkillInfo(2.5f)},
                {SkillType.FireMastery, new SkillInfo(3f)},
                {SkillType.FrozenSoul, new SkillInfo(2f)},
                {SkillType.Guzzler, new SkillInfo(2.2f)},
                {SkillType.ImprovedFireball, new SkillInfo(4f)},
                {SkillType.ImprovedIceball, new SkillInfo(4f)},
                {SkillType.ImprovedWand, new SkillInfo(4f)},
                {SkillType.Replenish, new SkillInfo(0.5f)},
                {SkillType.Rewind, new SkillInfo(0.5f)},
                {SkillType.Sprinter, new SkillInfo(5f)},
                {SkillType.Swiftness, new SkillInfo(1.5f)},
                {SkillType.DeepBurn, new SkillInfo(7f)},
                {SkillType.FireballExplosion, new SkillInfo(5f)},
                {SkillType.Exposure, new SkillInfo(2f)},
                {SkillType.Sniper, new SkillInfo(1f)},
                {SkillType.ColdWinds, new SkillInfo(0.4f)},
                {SkillType.BlazingSpeed, new SkillInfo(2f)},
                {SkillType.BleedFire, new SkillInfo(4f)},
                {SkillType.Maniac, new SkillInfo(8f)},
                {SkillType.Shatter, new SkillInfo(5f)},
                {SkillType.DeepCold, new SkillInfo(5f)},
                {SkillType.IceShield, new SkillInfo(0.6f)},
                {SkillType.FrostAura, new SkillInfo(3f)},
                {SkillType.ComboMaster, new SkillInfo(20f)},
                {SkillType.Rage, new SkillInfo(5f)},
                {SkillType.Hardened, new SkillInfo(4f)}
            };
        }
    }
}
