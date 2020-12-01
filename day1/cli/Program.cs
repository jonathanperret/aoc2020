using System;
using System.IO;
using System.Linq;

namespace cli
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("day1.txt");
            int[] amounts = lines.Select(l => int.Parse(l)).ToArray();
            for (int i = 0; i < amounts.Length - 1; i++)
            {
                for (int j = i + 1; j < amounts.Length; j++)
                {
                    if (amounts[i] + amounts[j] == 2020)
                    {
                        Console.WriteLine(amounts[i] * amounts[j]);
                    }

                }
            }
        }
    }
}
