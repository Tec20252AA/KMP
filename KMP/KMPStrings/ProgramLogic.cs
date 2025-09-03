using System;
using System.Collections.Generic;

static class ProgramLogic
{
    // Constantes para el texto y el patrón a buscar (ejemplo grande para comparar eficiencia)
    const string TEXT = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                    "B" +
                    "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                    "B" +
                    "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";

    const string PATTERN = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                    "B";

    public static void Run(string[] args)
    {
        bool verbose = false;
        if (args != null)
        {
            foreach (var a in args)
            {
                if (a == "--verbose" || a == "-v") { verbose = true; break; }
            }
        }

        Console.WriteLine("Algoritmo KMP - Búsqueda de patrones en cadenas\n");
        if (verbose)
        {
            Console.WriteLine($"Texto de ejemplo: {TEXT}");
            Console.WriteLine($"Patrón de ejemplo: {PATTERN}\n");
        }

        Console.WriteLine("--- Ejemplo fijo ---");
        int[] lps = BuildLPS(PATTERN, verbose);

        Console.WriteLine("\nComparativo: Fuerza bruta vs KMP\n");
        SearchResult bruteResult = BruteForceSearch(TEXT, PATTERN, verbose);
        SearchResult kmpResult = KMPSearch(TEXT, PATTERN, lps, verbose);
        Console.WriteLine($"\nComparaciones realizadas:");
        Console.WriteLine($"Fuerza bruta: {bruteResult.Comparisons}");
        Console.WriteLine($"KMP: {kmpResult.Comparisons}");
        Console.WriteLine("(Menos comparaciones = mayor eficiencia)\n");

        if (bruteResult.Positions.Count > 0)
        {
            Console.WriteLine($"Fuerza bruta: patrón encontrado en las posiciones: {string.Join(", ", bruteResult.Positions)}");
        }
        else
        {
            Console.WriteLine("Fuerza bruta: patrón no encontrado en el texto.");
        }

        if (kmpResult.Positions.Count > 0)
        {
            Console.WriteLine($"KMP: patrón encontrado en las posiciones: {string.Join(", ", kmpResult.Positions)}");
        }
        else
        {
            Console.WriteLine("KMP: patrón no encontrado en el texto.");
        }

        Console.WriteLine("\n--- LPS de patrón ingresado por el usuario ---");
        Console.Write("Ingresa un patrón para calcular su LPS: ");
        string userPattern = Console.ReadLine();
        if (!string.IsNullOrEmpty(userPattern))
        {
            int[] userLps = BuildLPS(userPattern, verbose);
            Console.WriteLine($"LPS para el patrón '{userPattern}': {string.Join(",", userLps)}");
        }
    }

    // Función para construir el arreglo de prefijos (lps)
    public static int[] BuildLPS(string pattern, bool verbose)
    {
        int n = pattern.Length;
        int[] lps = new int[n];
        int len = 0; // longitud del prefijo más largo
        if (n == 0) return lps;
        lps[0] = 0; // lps[0] siempre es 0
        if (verbose) Console.WriteLine($"Construyendo el arreglo LPS (Longest Prefix Suffix) para el patrón '{pattern}':");
        int i = 1;
        while (i < n)
        {
            if (verbose) Console.WriteLine($"i={i}, len={len}, lps={string.Join(",", lps)}");
            if (pattern[i] == pattern[len])
            {
                len++;
                lps[i] = len;
                if (verbose) Console.WriteLine($"  Coincidencia: pattern[{i}] == pattern[{len-1}] -> len={len}, lps[{i}]={lps[i]}");
                i++;
            }
            else
            {
                if (len != 0)
                {
                    len = lps[len - 1];
                    if (verbose) Console.WriteLine($"  No coincide, pero len!=0: len actualizado a {len}");
                }
                else
                {
                    lps[i] = 0;
                    if (verbose) Console.WriteLine($"  No coincide y len==0: lps[{i}]=0");
                    i++;
                }
            }
        }
        if (verbose) Console.WriteLine($"Arreglo LPS final: {string.Join(",", lps)}\n");
        return lps;
    }

    // Resultado de búsqueda: número de comparaciones y posiciones donde aparece el patrón
    public class SearchResult
    {
        public int Comparisons { get; set; }
        public List<int> Positions { get; set; } = new List<int>();
    }

    // Función para buscar el patrón en el texto usando KMP (con impresiones opcionales)
    public static SearchResult KMPSearch(string text, string pattern, int[] lps, bool verbose)
    {
        // Delegate actual KMP work to KmpEngine (no printing). Keep verbose headers and per-match prints here.
        if (verbose) Console.WriteLine("Buscando el patrón en el texto con KMP:");
        var (comparisons, positions) = KmpEngine.Search(text, pattern);
        if (verbose)
        {
            foreach (var pos in positions)
            {
                Console.WriteLine($"Patrón encontrado en la posición {pos}\n");
            }
        }
        return new SearchResult { Comparisons = comparisons, Positions = positions };
    }

    // Búsqueda de fuerza bruta (con impresiones opcionales)
    public static SearchResult BruteForceSearch(string text, string pattern, bool verbose)
    {
        int n = text.Length;
        int m = pattern.Length;
        int comparisons = 0;
        if (verbose) Console.WriteLine("Buscando el patrón en el texto con fuerza bruta:");
        var positions = new List<int>();
        for (int i = 0; i <= n - m; i++)
        {
            int j;
            for (j = 0; j < m; j++)
            {
                comparisons++;
                if (verbose) Console.WriteLine($"Comparando text[{i + j}]={text[i + j]} con pattern[{j}]={pattern[j]}");
                if (text[i + j] != pattern[j])
                {
                    if (verbose) Console.WriteLine($"  No coincide en posición {i + j}");
                    break;
                }
            }
            if (j == m)
            {
                if (verbose) Console.WriteLine($"Patrón encontrado en la posición {i}\n");
                positions.Add(i);
            }
        }
        return new SearchResult { Comparisons = comparisons, Positions = positions };
    }
}
