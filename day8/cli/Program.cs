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
            var states = new List<(long ip, long acc)>();
            var (_, firstLoopAcc) = Run(lines, seen, (0, 0), states.Add);
            W($"part 1: {firstLoopAcc}");
            foreach (var state in states)
            {
                var (ip, acc) = state;
                W($"restarting from ip={ip}, A={acc}");
                var (terminated, finalAcc) = Run(lines, seen, state, (_) => { });
                if (terminated)
                {
                    W($"part 2: {finalAcc}");
                    break;
                }
            }
        }

        private static (bool terminated, long finalAcc) Run(string[] lines, HashSet<long> seen, (long ip, long acc) state, Action<(long ip, long acc)> saveState)
        {
            var (ip, acc) = state;
            int opcnt = 0;
            while (opcnt < 10000)
            {
                if (ip == lines.Length)
                {
                    W($"terminating after {opcnt} ops, A={acc}");
                    return (true, acc);
                }
                string line = lines[ip];

                if (seen.Contains(ip))
                {
                    W($"infinite loop detected after {opcnt} ops, ip={ip}, A={acc}");
                    return (false, acc);
                }

                seen.Add(ip);

                var (op, argstr) = line.Split(" ");
                long arg = long.Parse(argstr);
                long nextip = ip + 1;
                switch (op)
                {
                    case "nop":
                        saveState((ip + arg, acc));
                        break;
                    case "acc":
                        acc += arg;
                        break;
                    case "jmp":
                        saveState((ip + 1, acc));
                        nextip = ip + arg;
                        break;
                    default:
                        W($"bad op {op}");
                        break;
                }
                //W($"{ip} {line} A={acc}");
                ip = nextip;
                opcnt++;
            }
            W("timeout");
            return (false, acc);
        }
    }
}
