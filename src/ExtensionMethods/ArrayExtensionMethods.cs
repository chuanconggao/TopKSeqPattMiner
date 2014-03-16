using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace ExtensionMethods
{
    public static class ArrayExtensionMethods
    {
        public static T[] Append<T>(this T[] source, T t)
        {
            int len = source.Length;
            T[] dest = new T[len + 1];
            Array.Copy(source, 0, dest, 0, len);
            dest[len] = t;
            return dest;
        }

        public static T[] Subarray<T>(this T[] source, int start)
        {
            return source.Subarray(start, source.Length - start);
        }

        public static T[] Subarray<T>(this T[] source, int start, int len)
        {
            T[] dest = new T[len];
            Array.Copy(source, start, dest, 0, len);
            return dest;
        }
    }

    public static class ArraySpecialExtensionMethods
    {
        public static IEnumerable<T[]> LongestSatisfyingSubarrays<T>(this T[] source, Func<T, bool> func)
        {
            for (int s = 0, e = 0; ; e++)
            {
                if (e >= source.Length || !func(source[e]))
                {
                    if (e > s)
                    {
                        yield return source.Subarray(s, e - s);
                    }
                    s = e + 1;

                    if (e >= source.Length)
                    {
                        break;
                    }
                }
            }
        }
    }
}

