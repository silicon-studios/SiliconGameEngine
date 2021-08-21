using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiliconGameEngine.Logic
{
    public static class ListExtensions
    {
        public static void Set<T>(this List<T> lst, T obj, int num) where T : class
        {
            int cnt = lst.Count;
            if (cnt > num)
                lst[num] = obj;
            else
            {
                for (int i = cnt; cnt <= num - 1; i++)
                {
                    lst.Add(default);
                }
                lst.Add(obj);
            }
        }
    }
}
