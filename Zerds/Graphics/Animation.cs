using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Zerds.Graphics
{
    public class Animation
    {
        readonly List<AnimationFrame> _frames = new List<AnimationFrame>();
        TimeSpan _timeIntoAnimation;
        AnimationFrame _lastFrame;
        public string Name { get; set; }

        private TimeSpan Duration
        {
            get
            {
                var totalSeconds = _frames.Sum(frame => frame.Duration.TotalSeconds);

                return TimeSpan.FromSeconds(totalSeconds);
            }
        }

        public Animation(string name)
        {
            Name = name;
        }

        public void AddFrame(Rectangle rectangle, TimeSpan duration, Func<bool> func = null)
        {
            _frames.Add(new AnimationFrame
            {
                SourceRectangle = rectangle,
                Duration = duration,
                StartFunc = func
            });
        }

        public void Update(GameTime gameTime)
        {
            var secondsIntoAnimation = _timeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;

            var remainder = secondsIntoAnimation % Duration.TotalSeconds;

            _timeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }

        public void ResetAnimation()
        {
            _timeIntoAnimation = TimeSpan.Zero;
        }

        public Rectangle CurrentRectangle
        {
            get
            {
                var frame = GetCurrentAnimationFrame();
                if (frame != _lastFrame)
                    frame?.StartFunc?.Invoke();
                _lastFrame = frame;
                return frame.SourceRectangle;
            }
        }

        private AnimationFrame GetCurrentAnimationFrame()
        {
            AnimationFrame currentFrame = null;

            var accumulatedTime = TimeSpan.Zero;
            foreach (var frame in _frames)
            {
                if (accumulatedTime + frame.Duration >= _timeIntoAnimation)
                {
                    currentFrame = frame;
                    break;
                }
                accumulatedTime += frame.Duration;
            }

            currentFrame = currentFrame ?? _frames.LastOrDefault();

            return currentFrame ?? new AnimationFrame { SourceRectangle = Rectangle.Empty };
        }
    }
}