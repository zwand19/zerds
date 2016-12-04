using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zerds.Services
{
    public class ControllerService
    {
        public List<Func<bool>> AButtonFunctions { get; set; }
        public List<Func<bool>> BButtonFunctions { get; set; }
        public List<Func<bool>> XButtonFunctions { get; set; }
        public List<Func<bool>> YButtonFunctions { get; set; }
        public List<Func<bool>> L1ButtonFunctions { get; set; }
        public List<Func<bool>> R1ButtonFunctions { get; set; }
        public List<Func<bool>> L2ButtonFunctions { get; set; }
        public List<Func<bool>> R2ButtonFunctions { get; set; }
        public Vector2 LeftStickDirection { get; set; }
        public Vector2 RightStickDirection { get; set; }
        public PlayerIndex PlayerIndex { get; set; }

        private TimeSpan _remainingVibration = new TimeSpan();
        private GamePadState _oldState;
        private static List<ControllerService> _services = new List<ControllerService>();

        public ControllerService(PlayerIndex playerIndex)
        {
            AButtonFunctions = new List<Func<bool>>();
            BButtonFunctions = new List<Func<bool>>();
            XButtonFunctions = new List<Func<bool>>();
            YButtonFunctions = new List<Func<bool>>();
            L1ButtonFunctions = new List<Func<bool>>();
            R1ButtonFunctions = new List<Func<bool>>();
            L2ButtonFunctions = new List<Func<bool>>();
            R2ButtonFunctions = new List<Func<bool>>();
            LeftStickDirection = new Vector2(0, 0);
            RightStickDirection = new Vector2(0, 0);
            PlayerIndex = playerIndex;
            _services.Add(this);
        }

        public static ControllerService GetService(PlayerIndex index)
        {
            return _services.First(s => s.PlayerIndex == index);
        }

        public void OnAPressed(Func<bool> func)
        {
            AButtonFunctions.Add(func);
        }

        public void OnBPressed(Func<bool> func)
        {
            BButtonFunctions.Add(func);
        }

        public void OnXPressed(Func<bool> func)
        {
            XButtonFunctions.Add(func);
        }

        public void OnYPressed(Func<bool> func)
        {
            YButtonFunctions.Add(func);
        }

        public void OnL1Pressed(Func<bool> func)
        {
            L1ButtonFunctions.Add(func);
        }

        public void OnR1Pressed(Func<bool> func)
        {
            R1ButtonFunctions.Add(func);
        }

        public void OnL2Pressed(Func<bool> func)
        {
            L2ButtonFunctions.Add(func);
        }

        public void OnR2Pressed(Func<bool> func)
        {
            R2ButtonFunctions.Add(func);
        }

        public void Update(GameTime gameTime)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex);
            _remainingVibration -= gameTime.ElapsedGameTime;
            if (_remainingVibration < new TimeSpan(0, 0, 0))
                GamePad.SetVibration(PlayerIndex, 0, 0);
            _oldState = _oldState == null ? gamePadState : _oldState;
            if (gamePadState.IsConnected)
            {
                if (gamePadState.Buttons.A == ButtonState.Released && _oldState.Buttons.A == ButtonState.Pressed)
                    AButtonFunctions.ForEach(f => f());
                if (gamePadState.Buttons.B == ButtonState.Released && _oldState.Buttons.B == ButtonState.Pressed)
                    BButtonFunctions.ForEach(f => f());
                if (gamePadState.Buttons.LeftShoulder == ButtonState.Released && _oldState.Buttons.LeftShoulder == ButtonState.Pressed)
                    L1ButtonFunctions.ForEach(f => f());
                if (gamePadState.Buttons.RightShoulder == ButtonState.Released && _oldState.Buttons.RightShoulder == ButtonState.Pressed)
                    R1ButtonFunctions.ForEach(f => f());
                if (gamePadState.Triggers.Left > 0.8f && _oldState.Triggers.Left < 0.8f)
                    L2ButtonFunctions.ForEach(f => f());
                if (gamePadState.Triggers.Right > 0.8f && _oldState.Triggers.Right < 0.8f)
                    R2ButtonFunctions.ForEach(f => f());
                LeftStickDirection = new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
                RightStickDirection = new Vector2(gamePadState.ThumbSticks.Right.X, gamePadState.ThumbSticks.Right.Y);
            }
            _oldState = gamePadState;
        }

        public void VibrateController(TimeSpan timeSpan, float intensity = 0.5f)
        {
            if (timeSpan > _remainingVibration)
            {
                _remainingVibration = timeSpan;
                GamePad.SetVibration(PlayerIndex, intensity, intensity);
            }
        }
    }
}
