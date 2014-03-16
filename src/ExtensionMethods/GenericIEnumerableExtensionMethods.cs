using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace ExtensionMethods
{
    public static class GenericIEnumerableExtensionMethods
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach(T item in source)
            {
                action(item);
            }
        }
    }

    public static class GenericIEnumerableMappingExtensionMethods
    {
        public static IEnumerable<TDst> Map<TSrc, TDst>(this IEnumerable<TSrc> src, Dictionary<TSrc, TDst> dict)
        {
            return src.Select(s => dict[s]);
        }

        public static IEnumerable<TDst> Map<TSrc, TDst>(this IEnumerable<TSrc> src, ref Dictionary<TSrc, TDst> dict, TDst start, Func<TDst, TDst> stepFunc)
        {
            if (dict == null)
            {
                dict = new Dictionary<TSrc, TDst>();
            }

            List<TDst> dst = new List<TDst>(src.Count());

            TDst pt = start;

            foreach (TSrc s in src)
            {
                TDst t;

                if (!dict.TryGetValue(s, out t))
                {
                    t = pt;

                    dict.Add(s, pt);
                    pt = stepFunc(pt);
                }

                dst.Add(t);
            }

            return dst;
        }
    }
}
