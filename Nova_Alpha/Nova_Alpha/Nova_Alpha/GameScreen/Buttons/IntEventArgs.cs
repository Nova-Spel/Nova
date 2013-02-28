using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova_Alpha
{
    class IntEventArgs : EventArgs
    {
        int value;

        public IntEventArgs(int value)
        {
            this.value = value;
        }
    }
}
