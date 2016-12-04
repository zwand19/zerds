using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Zerds.Graphics
{
    public class Animation
    {
        List<AnimationFrame> frames = new List<AnimationFrame>();
        TimeSpan timeIntoAnimation;
        AnimationFrame lastFrame;
        public string Name { get; set; }

        TimeSpan Duration
        {
            get
            {
                double totalSeconds = 0;
                foreach (var frame in frames)
                {
                    totalSeconds += frame.Duration.TotalSeconds;
                }

                return TimeSpan.FromSeconds(totalSeconds);
            }
        }

        public Animation(string name)
        {
            Name = name;
        }

        public void AddFrame(Rectangle rectangle, TimeSpan duration, Func<bool> func = null)
        {
            AnimationFrame newFrame = new AnimationFrame
            {
                SourceRectangle = rectangle,
                Duration = duration,
                StartFunc = func
            };

            frames.Add(newFrame);
        }

        public void Update(GameTime gameTime)
        {
            double secondsIntoAnimation =
                timeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;

            double remainder = secondsIntoAnimation % Duration.TotalSeconds;

            timeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }

        public Rectangle CurrentRectangle
        {
            get
            {
                var frame = GetCurrentAnimationFrame();
                if (frame != lastFrame && frame?.StartFunc != null)
                    frame.StartFunc();
                lastFrame = frame;
                return frame.SourceRectangle;
            }
        }

        private AnimationFrame GetCurrentAnimationFrame()
        {
            AnimationFrame currentFrame = null;

            TimeSpan accumulatedTime;
            foreach (var frame in frames)
            {
                if (accumulatedTime + frame.Duration >= timeIntoAnimation)
                {
                    currentFrame = frame;
                    break;
                }
                else
                {
                    accumulatedTime += frame.Duration;
                }
            }

            currentFrame = currentFrame ?? frames.LastOrDefault();

            return currentFrame != null ? currentFrame : new AnimationFrame { SourceRectangle = Rectangle.Empty };
        }
    }
}