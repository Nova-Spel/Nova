using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using System.IO;

namespace Nova_Alpha
{
    /// <summary>
    /// Contains all data needed for animation
    /// </summary>
    public class AnimationManager
    {
        private Dictionary<string, AnimationFrames> data;
        public AnimationFrames GetCurrentAnimFrame
        {
            get { return data[currentAnimation]; }
        }

        int xSprites;
        int ySprites;
        int currentFrame;

        bool frozen;
        public bool IsFrozen { get { return frozen; } }

        float currentFrameTime;

        Rectangle sourceRect;
        public Rectangle GetSourceRect { get { return sourceRect; } }

        Point spriteDimensions;

        string currentAnimation;
        public string GetCurrentAnimation { get { return currentAnimation; } }

        public event EventHandler<AnimationEventArgs> OnFinishedAnimation;
        public event EventHandler<AnimationEventArgs> OnLastFrame;

        /// <summary>
        /// Creates a new class containing all data needed for animation
        /// </summary>
        /// <param name="currentAnimation">The animation to start out at</param>
        /// <param name="spriteDimensions">The dimensions of the sprite</param>
        /// <param name="xSprites">The amount of sprites there are on the sprite sheet on the x-axis</param>
        /// <param name="ySprites">The amount of sprites there are on the sprite sheet on the y-axis</param>
        /// <param name="animationName">The name of the animation to pair with the animationFrame at the same index</param>
        /// <param name="animationFrame"></param>
        public AnimationManager(string currentAnimation, Point spriteDimensions, int xSprites, int ySprites, string[] animationName, AnimationFrames[] animationFrame)
        {
            this.currentAnimation = currentAnimation;
            this.spriteDimensions = spriteDimensions;
            this.xSprites = xSprites;
            this.ySprites = ySprites;

            data = new Dictionary<string, AnimationFrames>();

            for (int i = 0; i < animationName.Length; i++)
                data.Add(animationName[i], animationFrame[i]);

            currentFrame = GetCurrentAnimFrame.fromFrame;
            currentFrameTime = 0.0f;

            UpdateSourceRect();
        }

        /// <summary>
        /// To be called when an animation reaches its final frame
        /// </summary>
        protected void AnimationFinished()
        {
            if (OnFinishedAnimation != null)
                OnFinishedAnimation(this, new AnimationEventArgs(currentAnimation));
        }

        protected void LastFrame()
        {
            if (OnLastFrame != null)
                OnLastFrame(this, new AnimationEventArgs(currentAnimation));
        }

        /// <summary>
        /// Changes the current animation
        /// </summary>
        /// <param name="newAnimName">The name of the new animation to use</param>
        /// <param name="force">Override priority?</param>
        public void SetAnimation(string newAnimName, bool force)
        {
            if (newAnimName != currentAnimation)
            {
                if (!force)
                {
                    if (GetCurrentAnimFrame.priority <= data[newAnimName].priority ||
                        (currentFrame == GetCurrentAnimFrame.toFrame))
                    {
                        currentAnimation = newAnimName;
                        currentFrame = GetCurrentAnimFrame.fromFrame;

                        UpdateSourceRect();
                    }
                }
                else
                {
                    currentAnimation = newAnimName;
                    currentFrame = GetCurrentAnimFrame.fromFrame;

                    UpdateSourceRect();
                }
            }
        }

        /// <summary>
        /// Updates sourceRect as needed
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (!frozen)
            {
                currentFrameTime += gameTime.ElapsedGameTime.Milliseconds;

                if (currentFrameTime > GetCurrentAnimFrame.frameTime)
                {
                    IncreaseFrame();

                    currentFrameTime = 0f;
                }
            }
        }

        /// <summary>
        /// Updates sourceRect as needed
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(float delta)
        {
            if (!frozen)
            {
                currentFrameTime += delta * 1000.0f;

                if (currentFrameTime > GetCurrentAnimFrame.frameTime)
                {
                    IncreaseFrame();

                    currentFrameTime = 0f;
                }
            }
        }

        /// <summary>
        /// Increases the current frame and updates sourceRect
        /// </summary>
        private void IncreaseFrame()
        {
            currentFrame++;

            if (currentFrame > GetCurrentAnimFrame.toFrame)
            {
                currentFrame = GetCurrentAnimFrame.fromFrame;
                AnimationFinished();
            }

            UpdateSourceRect();
        }

        public void StepBack()
        {
            currentFrame--;

            if (currentFrame < GetCurrentAnimFrame.fromFrame)
                currentFrame = GetCurrentAnimFrame.toFrame;

            UpdateSourceRect();
        }

        /// <summary>
        /// Updates the source rect to the current frame
        /// </summary>
        private void UpdateSourceRect()
        {
            sourceRect = new Rectangle(currentFrame % xSprites * spriteDimensions.X, (int)Math.Floor((float)currentFrame / xSprites) * spriteDimensions.Y, spriteDimensions.X, spriteDimensions.Y);
        }

        public void Freeze()
        {
            frozen = true;
        }

        public void UnFreeze()
        {
            frozen = false;
        }

        public static AnimationManager CreateFromFile(string path)
        {
            string currentAnimation = string.Empty;

            Point spriteDimensions = Point.Zero;

            int xSprites = 0;
            int ySprites = 0;

            List<string> animations = new List<string>();
            List<AnimationFrames> animationFrames = new List<AnimationFrames>();

            using (XmlReader reader = XmlReader.Create(new StreamReader(path)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "SheetSize":
                                spriteDimensions = new Point(int.Parse(reader.GetAttribute(0)), int.Parse(reader.GetAttribute(1)));
                                break;
                            case "Frames":
                                xSprites = int.Parse(reader.GetAttribute(0));
                                ySprites = int.Parse(reader.GetAttribute(1));
                                break;
                            case "NewAnimation":
                                animationFrames.Add(new AnimationFrames(int.Parse(reader.GetAttribute(2)), int.Parse(reader.GetAttribute(3)), float.Parse(reader.GetAttribute(4)), int.Parse(reader.GetAttribute(5))));
                                animations.Add(reader.GetAttribute(0));

                                if(bool.Parse(reader.GetAttribute(1)))
                                    currentAnimation = reader.GetAttribute(0);
                                break;
                        }
                    }
                }
            }

            return new AnimationManager(currentAnimation, spriteDimensions, xSprites, ySprites, animations.ToArray(), animationFrames.ToArray());
        }
    }
}
