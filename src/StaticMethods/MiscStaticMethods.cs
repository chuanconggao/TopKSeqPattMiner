using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace StaticMethods
{
    public static class MiscStaticMethods
    {
        public static KeyValuePair<TKey, TValue> MakeKeyValuePair<TKey, TValue>(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }

        // Use File.ReadLines() on .Net Framework 4.0+
        public static IEnumerable<string> ReadLines(string fileName, System.Text.Encoding encoding)
        {
            List<string> results = new List<string>();
            using (var reader = new StreamReader(fileName, encoding))
            {
                while (!reader.EndOfStream)
                {
                    results.Add(reader.ReadLine());
                }
            }
            return results;
        }

        // Use string.Join() on .Net Framework 4.0+
        public static string Join(string sep, IEnumerable<string> src)
        {
            return src.Aggregate((p, c) => p + sep + c);
        }
    }
}
