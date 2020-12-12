using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using static Util;

public static class Program
{
    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");
        var lines = File.ReadAllLines("input.txt");

        int x = 0, y = 0;
        int wx = 10, wy = 1;
        foreach (string line in lines)
        {
            char move = line[0];
            int dist = int.Parse(line.Substring(1));
            switch (line)
            {
                case "L180":
                case "R180":
                    wx = -wx;
                    wy = -wy;
                    break;
                case "L270":
                case "R90":
                    (wx, wy) = (wy, -wx);
                    break;
                case "R270":
                case "L90":
                    (wx, wy) = (-wy, wx);
                    break;
                default:
                    switch (move)
                    {
                        case 'F':
                            x += dist * wx;
                            y += dist * wy;
                            break;
                        case 'N':
                            wy += dist;
                            break;
                        case 'S':
                            wy -= dist;
                            break;
                        case 'W':
                            wx -= dist;
                            break;
                        case 'E':
                            wx += dist;
                            break;
                    }
                    break;
            }
            W($"{line} {x} {y} {wx} {wy} {Math.Abs(x) + Math.Abs(y)}");
        }
    }
}