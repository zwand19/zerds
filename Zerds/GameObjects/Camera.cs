using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Zerds.Constants;

namespace Zerds.GameObjects
{
    public class Camera
    {
        // If we're offset by this much or less don't adjust
        private const float IgnoreBufferX = 70f;
        private const float IgnoreBufferY = 55f;
        
        public float Zoom { get; set; }
        /// <summary>
        /// The X of the camera represents the center of the screen horizontally
        /// </summary>
        public float X { get; set; }
        /// <summary>
        /// The Y of the camera represents the center of the screen vertically
        /// </summary>
        public float Y { get; set; }

        public float LeftDrawBound => X - Globals.ViewportBounds.Width / 2 - DisplayConstants.CameraBuffer;
        public float RightDrawBound => X + Globals.ViewportBounds.Width / 2 + DisplayConstants.CameraBuffer;
        public float TopDrawBound => Y - Globals.ViewportBounds.Height / 2 - DisplayConstants.CameraBuffer;
        public float BottomDrawBound => Y + Globals.ViewportBounds.Height / 2 + DisplayConstants.CameraBuffer;
        public float ScreenLeft => X - Globals.ViewportBounds.Width / 2;
        public float ScreenTop => Y - Globals.ViewportBounds.Height / 2;


        public Camera()
        {
            Zoom = 1;
            // Arbitrarily start the camera here, off map
            X = -2000;
            Y = -2000;
        }

        public void Update(GameTime gameTime)
        {
            var zerds = Globals.GameState.Zerds;
            if (!zerds.Any())
                return;
            X = zerds.Average(z => z.X);
            Y = zerds.Average(z => z.Y);
        }
    }
}
