using System;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;

namespace Zerds.Abilities
{
    public class Sprint : Ability
    {
        public Sprint(Being being) : base(AbilityTypes.Sprint, being, TimeSpan.Zero, TimeSpan.Zero, 0f, "")
        {

        }

        protected override bool Execute()
        {
            Being.AddBuff(BuffTypes.Sprint);
            return base.Execute();
        }
    }
}
