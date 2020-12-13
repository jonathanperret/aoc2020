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
    static void ExtendedGCD(long a, long b, out long x, out long y, out long g)
    {
        if (b == 0)
        {
            x = 1;
            y = 0;
            g = a;
            return;
        }
        ExtendedGCD(b, a % b, out long x1, out long y1, out long d);
        x = y1;
        y = x1 - y1 * (a / b);
        g = d;
        W($"{a} * {x} + {b} * {y} = {g}");
    }

    // solve ax + by = c where a and b are coprime, return x
    static long solve(long a, long b, long c)
    {
        W($"Solving {a} * x + {b} * y = {c}");
        ExtendedGCD(Math.Abs(a), Math.Abs(b), out var x, out var y, out var g);
        if (g != 1)
        {
            throw new Exception($"{c}%{g} = {c % g}");
        }
        long x0 = x * c;
        W($"solution: {a} * {x0} + {b} * y = {c}");
        return x0;
    }

    static long MultiplyModuloLong(long a, long b, long m)
    {
        if (b < 0)
        {
            return -MultiplyModuloLong(a, -b, m);
        }
        if (b > 1)
        {
            long half = b / 2;
            long result = 2 * MultiplyModuloLong(a, half, m);
            if (b % 2 != 0) result += a;
            return result % m;
        }
        return (a * b) % m;
    }

    private static void step(long bus1period, long bus1offset, long bus2period, long bus2offset, out long newbusperiod, out long newbusoffset)
    {
        long x0 = solve(bus1period, -bus2period, -bus1offset - bus2offset);
        newbusperiod = bus1period * bus2period;
        W($"newbusperiod = {newbusperiod}");
        W($"x0 =           {x0}");
        newbusoffset = MultiplyModuloLong(bus1period, x0, newbusperiod) + bus1offset;
        W($"newbusoffset with pre-modulo = {newbusoffset}");
        if (newbusoffset < 0)
        {
            W($"newbusoffset is negative, adding period");
            newbusoffset = newbusoffset + newbusperiod;
        }
        W($"t = n * {newbusperiod} + {newbusoffset}");
        W($"t0 = {newbusoffset}");
        // W($"t1 = {newbusperiod + newbusoffset}");
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt").ToArray();
        // var lines = File.ReadAllLines("example.txt").ToArray();
        var buses = lines[1].Split(",");

        buses.Select((busid, busoffset) =>
        {
            W($"Adding bus {busid} at {busoffset}");
            return (busid == "x" ? -1 : long.Parse(busid), (long)busoffset);
        })
        .Where(bus => bus.Item1 > 0)
        .Aggregate((bus1, bus2) =>
        {
            W($"Merging bus {bus1} with {bus2}");
            var (period1, offset1) = bus1;
            var (period2, offset2) = bus2;

            step(period1, offset1, period2, offset2, out long newperiod, out long newoffset);

            W($"Result is bus {newperiod},{newoffset}");

            return (newperiod, newoffset);
        });

    }
}