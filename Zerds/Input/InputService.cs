using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zerds.Constants;
using System;

namespace Zerds.Input
{
    public static class InputService
    {
        public static Dictionary<PlayerIndex, InputDevice> InputDevices { get; set; }

        public static void Initialize()
        {
            InputDevice playerOne = GamePad.GetState(PlayerIndex.One).IsConnected ? new Controller(PlayerIndex.One) : (InputDevice)(new Keyboard(PlayerIndex.One));
            InputDevices = new Dictionary<PlayerIndex, InputDevice>
            {
                {PlayerIndex.One, playerOne},
                {PlayerIndex.Two, new Controller(PlayerIndex.Two)},
                {PlayerIndex.Three, new Controller(PlayerIndex.Three)},
                {PlayerIndex.Four, new Controller(PlayerIndex.Four)}
            };    
        }

        public static void Update(GameTime gameTime)
        {
            InputDevices.Values.ToList().ForEach(c => c.Update(gameTime));
        }

        public static bool ButtonPressed(PlayerIndex playerIndex, Buttons button)
        {
            return InputDevices[playerIndex].ButtonsPressed.Contains(button);
        }

        public static void Vibrate(PlayerIndex playerIndex, TimeSpan timeSpan, float intensity)
        {
            InputDevices[playerIndex].Vibrate(timeSpan, intensity);
        }
    }
}
