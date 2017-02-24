using System;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;

namespace Zerds.Abilities
{
    public class Dash : Ability
    {
        public Vector2 LastPosition { get; set; }
        public float LastDashMilliseconds { get; set; }
        public int LastLevel { get; set; }
        public bool Rewound { get; set; }

        public Dash(Being being) : base(AbilityTypes.Dash, being, AbilityConstants.DashCooldown, 0f, "charging-bull")
        {

        }

        public override void Cast()
        {
            if (Cooldown <= TimeSpan.Zero)
                Execute();
            else if (Being.SkillPoints(SkillType.Rewind) > 0 && !Rewound)
            {
                if (LastLevel != Level.CurrentLevel ||
                    Level.TimeIntoLevel.TotalMilliseconds - LastDashMilliseconds <
                    Being.SkillValue(SkillType.Rewind, false) * 1000)
                {
                    Being.X = LastPosition.X;
                    Being.Y = LastPosition.Y;
                    Rewound = true;
                }
            }
        }


        protected override bool Execute()
        {
            LastDashMilliseconds = (float)Level.TimeIntoLevel.TotalMilliseconds;
            LastLevel = Level.CurrentLevel;
            LastPosition = new Vector2(Being.Position.X, Being.Position.Y);
            Rewound = false;
            Being.AddBuff(BuffTypes.Dash);
            Cooldown = TimeSpan.FromSeconds(FullCooldown.TotalSeconds - Being.SkillValue(SkillType.Dancer, false));
            return true;
        }
    }
}
