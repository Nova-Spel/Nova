using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova_Alpha
{
    public class AnimationEventArgs : EventArgs
    {
        public string finishedAnimation;

        public AnimationEventArgs(string animation)
        {
            finishedAnimation = animation;
        }
    }
}
