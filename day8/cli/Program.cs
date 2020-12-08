using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace cli
{
    public static class Program
    {
        public static void W<T>(T data)
        {
            Console.WriteLine(data);
        }
        static void Main(string[] args)
        {
            var text = File.ReadAllText("day8.txt");
            var lines = File.ReadAllLines("day8.txt");
            var groups = lines.Split("");

            var seen = new HashSet<long>();
            Run(lines, seen);

            W($"loop length={seen.Count}");
            foreach (long ip in seen.OrderBy(x => x))
            {
                string line = lines[ip];
                var (op, argstr) = line.Split(" ");
                switch (op)
                {
                    case "jmp":
                        W($"changing ip={ip} to nop");
                        lines[ip] = $"nop {argstr}";
                        break;
                    case "nop":
                        W($"changing ip={ip} to jmp");
                        lines[ip] = $"jmp {argstr}";
                        break;
                    default:
                        W($"not changing ip={ip}");
                        continue;
                }
                if (Run(lines, new HashSet<long>()))
                {
                    return;
                }
                else
                {
                    lines[ip] = line;
                }
            }
        }

        private static bool Run(string[] lines, HashSet<long> seen)
        {
            long acc = 0;
            int opcnt = 0;
            long ip = 0;
            var ipseq = new List<int>();
            while (opcnt < 10000)
            {
                if (ip == lines.Length)
                {
                    W($"terminating after {opcnt} ops, A={acc}");
                    return true;
                }
                string line = lines[ip];

                if (seen.Contains(ip))
                {
                    W($"infinite loop detected after {opcnt} ops, ip={ip}, A={acc}");
                    return false;
                }

                seen.Add(ip);

                var (op, argstr) = line.Split(" ");
                long arg = long.Parse(argstr);
                long nextip = ip + 1;
                switch (op)
                {
                    case "nop":
                        break;
                    case "acc":
                        acc += arg;
                        break;
                    case "jmp":
                        nextip = ip + arg;
                        break;
                    default:
                        W($"bad op {op}");
                        break;
                }
                W($"{ip} {line} A={acc}");
                ip = nextip;
                opcnt++;
            }
            W("timeout");
            return false;
        }
    }
}
