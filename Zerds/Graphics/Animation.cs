using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Constants;

namespace Zerds.Graphics
{
    public class Animation
    {
        private readonly List<AnimationFrame> _frames = new List<AnimationFrame>();
        private TimeSpan _timeIntoAnimation;
        private AnimationFrame _lastFrame;
        public string Name { get; set; }
        private int _width;
        private int _height;
        
        public Animation(string name, BodyPart bodyPart)
        {
            _width = bodyPart.Width;
            _height = bodyPart.Height;
            Name = name;
        }

        public Animation(string name, int width, int height)
        {
            _width = width;
            _height = height;
            Name = name;
        }

        public Animation(string name)
        {
            Name = name;
        }

        private TimeSpan Duration
        {
            get
            {
                var totalSeconds = _frames.Sum(frame => frame.Duration.TotalSeconds);

                return TimeSpan.FromSeconds(totalSeconds);
            }
        }

        /// <summary>
        /// Add a frame with a given row/col within the sprite sheet. All frames must be the same size for this to work.
        /// </summary>
        /// <param name="x">Column in the sprite sheet.</param>
        /// <param name="y">Row in the sprite sheet.</param>
        /// <param name="duration">Duration of this frame.</param>
        /// <param name="func">Function to run when this frame is active.</param>
        public void AddFrame(int x, int y, TimeSpan duration, Func<bool> func = null)
        {
            var rectangle = new Rectangle(x * _width, y * _height, _width, _height);
            _frames.Add(new AnimationFrame { SourceRectangle = rectangle, Duration = duration, StartFunc = func, Origin = new Vector2(rectangle.Width / 2f, rectangle.Height / 2f) });
        }

        public void AddFrame(Rectangle rectangle, TimeSpan duration, Func<bool> func = null, Vector2? origin = null)
        {
            _frames.Add(new AnimationFrame {SourceRectangle = rectangle, Duration = duration, StartFunc = func, Origin = origin ?? new Vector2(rectangle.Width / 2f, rectangle.Height / 2f)});
        }

        public void Update(GameTime gameTime)
        {
            var secondsIntoAnimation = _timeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed;

            var remainder = Math.Abs(Duration.TotalSeconds) < CodingConstants.Tolerance ? 0 : secondsIntoAnimation % Duration.TotalSeconds;

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

        public Vector2 CurrentOrigin => GetCurrentAnimationFrame().Origin;

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