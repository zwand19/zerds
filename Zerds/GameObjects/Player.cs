using System;
using Microsoft.Xna.Framework;
using Zerds.Services;

namespace Zerds.GameObjects
{
    public class Player
    {
        public string Name { get; set; }
        public bool IsPlaying { get; set; }
        public PlayerIndex PlayerIndex { get; set; }
        public ControllerService Controller { get; set; }
        public Entities.Zerd Zerd { get; set; }

        public Player(string name, bool isPlaying, PlayerIndex playerIndex)
        {
            Name = name;
            IsPlaying = isPlaying;
            PlayerIndex = playerIndex;
            Controller = new ControllerService(PlayerIndex);
        }

        public void Update(GameTime gameTime)
        {
            Controller.Update(gameTime);
            Zerd.ControllerUpdate(Controller.LeftTrigger, Controller.RightTrigger, Controller.LeftStickDirection, Controller.RightStickDirection);
        }

        public void GameCreated(GameState game)
        {
            if (IsPlaying)
            {
                Zerd = new Entities.Zerd(PlayerIndex);
                game.Zerds.Add(Zerd);
                Controller.OnR1Pressed(Zerd.Dashed);
                Controller.OnR2Pressed(Zerd.Sprinted);
                Controller.OnAPressed(Zerd.WandAttack);
                Controller.OnBPressed(Zerd.FrostAttack);
            }
        }
    }
}
