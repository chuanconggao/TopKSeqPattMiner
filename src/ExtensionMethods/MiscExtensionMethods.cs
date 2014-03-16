using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace ExtensionMethods
{
    public static class MiscExtensionMethods
    {
        public static bool EqualsAnyOf<T>(this T source, params T[] list)
        {
          return list.Contains(source);
        }
    }
}
