using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Zerds.Input
{
    public abstract class InputDevice
    {
        public PlayerIndex PlayerIndex { get; set; }
        public List<Buttons> ButtonsPressed { get; set; }
        public List<Buttons> ButtonsReleased { get; set; }
        public Vector2 LeftStickDirection { get; set; }
        public Vector2 RightStickDirection { get; set; }
        public float LeftTrigger { get; set; }
        public float RightTrigger { get; set; }
        public bool LeftStickIn { get; set; }

        public bool IsPressed(Buttons button) => ButtonsPressed.Contains(button);
        public bool IsReleased(Buttons button) => ButtonsReleased.Contains(button);

        public InputDevice(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }

        public abstract void Update(GameTime gameTime);
        public void Vibrate(TimeSpan timeSpan, float intensity = 0.5f) { }
    }
}
