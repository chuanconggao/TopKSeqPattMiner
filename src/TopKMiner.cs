using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ExtensionMethods;
using StaticMethods;

class TopKMiner
{
    public TopKMiner()
    {
    }

    private Dictionary<int, List<KeyValuePair<int, int>>> getLocalFrequentDict(List<int[]> db, List<KeyValuePair<int, int>> mdb)
    {
        var localFreqDict = new Dictionary<int, List<KeyValuePair<int, int>>>();
        for (int i = 0; i < mdb.Count; i++)
        {
            int[] a = db[mdb[i].Key];
            HashSet<int> visited = new HashSet<int>();
            for (int j = mdb[i].Value; j < a.Length; j++)
            {
                int w = a[j];
                if (!visited.Add(w))
                {
                    continue;
                }

                List<KeyValuePair<int, int>> l;
                if (!localFreqDict.TryGetValue(w, out l))
                {
                    l = new List<KeyValuePair<int, int>>();
                    localFreqDict.Add(w, l);
                }
                l.Add(MiscStaticMethods.MakeKeyValuePair(mdb[i].Key, j + 1));
            }
        }

        return localFreqDict;
    }

    private void mine_rec(List<int[]> db, int[] patt, 
        List<KeyValuePair<int, int>> mdb, 
        C5.IntervalHeap<KeyValuePair<int[], int>> maxHeap, int k, 
        int minLen, int maxLen)
    {
        foreach (var localFreq in getLocalFrequentDict(db, mdb).OrderByDescending(p => p.Value.Count))
        {
            if (patt.Length > 0 && patt.Any(x => x == localFreq.Key))
            {
                continue;
            }

            int newSup = localFreq.Value.Count;
            int[] newPatt = patt.Append(localFreq.Key);

            if (maxHeap.Count == k)
            {
                if (maxHeap.FindMin().Value >= newSup)
                {
                    continue;
                }
            }
            if (newPatt.Length >= minLen)
            {
                maxHeap.Add(MiscStaticMethods.MakeKeyValuePair(newPatt, newSup));
                if (maxHeap.Count > k)
                {
                    maxHeap.DeleteMin();
                }
            }

            if (newPatt.Length < maxLen)
            {
                mine_rec(db, newPatt, localFreq.Value, maxHeap, k, minLen, maxLen);
            }
        }
    }

    public List<KeyValuePair<int[], int>> Mine(List<int[]> db, int k, int minLen, int maxLen)
    {
        var maxHeap = new C5.IntervalHeap<KeyValuePair<int[], int>>(ComparerGenerator.Create<KeyValuePair<int[], int>>((x, y) => 
        {
            int v = x.Value.CompareTo(y.Value);
            if (v != 0)
            {
                return v;
            }
            else
            {
                return y.Key.Length.CompareTo(x.Key.Length);
            }
        }));

        mine_rec(db, new int[0], 
             Enumerable.Range(0, db.Count)
                       .Select(i => MiscStaticMethods.MakeKeyValuePair(i, 0))
                       .ToList(), 
             maxHeap, k, minLen, maxLen);

        var results = new List<KeyValuePair<int[], int>>();
        while (maxHeap.Count > 0)
        {
            var pair = maxHeap.DeleteMax();
            results.Add(pair);
        }
        return results;
    }
}
