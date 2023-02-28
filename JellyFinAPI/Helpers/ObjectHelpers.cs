using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JellyFinAPI.Helpers
{
    internal static class ObjectHelpers
    {
        public static T2? Then<T1,T2>(this T1? Input, Func<T1,T2> Action)
        {
            if(Input != null)
                return Action(Input);
            return default;
        }
    }
}
