using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using ExtensionMethods;

class Mapper
{
    public class MapperHelper
    {
        private static Regex symbolRegex = new Regex(@"(\W+)", RegexOptions.Compiled);
        private static Regex spaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

        public static string[] Tokenize(string str, HashSet<string> stopWords)
        {
            return spaceRegex.Split(str.Trim().ToLower())
                .SelectMany(wl => symbolRegex.Split(wl)
                                             .Where(w => w.Length > 0))
                .Where(w => !stopWords.Contains(w))
                .ToArray();
        }
    }
    private Dictionary<string, int> dict;
    private Dictionary<int, string> revDict;

    public Dictionary<string, int> Dictionary
    {
        get
        {
            return dict;
        }
    }

    public Dictionary<int, string> ReverseDictionary
    {
        get
        {
            return revDict;
        }
    }

    public Mapper()
    {
    }

    public List<int[]> MapDB(List<string[]> strDB)
    {
        dict = new Dictionary<string, int>();

        List<int[]> intDB = new List<int[]>(strDB.Count);

        for (int i = 0; i < strDB.Count; i++)
        {
            intDB.Add(strDB[i].Map(ref dict, dict.Count, j => j + 1)
                              .ToArray());
        }

        revDict = dict.ToDictionary(x => x.Value, x => x.Key);

        return intDB;
    }
}
