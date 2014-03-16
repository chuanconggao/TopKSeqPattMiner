using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ExtensionMethods;
using StaticMethods;

namespace StaticMethods
{
    public class ComparerGenerator
    {
        public class Comparer<T> : IComparer<T>
        {
            private Func<T, T, int> func;

            public int Compare(T x, T y)
            {
                return func(x, y);
            }

            public Comparer(Func<T, T, int> func)
            {
                this.func = func;
            }
        }

        public static Comparer<T> Create<T>(Func<T, T, int> func)
        {
            return new ComparerGenerator.Comparer<T>(func);
        }
    }
}
