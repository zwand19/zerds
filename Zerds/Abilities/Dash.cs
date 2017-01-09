﻿using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;

namespace Zerds.Abilities
{
    public class Dash : Ability
    {
        public Dash(Being being) : base(AbilityTypes.Dash, being, AbilityConstants.DashCooldown, 0f, "charging-bull.png")
        {

        }

        protected override bool Execute()
        {
            Being.AddBuff(BuffTypes.Dash);
            Cooldown = TimeSpan.FromSeconds(FullCooldown.TotalSeconds - ((Zerd)Being).Player.Skills.Dancer * SkillConstants.DancerStat);
            return true;
        }
    }
}
