using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiliconGameEngine.Mathematics
{
    public static class NumberExtensions
    {
        public static int Floor(this double val) => (int)Math.Floor(val);
        public static int Floor(this decimal val) => (int)Math.Floor(val);
        public static int Floor(this int val) => val;
    }
}
