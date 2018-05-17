using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zerds.Constants;
using System;

namespace Zerds.Input
{
    public class Keyboard : InputDevice
    {
        public KeyboardState LastState { get; set; }

        public Keyboard(PlayerIndex index) : base(index)
        {
            LeftStickDirection = new Vector2(0, 0);
            RightStickDirection = new Vector2(0, 0);
        }
        public bool IsConnected => GamePad.GetState(PlayerIndex).IsConnected;

        public override void Update(GameTime gameTime)
        {
            var state = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            ButtonsPressed = GetButtonsPressed(state);
            ButtonsReleased = GetButtonsReleased(state);
            LastState = state;
            LeftStickDirection = new Vector2(
                state.IsKeyDown(Keys.Left) ? -1f : state.IsKeyDown(Keys.Right) ? 1f : 0f,
                state.IsKeyDown(Keys.Down) ? -1f : state.IsKeyDown(Keys.Up) ? 1f : 0f);
            RightStickDirection = LeftStickDirection;
            if (state.IsKeyDown(Keys.LeftShift))
                RightStickDirection = RightStickDirection.Rotate(180);
            LeftTrigger = state.IsKeyDown(Keys.A) ? 1f : 0;
            RightTrigger = state.IsKeyDown(Keys.S) ? 1f : 0;
            LeftStickIn = state.IsKeyDown(Keys.Z);
        }
        

        private List<Buttons> GetButtonsPressed(KeyboardState state)
        {
            var buttons = new List<Buttons>();
            // QWER = ABXY
            if (state.IsKeyDown(Keys.Q) && LastState.IsKeyUp(Keys.Q))
                buttons.Add(Buttons.A);
            if (state.IsKeyDown(Keys.E) && LastState.IsKeyUp(Keys.E))
                buttons.Add(Buttons.B);
            if (state.IsKeyDown(Keys.W) && LastState.IsKeyUp(Keys.W))
                buttons.Add(Buttons.X);
            if (state.IsKeyDown(Keys.R) && LastState.IsKeyUp(Keys.R))
                buttons.Add(Buttons.Y);
            // Enter = A
            if (state.IsKeyDown(Keys.Enter) && LastState.IsKeyUp(Keys.Enter))
                buttons.Add(Buttons.A);
            // Space = Start
            if (state.IsKeyDown(Keys.Space) && LastState.IsKeyUp(Keys.Space))
                buttons.Add(Buttons.Start);
            // ASDF = bumpers/triggers
            if (state.IsKeyDown(Keys.A) && LastState.IsKeyUp(Keys.A))
                buttons.Add(Buttons.LeftShoulder);
            if (state.IsKeyDown(Keys.S) && LastState.IsKeyUp(Keys.S))
                buttons.Add(Buttons.LeftTrigger);
            if (state.IsKeyDown(Keys.D) && LastState.IsKeyUp(Keys.D))
                buttons.Add(Buttons.RightShoulder);
            if (state.IsKeyDown(Keys.F) && LastState.IsKeyUp(Keys.F))
                buttons.Add(Buttons.RightTrigger);
            // ZX = left/right stick
            if (state.IsKeyDown(Keys.Z) && LastState.IsKeyUp(Keys.Z))
                buttons.Add(Buttons.LeftStick);
            if (state.IsKeyDown(Keys.X) && LastState.IsKeyUp(Keys.X))
                buttons.Add(Buttons.RightStick);
            // Arrow Keys = Left thumbstick
            if (state.IsKeyDown(Keys.Left) && LastState.IsKeyUp(Keys.Left))
                buttons.Add(Buttons.LeftThumbstickLeft);
            if (state.IsKeyDown(Keys.Up) && LastState.IsKeyUp(Keys.Up))
                buttons.Add(Buttons.LeftThumbstickUp);
            if (state.IsKeyDown(Keys.Right) && LastState.IsKeyUp(Keys.Right))
                buttons.Add(Buttons.LeftThumbstickRight);
            if (state.IsKeyDown(Keys.Down) && LastState.IsKeyUp(Keys.Down))
                buttons.Add(Buttons.LeftThumbstickDown);
            // IJKL = D Pad
            if (state.IsKeyDown(Keys.K) && LastState.IsKeyUp(Keys.K))
                buttons.Add(Buttons.DPadDown);
            if (state.IsKeyDown(Keys.J) && LastState.IsKeyUp(Keys.J))
                buttons.Add(Buttons.DPadLeft);
            if (state.IsKeyDown(Keys.I) && LastState.IsKeyUp(Keys.I))
                buttons.Add(Buttons.DPadUp);
            if (state.IsKeyDown(Keys.L) && LastState.IsKeyUp(Keys.L))
                buttons.Add(Buttons.DPadRight);
            return buttons;
        }

        private List<Buttons> GetButtonsReleased(KeyboardState state)
        {
            var buttons = new List<Buttons>();
            // QWER = ABXY
            if (state.IsKeyUp(Keys.Q) && LastState.IsKeyDown(Keys.Q))
                buttons.Add(Buttons.A);
            if (state.IsKeyUp(Keys.W) && LastState.IsKeyDown(Keys.W))
                buttons.Add(Buttons.B);
            if (state.IsKeyUp(Keys.E) && LastState.IsKeyDown(Keys.E))
                buttons.Add(Buttons.X);
            if (state.IsKeyUp(Keys.R) && LastState.IsKeyDown(Keys.R))
                buttons.Add(Buttons.Y);
            // Enter = A
            if (state.IsKeyUp(Keys.Enter) && LastState.IsKeyDown(Keys.Enter))
                buttons.Add(Buttons.A);
            // Space = Start
            if (state.IsKeyUp(Keys.Space) && LastState.IsKeyDown(Keys.Space))
                buttons.Add(Buttons.Start);
            // ASDF = bumpers/triggers
            if (state.IsKeyUp(Keys.A) && LastState.IsKeyDown(Keys.A))
                buttons.Add(Buttons.LeftShoulder);
            if (state.IsKeyUp(Keys.S) && LastState.IsKeyDown(Keys.S))
                buttons.Add(Buttons.LeftTrigger);
            if (state.IsKeyUp(Keys.D) && LastState.IsKeyDown(Keys.D))
                buttons.Add(Buttons.RightShoulder);
            if (state.IsKeyUp(Keys.F) && LastState.IsKeyDown(Keys.F))
                buttons.Add(Buttons.RightThumbstickDown);
            // ZX = left/right stick
            if (state.IsKeyUp(Keys.Z) && LastState.IsKeyDown(Keys.Z))
                buttons.Add(Buttons.LeftStick);
            if (state.IsKeyUp(Keys.X) && LastState.IsKeyDown(Keys.X))
                buttons.Add(Buttons.RightStick);
            // Arrow Keys = Left thumbstick
            if (state.IsKeyUp(Keys.Left) && LastState.IsKeyDown(Keys.Left))
                buttons.Add(Buttons.LeftThumbstickLeft);
            if (state.IsKeyUp(Keys.Up) && LastState.IsKeyDown(Keys.Up))
                buttons.Add(Buttons.LeftThumbstickUp);
            if (state.IsKeyUp(Keys.Right) && LastState.IsKeyDown(Keys.Right))
                buttons.Add(Buttons.LeftThumbstickRight);
            if (state.IsKeyUp(Keys.Down) && LastState.IsKeyDown(Keys.Down))
                buttons.Add(Buttons.LeftThumbstickDown);
            // IJKL = D Pad
            if (state.IsKeyUp(Keys.K) && LastState.IsKeyDown(Keys.K))
                buttons.Add(Buttons.DPadDown);
            if (state.IsKeyUp(Keys.J) && LastState.IsKeyDown(Keys.J))
                buttons.Add(Buttons.DPadLeft);
            if (state.IsKeyUp(Keys.I) && LastState.IsKeyDown(Keys.I))
                buttons.Add(Buttons.DPadUp);
            if (state.IsKeyUp(Keys.L) && LastState.IsKeyDown(Keys.L))
                buttons.Add(Buttons.DPadRight);
            return buttons;
        }
    }
}
