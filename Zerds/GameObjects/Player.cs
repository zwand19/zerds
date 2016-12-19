using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Zerds.Enums;
using Zerds.Input;

namespace Zerds.GameObjects
{
    public class Player
    {
        public string Name { get; set; }
        public bool IsPlaying { get; set; }
        public PlayerIndex PlayerIndex { get; set; }
        public Entities.Zerd Zerd { get; set; }

        public Player(string name, PlayerIndex playerIndex)
        {
            Name = name;
            PlayerIndex = playerIndex;
        }

        public void Update(GameTime gameTime)
        {
            if (ControllerService.ButtonPressed(PlayerIndex, Buttons.Start))
                JoinGame();
            var controller = ControllerService.Controllers[PlayerIndex];
            Zerd?.ControllerUpdate(controller.LeftTrigger, controller.RightTrigger, controller.LeftStickDirection, controller.RightStickDirection);
            var buttonsPressed = ControllerService.Controllers[PlayerIndex].ButtonsPressed;
            if (buttonsPressed.Contains(Buttons.RightTrigger))
                Zerd?.Abilities.First(a => a.Type == AbilityTypes.Dash).Cast();
            if (buttonsPressed.Contains(Buttons.RightShoulder))
                Zerd?.Abilities.First(a => a.Type == AbilityTypes.Sprint).Cast();
            if (buttonsPressed.Contains(Buttons.A))
                Zerd?.Abilities.First(a => a.Type == AbilityTypes.Wand).Cast();
            if (buttonsPressed.Contains(Buttons.B))
                Zerd?.Abilities.First(a => a.Type == AbilityTypes.Iceball).Cast();
            if (buttonsPressed.Contains(Buttons.Y))
                Zerd?.Abilities.First(a => a.Type == AbilityTypes.Fireball).Cast();
        }

        public void JoinGame()
        {
            if (Zerd != null) return;

            IsPlaying = true;
            Zerd = new Entities.Zerd(PlayerIndex);
            Globals.GameState.Zerds.Add(Zerd);
        }
    }
}
