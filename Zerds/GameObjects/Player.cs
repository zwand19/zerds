using Microsoft.Xna.Framework;
using Zerds.Services;
using System.Linq;
using Zerds.Enums;

namespace Zerds.GameObjects
{
    public class Player
    {
        public string Name { get; set; }
        public bool IsPlaying { get; set; }
        public PlayerIndex PlayerIndex { get; set; }
        public ControllerService Controller { get; set; }
        public Entities.Zerd Zerd { get; set; }

        public Player(string name, PlayerIndex playerIndex)
        {
            Name = name;
            PlayerIndex = playerIndex;
            Controller = new ControllerService(PlayerIndex);
            Controller.OnStartPressed(JoinGame);
        }

        public void Update(GameTime gameTime)
        {
            Controller.Update(gameTime);
            Zerd?.ControllerUpdate(Controller.LeftTrigger, Controller.RightTrigger, Controller.LeftStickDirection, Controller.RightStickDirection);
        }

        private bool JoinGame()
        {
            if (Zerd != null) return false;

            IsPlaying = true;
            Zerd = new Entities.Zerd(PlayerIndex);
            Globals.GameState.Zerds.Add(Zerd);
            Controller.OnR1Pressed(() => { return Zerd.Abilities.First(a => a.Type == AbilityTypes.Dash).Cast(); });
            Controller.OnR2Pressed(() => { return Zerd.Abilities.First(a => a.Type == AbilityTypes.Sprint).Cast(); });
            Controller.OnAPressed(() => { return Zerd.Abilities.First(a => a.Type == AbilityTypes.Wand).Cast(); });
            Controller.OnBPressed(() => { return Zerd.Abilities.First(a => a.Type == AbilityTypes.Iceball).Cast(); });
            Controller.OnYPressed(() => { return Zerd.Abilities.First(a => a.Type == AbilityTypes.Fireball).Cast(); });
            return true;
        }
    }
}
