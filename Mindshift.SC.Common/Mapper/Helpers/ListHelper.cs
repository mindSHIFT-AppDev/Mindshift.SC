using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindshift.SC.Common.Mapper.Helpers
{
    public static class ListHelper
    {
        public static List<T> ConvertType<T>(this List<object> list)
        {
            var list2 = new List<T>();
            for (int i = 0; i < list.Count; i++) list2.Add((T)list[i]);
            return list2;
        }
    }
}
