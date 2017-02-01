using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zerds.Constants;

namespace Zerds.Input
{
    public static class ControllerService
    {
        public static Dictionary<PlayerIndex, Controller> Controllers { get; set; }

        public static void Initialize()
        {
            Controllers = new Dictionary<PlayerIndex, Controller>
            {
                {PlayerIndex.One, new Controller(PlayerIndex.One)},
                {PlayerIndex.Two, new Controller(PlayerIndex.Two)},
                {PlayerIndex.Three, new Controller(PlayerIndex.Three)},
                {PlayerIndex.Four, new Controller(PlayerIndex.Four)}
            };    
        }

        public static void Update(GameTime gameTime)
        {
            Controllers.Values.ToList().ForEach(c => c.Update(gameTime));
        }

        public static List<Buttons> ButtonsPressed(GamePadState state, GamePadState oldState)
        {
            var buttons = new List<Buttons>();
            if (state.Buttons.A == ButtonState.Pressed && oldState.Buttons.A != ButtonState.Pressed)
                buttons.Add(Buttons.A);
            if (state.Buttons.B == ButtonState.Pressed && oldState.Buttons.B != ButtonState.Pressed)
                buttons.Add(Buttons.B);
            if (state.Buttons.Y == ButtonState.Pressed && oldState.Buttons.Y != ButtonState.Pressed)
                buttons.Add(Buttons.Y);
            if (state.Buttons.X == ButtonState.Pressed && oldState.Buttons.X != ButtonState.Pressed)
                buttons.Add(Buttons.X);
            if (state.Buttons.Start == ButtonState.Pressed && oldState.Buttons.Start != ButtonState.Pressed)
                buttons.Add(Buttons.Start);
            if (state.Buttons.LeftShoulder == ButtonState.Pressed && oldState.Buttons.LeftShoulder != ButtonState.Pressed)
                buttons.Add(Buttons.LeftShoulder);
            if (state.Buttons.RightShoulder == ButtonState.Pressed && oldState.Buttons.RightShoulder != ButtonState.Pressed)
                buttons.Add(Buttons.RightShoulder);
            if (state.Triggers.Left > CodingConstants.TriggerPress && oldState.Triggers.Left < CodingConstants.TriggerPress)
                buttons.Add(Buttons.LeftTrigger);
            if (state.Triggers.Right > CodingConstants.TriggerPress && oldState.Triggers.Right < CodingConstants.TriggerPress)
                buttons.Add(Buttons.RightTrigger);
            if (state.Buttons.LeftStick == ButtonState.Pressed && oldState.Buttons.LeftStick == ButtonState.Released)
                buttons.Add(Buttons.LeftStick);
            if (state.Buttons.RightStick == ButtonState.Pressed && oldState.Buttons.RightStick == ButtonState.Released)
                buttons.Add(Buttons.RightStick);
            if (state.ThumbSticks.Left.Y < -CodingConstants.JoystickPress && oldState.ThumbSticks.Left.Y > -CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickDown);
            if (state.ThumbSticks.Left.Y > CodingConstants.JoystickPress && oldState.ThumbSticks.Left.Y < CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickUp);
            if (state.ThumbSticks.Left.X < -CodingConstants.JoystickPress && oldState.ThumbSticks.Left.X > -CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickLeft);
            if (state.ThumbSticks.Left.X > CodingConstants.JoystickPress && oldState.ThumbSticks.Left.X < CodingConstants.JoystickPress)
                buttons.Add(Buttons.LeftThumbstickRight);
            if (state.ThumbSticks.Right.Y < -CodingConstants.JoystickPress && oldState.ThumbSticks.Right.Y > -CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickDown);
            if (state.ThumbSticks.Right.Y > CodingConstants.JoystickPress && oldState.ThumbSticks.Right.Y < CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickUp);
            if (state.ThumbSticks.Right.X < -CodingConstants.JoystickPress && oldState.ThumbSticks.Right.X > -CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickLeft);
            if (state.ThumbSticks.Right.X > CodingConstants.JoystickPress && oldState.ThumbSticks.Right.X < CodingConstants.JoystickPress)
                buttons.Add(Buttons.RightThumbstickRight);
            if (state.DPad.Down == ButtonState.Pressed && oldState.DPad.Down == ButtonState.Released)
                buttons.Add(Buttons.DPadDown);
            if (state.DPad.Left == ButtonState.Pressed && oldState.DPad.Left == ButtonState.Released)
                buttons.Add(Buttons.DPadLeft);
            if (state.DPad.Right == ButtonState.Pressed && oldState.DPad.Right == ButtonState.Released)
                buttons.Add(Buttons.DPadRight);
            if (state.DPad.Up == ButtonState.Pressed && oldState.DPad.Up == ButtonState.Released)
                buttons.Add(Buttons.DPadUp);
            return buttons;
        }

        public static bool ButtonPressed(PlayerIndex playerIndex, Buttons start)
        {
            return Controllers[playerIndex].ButtonsPressed.Contains(Buttons.Start);
        }
    }
}
