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
    static void ExtendedGCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y, out BigInteger g)
    {
        if (b == 0)
        {
            x = 1;
            y = 0;
            g = a;
            return;
        }
        ExtendedGCD(b, a % b, out BigInteger x1, out BigInteger y1, out BigInteger d);
        x = y1;
        y = x1 - y1 * (a / b);
        g = d;
        //W($"{a} * {x} + {b} * {y} = {g}");
    }

    // solve ax + by = c
    static void solve(BigInteger a, BigInteger b, BigInteger c, out BigInteger x0, out BigInteger y0)
    {
        W($"solving {a}*x + {b}*y = {c}");
        ExtendedGCD(BigInteger.Abs(a), BigInteger.Abs(b), out var x, out var y, out var g);
        if (c % g != 0)
        {
            throw new Exception($"{c}%{g} = {c % g}");
        }
        x0 = x * c / g;
        y0 = y * c / g;
        if (a < 0) x0 = -x0;
        if (b < 0) y0 = -y0;
        W($"{a} * {x0} + {b} * {y0} = {a * x0 + b * y0} = {c}");
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt").ToArray();
        // var lines = File.ReadAllLines("example.txt").ToArray();
        var buses = lines[1].Split(",");


        void step(BigInteger bus1period, BigInteger bus1offset, BigInteger bus2period, BigInteger bus2offset, out BigInteger newbusperiod, out BigInteger newbusoffset)
        {
            solve(bus1period, -bus2period, -bus1offset - bus2offset, out BigInteger x0b, out BigInteger y0b);
            newbusperiod = bus1period * bus2period;
            newbusoffset = bus1period * x0b + bus1offset;
            W($"newbusoffset before modulo = {newbusoffset}");
            newbusoffset = newbusoffset % newbusperiod;
            W($"newbusoffset after modulo = {newbusoffset}");
            if (newbusoffset < 0)
            {
                W($"newbusoffset is negative, adding period");
                newbusoffset = newbusoffset + newbusperiod;
            }
            W($"t = n * {newbusperiod} + {newbusoffset}");
            W($"t0 = {newbusoffset}");
            // W($"t1 = {newbusperiod + newbusoffset}");
        }

        // step(7, 0, 13, 1, out BigInteger period, out BigInteger offset);
        // step(period, offset, 59, 4, out BigInteger newbusperiod, out BigInteger newbusoffset);

        buses.Select((busid, busoffset) =>
        {
            W($"Adding bus {busid} at {busoffset}");
            return (busid == "x" ? -1 : BigInteger.Parse(busid), (BigInteger)busoffset);
        })
        .Where(bus => bus.Item1 > 0)
        .Aggregate((bus1, bus2) =>
        {
            W($"Merging bus {bus1} with {bus2}");
            var (period1, offset1) = bus1;
            var (period2, offset2) = bus2;

            step(period1, offset1, period2, offset2, out BigInteger newperiod, out BigInteger newoffset);

            W($"Result is bus {newperiod},{newoffset}");

            return (newperiod, newoffset);
        });


        //        solve(91, -59, 10);
        //ExtendedGCD(91,-59, out var x, out var y, out var g);

        // buses.Aggreg(id =>
        // {
        //     BigInteger period = 1;
        //     W($"{id} {period}");
        //     return result;
        // }).Consume();


        // i_0 % t == 0 && i_1 % t == i_1 - 1 && i_4 % t == i_4 - 4

        // i_0 % t == 0 && All(n > 1, i_n - i_n % t == 0)

        // 13y = 7x + 1
        // 7x - 13y = -1

        // 7x - t = 0 => t = 7x
        // 13y - t = 1 => t = 13y -1

        // 7x = 13y - 1

        // 7x - 13y = -1

        // => x = 13n - 2
        //    y = 7n - 1

        // => t = (13 * 7)n - 14

        // n = 1 => x = 11, y = 6 =>  (7 * x = ) 77, 78
        // n = 2 => x = 24, y = 13 => 168, 169
        // n = 3 => x = 37, y = 20 => 259, 260
        // => t_n = (13 * 7) * n - 14
        // => t_n = 7 * (13 * n - 2)

        // period = 13 * 7 = 91
        // offset = -14

        // => t = 91 * z - 14

        // 7 * x - t = 0
        // 13 * y - t = 1

        // t = 91 * z - 14
        // 59 * w - t = 4  => t = 59 * w - 4

        // 91 * z - 14 = 59 * w - 4
        // 91 * z - 59 * w = 14 - 4 = 10

        // z = 240 + 59 * n
        // w = 370 + 91 * n

        // t = 91 * z - 14
        //   = 91 * (240 + 59 * n) - 14
        //   = (91 * 59) * n + 91 * 240 - 14
        //   = 5369 * n + 21826
        // t_min = 21826 % 5369

        // n = 0 => z = 240, w = 370 => (91 * 240 - 14 = 21826, 59 * 370 - 4 = 21826) 

        // period = 91 * 59 = 5369

        // 21826 % 7 = 0   ✅
        // 21826 % 13 = 12 ✅
        // 21826 % 59 = 55 ✅

        // 21826 % 5369 = 350

        // 350 % 7 = 0   ✅
        // 350 % 13 = 12 ✅
        // 350 % 59 = 55 ✅

        // 27195 % 7 = 0   ✅
        // 27195 % 13 = 12 ✅
        // 27195 % 59 = 55 ✅



        // n0 * x - n1 * y = -1 => x = -k +  p(n0, n1) = 
        // p(x0,x1)

        // Bézout => 13a + 7b = 1

        // 59z = 7x + 4

        // 7  14 21 28 35 42
        // 13 26 39 52 
    }
}