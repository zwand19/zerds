using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Zerds.Input
{
    public class Controller
    {
        public PlayerIndex PlayerIndex { get; set; }
        public GamePadState LastState { get; set; }
        public List<Buttons> ButtonsPressed { get; set; }
        public Vector2 LeftStickDirection { get; set; }
        public Vector2 RightStickDirection { get; set; }
        public float LeftTrigger { get; set; }
        public float RightTrigger { get; set; }

        private TimeSpan _remainingVibration;

        public Controller(PlayerIndex index)
        {
            LeftStickDirection = new Vector2(0, 0);
            RightStickDirection = new Vector2(0, 0);
            PlayerIndex = index;
        }
        
        public void Update(GameTime gameTime)
        {
            var gamePadState = GamePad.GetState(PlayerIndex);
            _remainingVibration -= gameTime.ElapsedGameTime;
            if (_remainingVibration < new TimeSpan(0, 0, 0))
                GamePad.SetVibration(PlayerIndex, 0, 0);
            if (gamePadState.IsConnected)
            {
                ButtonsPressed = ControllerService.ButtonsPressed(gamePadState, LastState);
                LastState = gamePadState;
                LeftStickDirection = new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
                RightStickDirection = new Vector2(gamePadState.ThumbSticks.Right.X, gamePadState.ThumbSticks.Right.Y);
                LeftTrigger = gamePadState.Triggers.Left;
                RightTrigger = gamePadState.Triggers.Right;
            }
            else
            {
                ButtonsPressed = new List<Buttons>();
            }
        }

        public void VibrateController(TimeSpan timeSpan, float intensity = 0.5f)
        {
            if (timeSpan > _remainingVibration)
            {
                _remainingVibration = timeSpan;
                //GamePad.SetVibration(PlayerIndex, intensity, intensity);
            }
        }
    }
}
