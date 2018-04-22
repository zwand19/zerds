using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zerds.Constants;

namespace Zerds.Input
{
    public class Controller : InputDevice
    {
        public GamePadState LastState { get; set; }
        private TimeSpan _remainingVibration;

        public Controller(PlayerIndex index) : base(index)
        {
            LeftStickDirection = new Vector2(0, 0);
            RightStickDirection = new Vector2(0, 0);
        }
        public bool IsConnected => GamePad.GetState(PlayerIndex).IsConnected;

        public override void Update(GameTime gameTime)
        {
            var gamePadState = GamePad.GetState(PlayerIndex);
            _remainingVibration = _remainingVibration.SubtractWithGameSpeed(gameTime.ElapsedGameTime);
            if (_remainingVibration < new TimeSpan(0, 0, 0))
                GamePad.SetVibration(PlayerIndex, 0, 0);
            if (gamePadState.IsConnected)
            {
                ButtonsPressed = GetButtonsPressed(gamePadState);
                ButtonsReleased = GetButtonsReleased(gamePadState);
                LastState = gamePadState;
                LeftStickDirection = new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
                RightStickDirection = new Vector2(gamePadState.ThumbSticks.Right.X, gamePadState.ThumbSticks.Right.Y);
                LeftTrigger = gamePadState.Triggers.Left;
                RightTrigger = gamePadState.Triggers.Right;
                LeftStickIn = gamePadState.Buttons.LeftStick == ButtonState.Pressed;
            }
            else
            {
                ButtonsPressed = new List<Buttons>();
                ButtonsReleased = new List<Buttons>();
            }
        }

        new public void Vibrate(TimeSpan timeSpan, float intensity = 0.5f)
        {
            if (timeSpan > _remainingVibration)
            {
                _remainingVibration = timeSpan;
                //GamePad.SetVibration(PlayerIndex, intensity, intensity);
            }
        }
        
        private List<Buttons> GetButtonsPressed(GamePadState state)
        {
            var buttons = new List<Buttons>();
            if (state.Buttons.A == ButtonState.Pressed && LastState.Buttons.A != ButtonState.Pressed)
                buttons.Add(Buttons.A);
            if (state.Buttons.B == ButtonState.Pressed && LastState.Buttons.B != ButtonState.Pressed)
                buttons.Add(Buttons.B);
            if (state.Buttons.Y == ButtonState.Pressed && LastState.Buttons.Y != ButtonState.Pressed)
                buttons.Add(Buttons.Y);
            if (state.Buttons.X == ButtonState.Pressed && LastState.Buttons.X != ButtonState.Pressed)
                buttons.Add(Buttons.X);
            if (state.Buttons.Start == ButtonState.Pressed && LastState.Buttons.Start != ButtonState.Pressed)
                buttons.Add(Buttons.Start);
            if (state.Buttons.LeftShoulder == ButtonState.Pressed && LastState.Buttons.LeftShoulder != ButtonState.Pressed)
                buttons.Add(Buttons.LeftShoulder);
            if (state.Buttons.RightShoulder == ButtonState.Pressed && LastState.Buttons.RightShoulder != ButtonState.Pressed)
                buttons.Add(Buttons.RightShoulder);
            if (state.Triggers.Left > CodingConstants.TriggerPress && LastState.Triggers.Left < CodingConstants.TriggerPress)
                buttons.Add(Buttons.LeftTrigger);
            if (state.Triggers.Right > CodingConstants.TriggerPress && LastState.Triggers.Right < CodingConstants.TriggerPress)
                buttons.Add(Buttons.RightTrigger);
            if (state.Buttons.LeftStick == ButtonState.Pressed && LastState.Buttons.LeftStick == ButtonState.Released)
                buttons.Add(Buttons.LeftStick);
            if (state.Buttons.RightStick == ButtonState.Pressed && LastState.Buttons.RightStick == ButtonState.Released)
                buttons.Add(Buttons.RightStick);
            if (state.ThumbSticks.Left.Y < -CodingConstants.JoystickPress && LastState.ThumbSticks.Left.Y > -CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickDown);
            if (state.ThumbSticks.Left.Y > CodingConstants.JoystickPress && LastState.ThumbSticks.Left.Y < CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickUp);
            if (state.ThumbSticks.Left.X < -CodingConstants.JoystickPress && LastState.ThumbSticks.Left.X > -CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickLeft);
            if (state.ThumbSticks.Left.X > CodingConstants.JoystickPress && LastState.ThumbSticks.Left.X < CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickRight);
            if (state.ThumbSticks.Right.Y < -CodingConstants.JoystickPress && LastState.ThumbSticks.Right.Y > -CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickDown);
            if (state.ThumbSticks.Right.Y > CodingConstants.JoystickPress && LastState.ThumbSticks.Right.Y < CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickUp);
            if (state.ThumbSticks.Right.X < -CodingConstants.JoystickPress && LastState.ThumbSticks.Right.X > -CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickLeft);
            if (state.ThumbSticks.Right.X > CodingConstants.JoystickPress && LastState.ThumbSticks.Right.X < CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickRight);
            if (state.DPad.Down == ButtonState.Pressed && LastState.DPad.Down == ButtonState.Released)
                buttons.Add(Buttons.DPadDown);
            if (state.DPad.Left == ButtonState.Pressed && LastState.DPad.Left == ButtonState.Released)
                buttons.Add(Buttons.DPadLeft);
            if (state.DPad.Right == ButtonState.Pressed && LastState.DPad.Right == ButtonState.Released)
                buttons.Add(Buttons.DPadRight);
            if (state.DPad.Up == ButtonState.Pressed && LastState.DPad.Up == ButtonState.Released)
                buttons.Add(Buttons.DPadUp);
            return buttons;
        }

        private List<Buttons> GetButtonsReleased(GamePadState state)
        {
            var buttons = new List<Buttons>();
            if (state.Buttons.A == ButtonState.Released && LastState.Buttons.A != ButtonState.Released)
                buttons.Add(Buttons.A);
            if (state.Buttons.B == ButtonState.Released && LastState.Buttons.B != ButtonState.Released)
                buttons.Add(Buttons.B);
            if (state.Buttons.Y == ButtonState.Released && LastState.Buttons.Y != ButtonState.Released)
                buttons.Add(Buttons.Y);
            if (state.Buttons.X == ButtonState.Released && LastState.Buttons.X != ButtonState.Released)
                buttons.Add(Buttons.X);
            if (state.Buttons.Start == ButtonState.Released && LastState.Buttons.Start != ButtonState.Released)
                buttons.Add(Buttons.Start);
            if (state.Buttons.LeftShoulder == ButtonState.Released && LastState.Buttons.LeftShoulder != ButtonState.Released)
                buttons.Add(Buttons.LeftShoulder);
            if (state.Buttons.RightShoulder == ButtonState.Released && LastState.Buttons.RightShoulder != ButtonState.Released)
                buttons.Add(Buttons.RightShoulder);
            if (state.Triggers.Left > CodingConstants.TriggerPress && LastState.Triggers.Left < CodingConstants.TriggerPress)
                buttons.Add(Buttons.LeftTrigger);
            if (state.Triggers.Right > CodingConstants.TriggerPress && LastState.Triggers.Right < CodingConstants.TriggerPress)
                buttons.Add(Buttons.RightTrigger);
            if (state.Buttons.LeftStick == ButtonState.Released && LastState.Buttons.LeftStick == ButtonState.Pressed)
                buttons.Add(Buttons.LeftStick);
            if (state.Buttons.RightStick == ButtonState.Released && LastState.Buttons.RightStick == ButtonState.Pressed)
                buttons.Add(Buttons.RightStick);
            if (state.ThumbSticks.Left.Y > -CodingConstants.JoystickPress && LastState.ThumbSticks.Left.Y < -CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickDown);
            if (state.ThumbSticks.Left.Y > CodingConstants.JoystickPress && LastState.ThumbSticks.Left.Y > CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickUp);
            if (state.ThumbSticks.Left.X < -CodingConstants.JoystickPress && LastState.ThumbSticks.Left.X < -CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickLeft);
            if (state.ThumbSticks.Left.X < CodingConstants.JoystickPress && LastState.ThumbSticks.Left.X > CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickRight);
            if (state.ThumbSticks.Right.Y > -CodingConstants.JoystickPress && LastState.ThumbSticks.Right.Y < -CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickDown);
            if (state.ThumbSticks.Right.Y < CodingConstants.JoystickPress && LastState.ThumbSticks.Right.Y > CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickUp);
            if (state.ThumbSticks.Right.X > -CodingConstants.JoystickPress && LastState.ThumbSticks.Right.X < -CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickLeft);
            if (state.ThumbSticks.Right.X < CodingConstants.JoystickPress && LastState.ThumbSticks.Right.X > CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickRight);
            if (state.DPad.Down == ButtonState.Released && LastState.DPad.Down == ButtonState.Pressed)
                buttons.Add(Buttons.DPadDown);
            if (state.DPad.Left == ButtonState.Released && LastState.DPad.Left == ButtonState.Released)
                buttons.Add(Buttons.DPadLeft);
            if (state.DPad.Right == ButtonState.Released && LastState.DPad.Right == ButtonState.Pressed)
                buttons.Add(Buttons.DPadRight);
            if (state.DPad.Up == ButtonState.Released && LastState.DPad.Up == ButtonState.Pressed)
                buttons.Add(Buttons.DPadUp);
            return buttons;
        }
    }
}
