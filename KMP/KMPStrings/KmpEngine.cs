using System.Collections.Generic;

public static class KmpEngine
{
    // Build LPS array without any printing
    public static int[] BuildLps(string pattern)
    {
        if (pattern == null) return new int[0];
        int n = pattern.Length;
        int[] lps = new int[n];
        if (n == 0) return lps;
        int len = 0;
        lps[0] = 0;
        int i = 1;
        while (i < n)
        {
            if (pattern[i] == pattern[len])
            {
                len++;
                lps[i] = len;
                i++;
            }
            else
            {
                if (len != 0)
                {
                    len = lps[len - 1];
                }
                else
                {
                    lps[i] = 0;
                    i++;
                }
            }
        }
        return lps;
    }

    // Execute KMP search without printing, return positions and comparison count
    public static (int Comparisons, List<int> Positions) Search(string text, string pattern)
    {
        var positions = new List<int>();
        if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text)) return (0, positions);
        int[] lps = BuildLps(pattern);
        int n = text.Length;
        int m = pattern.Length;
        int i = 0, j = 0;
        int comparisons = 0;
        while (i < n)
        {
            comparisons++;
            if (text[i] == pattern[j])
            {
                i++; j++;
            }
            if (j == m)
            {
                positions.Add(i - j);
                j = lps[j - 1];
            }
            else if (i < n && text[i] != pattern[j])
            {
                if (j != 0) j = lps[j - 1];
                else i++;
            }
        }
        return (comparisons, positions);
    }
}
