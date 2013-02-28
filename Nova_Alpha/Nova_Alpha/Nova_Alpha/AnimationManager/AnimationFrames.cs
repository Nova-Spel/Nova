using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova_Alpha
{
    /// <summary>
    /// Used to give the animation system frames for each animation
    /// </summary>
    public class AnimationFrames
    {
        public int fromFrame;
        public int toFrame;

        public float frameTime;

        public int priority;

        /// <summary>
        /// Used to give the animation system frames for each animation
        /// </summary>
        /// <param name="fromFrame">The 0-based index of which frame the current animation starts at</param>
        /// <param name="toFrame">The 0-based index of which frame the current animation finishes at</param>
        /// <param name="frameTime">The time (in milliseconds) each frame should aim for</param>
        public AnimationFrames(int fromFrame, int toFrame, float frameTime, int priority)
        {
            this.fromFrame = fromFrame;
            this.toFrame = toFrame;
            this.frameTime = frameTime;
            this.priority = priority;
        }
    }
}
