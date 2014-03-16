using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using NDesk.Options;

using ExtensionMethods;
using StaticMethods;

class Program
{
    public static T checkInitialized<T>(T v, T def)
    {
        if (v.Equals(def))
        {
            throw new OptionException();
        }
        return v;
    }

    public static System.Text.Encoding GlobalFileEncoding = System.Text.Encoding.UTF8;

    public static int Main(string[] args)
    {
        string inFileName = null;
        string stopWordFileName = null;

        int k = 0;
        int minLen = 1;
        int maxLen = int.MaxValue;

        bool showHelp = false;

        var optSet = new OptionSet()
        {
            {
                "in=", "Name of input file.", 
                v => inFileName = checkInitialized(v, inFileName)
            }, 
            {
                "stopwords=", "Name of stop word list file.", 
                v => stopWordFileName = checkInitialized(v, stopWordFileName)
            }, 
            {
                "k|num=", "Number of returned top results.", 
                v => k = checkInitialized(int.Parse(v), k)
            }, 
            {
                "minlen=", "Minimum length of returned patterns.", 
                v => minLen = int.Parse(v)
            }, 
            {
                "maxlen=", "Maximum length of returned patterns.", 
                v => maxLen = int.Parse(v)
            }, 
            {
                "h|help", "Show this help.", 
                v => showHelp = v != null
            }
        };

        int exitCode = 0;

        try
        {
            optSet.Parse(args);

            if (inFileName == null || stopWordFileName == null || k == 0)
            {
                throw new OptionException();
            }

        }
        catch(OptionException)
        {
            Console.WriteLine("Incorrect arguments.\n");
            showHelp = true;
            exitCode = 1;
        }

        if (showHelp)
        {
            optSet.WriteOptionDescriptions(Console.Out);
            return exitCode;
        }

        main(inFileName, stopWordFileName, 
             k, minLen, maxLen);

        return exitCode;
    }

    private static void main(string inFileName, string stopWordFileName, 
                             int k, int minLen, int maxLen)
    {
        HashSet<string> stopWords = new HashSet<string>();
#if DOTNET_35
        MiscStaticMethods
#else
        File
#endif
            .ReadLines(stopWordFileName, GlobalFileEncoding).ForEach(l => stopWords.Add(l));

        var mapper = new Mapper();

        Regex wholeSymbolRegex = new Regex(@"^\W+$", RegexOptions.Compiled);

        List<string[]> strDB = new List<string[]>();

        foreach (string line in 
#if DOTNET_35
            MiscStaticMethods
#else
            File
#endif
                .ReadLines(inFileName, GlobalFileEncoding))
        {
            Mapper.MapperHelper.Tokenize(line, stopWords)
                .LongestSatisfyingSubarrays(w => !wholeSymbolRegex.IsMatch(w))
                .Where(l => l.Length >= minLen)
                .ForEach(l => strDB.Add(l));
        }

        var miner = new TopKMiner();

        var results = miner.Mine(mapper.MapDB(strDB), 
                                 k, minLen, maxLen);

        foreach (var pair in results)
        {
            Console.WriteLine(
#if DOTNET_35
                MiscStaticMethods
#else
                string
#endif
                    .Join(" ", pair.Key.Map(mapper.ReverseDictionary)) + " : " + pair.Value.ToString());
        }
    }
}
