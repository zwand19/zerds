using System;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Zerds.Buffs;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Input;
using Zerds.Menus;

namespace Zerds.GameObjects
{
    public class Player
    {
        public string Name { get; set; }
        public bool IsPlaying { get; set; }
        public PlayerIndex PlayerIndex { get; set; }
        public Entities.Zerd Zerd { get; set; }
        public int FloatingSkillPoints { get; set; }
        public PlayerSkills Skills { get; set; }

        public Player(string name, PlayerIndex playerIndex)
        {
            Name = name;
            PlayerIndex = playerIndex;
            Skills = new PlayerSkills(playerIndex);
        }

        public void Update(GameTime gameTime)
        {
            if (Zerd == null) return;
            var controller = ControllerService.Controllers[PlayerIndex];
            Zerd.ControllerUpdate(controller.LeftTrigger, controller.RightTrigger, controller.LeftStickDirection, controller.RightStickDirection);
            var buttonsPressed = ControllerService.Controllers[PlayerIndex].ButtonsPressed;
            if (buttonsPressed.Contains(Buttons.RightShoulder))
                Zerd.Abilities.First(a => a.Type == AbilityTypes.Dash).Cast();
            if (buttonsPressed.Contains(Buttons.A))
                Zerd.Abilities.First(a => a.Type == AbilityTypes.Wand).Cast();
            if (buttonsPressed.Contains(Buttons.B))
                Zerd.Abilities.First(a => a.Type == AbilityTypes.Iceball).Cast();
            if (buttonsPressed.Contains(Buttons.Y))
                Zerd.Abilities.First(a => a.Type == AbilityTypes.Fireball).Cast();
            if (buttonsPressed.Contains(Buttons.X))
                Zerd.Abilities.FirstOrDefault(a => a.Type == AbilityTypes.LavaBlast)?.Cast();
            if (ControllerService.Controllers[PlayerIndex].RightTrigger > CodingConstants.TriggerPress && Zerd.Mana > 1)
            {
                Zerd.Mana -= AbilityConstants.SprintManaPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds * (1 - Skills.FireMastery * SkillConstants.SprintStat / 100);
                if (!Zerd.Buffs.Any(b => b is SprintBuff))
                    Zerd.Buffs.Add(new SprintBuff(Zerd));
            }
            else
            {
                if (Zerd.Buffs.Any(b => b is SprintBuff))
                    Zerd.Buffs.Remove(Zerd.Buffs.First(b => b is SprintBuff));
            }
        }

        public void JoinGame(ZerdTypes zerdType)
        {
            if (Zerd != null) return;

            IsPlaying = true;
            string zerdFile;
            switch (zerdType)
            {
                case ZerdTypes.Black:
                    zerdFile = "Entities/Zerd-Black.png";
                    break;
                case ZerdTypes.Blue:
                    zerdFile = "Entities/Zerd-Blue.png";
                    break;
                case ZerdTypes.Brown:
                    zerdFile = "Entities/Zerd-Brown.png";
                    break;
                case ZerdTypes.Cyan:
                    zerdFile = "Entities/Zerd-Cyan.png";
                    break;
                case ZerdTypes.Red:
                    zerdFile = "Entities/Zerd-Red.png";
                    break;
                default:
                    throw new Exception("Unknown Zerd Type");
            }
            Zerd = new Entities.Zerd(PlayerIndex, zerdFile);
            Globals.GameState.Zerds.Add(Zerd);
        }
    }
}
