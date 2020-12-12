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
        int dx = 10, dy = 1;
        foreach (string line in lines)
        {
            char move = line[0];
            int dist = int.Parse(line.Substring(1));
            switch (line)
            {
                case "L180":
                case "R180":
                    dx = -dx;
                    dy = -dy;
                    break;
                case "L270":
                case "R90":
                    (dx, dy) = (dy, -dx);
                    break;
                case "R270":
                case "L90":
                    (dx, dy) = (-dy, dx);
                    break;
                default:
                    switch (move)
                    {
                        case 'F':
                            x += dist * dx;
                            y += dist * dy;
                            break;
                        case 'N':
                            dy += dist;
                            break;
                        case 'S':
                            dy -= dist;
                            break;
                        case 'W':
                            dx -= dist;
                            break;
                        case 'E':
                            dx += dist;
                            break;
                    }
                    break;
            }
            W($"{line} {x} {y} {dx} {dy} {Math.Abs(x) + Math.Abs(y)}");
        }
    }
}