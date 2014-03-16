using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace ExtensionMethods
{
    public static class DebugExtensionMethods
    {
        private static void print(string s)
        {
            print(s, Console.Error);
        }

        private static void print(string s, System.IO.TextWriter output)
        {
            output.WriteLine(s);
        }

        public static T Debug<T>(this T t)
        {
            print(t.GetDebugInfo());
            return t;
        }

        public static T Debug<T>(this T t, System.IO.TextWriter output)
        {
            print(t.GetDebugInfo(), output);
            return t;
        }

        public static string GetDebugInfo<T>(this T t)
        {
            return JsonConvert.SerializeObject(t, Formatting.Indented, 
                new JsonSerializerSettings() 
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
    }
}

