using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova_Alpha
{
    class StringEventArgs : EventArgs
    {
        public string value;

        public StringEventArgs(string args)
        {
            this.value = args;
        }
    }
}
