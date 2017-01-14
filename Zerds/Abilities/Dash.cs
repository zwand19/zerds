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

        public Dash(Being being) : base(AbilityTypes.Dash, being, AbilityConstants.DashCooldown, 0f, "charging-bull.png")
        {

        }

        public override void Cast()
        {
            if (Cooldown <= TimeSpan.Zero)
                Execute();
            else if (Being.SkillPoints(SkillType.Rewind) > 0 && !Rewound)
            {
                if (LastLevel != Level.CurrentLevel ||
                    Globals.GameState.TimeIntoLevel.TotalMilliseconds - LastDashMilliseconds <
                    Being.SkillValue(SkillType.Rewind) * 1000)
                {
                    Being.X = LastPosition.X;
                    Being.Y = LastPosition.Y;
                    Rewound = true;
                }
            }
        }


        protected override bool Execute()
        {
            LastDashMilliseconds = (float) Globals.GameState.TimeIntoLevel.TotalMilliseconds;
            LastLevel = Level.CurrentLevel;
            Rewound = false;
            Being.AddBuff(BuffTypes.Dash);
            Cooldown = TimeSpan.FromSeconds(FullCooldown.TotalSeconds - Being.SkillValue(SkillType.Dancer));
            return true;
        }
    }
}
