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
    static BigInteger ExtendedGCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y, out BigInteger g)
    {
        if (b == 0)
        {
            x = 1;
            y = 0;
            g = a;
            return g;
        }
        ExtendedGCD(b, a % b, out BigInteger x1, out BigInteger y1, out BigInteger d);
        x = y1;
        y = x1 - y1 * (a / b);
        g = d;
        return g;
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

        // step(7, 0, 13, 1, out BigInteger period, out BigInteger offset);
        // step(period, offset, 59, 4, out BigInteger newbusperiod, out BigInteger newbusoffset);

        var busesPO = buses.Select((busid, busoffset) =>
        {
            W($"Adding bus {busid} at {busoffset}");
            return (busid == "x" ? -1 : BigInteger.Parse(busid), (BigInteger)busoffset);
        })
        .Where(bus => bus.Item1 > 0).ToArray();

        // busesPO = new (BigInteger, BigInteger)[] {
        //     // (12_996_139, 2_501_577), (19, 32)
        //     // (7,0),(13,1)
        // };

        int seen = 2;

        var (finalperiod, finaloffset) = busesPO
        .Aggregate((bus1, bus2) =>
        {
            W($"Merging bus {bus1} with {bus2}");
            var (period1, offset1) = bus1;
            var (period2, offset2) = bus2;

            step(period1, offset1, period2, offset2, out BigInteger newperiod, out BigInteger newoffset);

            W($"Result is bus {newperiod},{newoffset}");

            W($"new bus equation is t={newperiod}*x - {newoffset}");
            BigInteger t0 = newperiod - newoffset;
            W($"smallest t satisfying is t0={t0}");

            check(t0, busesPO.Take(seen));

            seen++;

            return (newperiod, newoffset);
        });

        BigInteger t0 = finaloffset;


        //        solve(91, -59, 10);
        //ExtendedGCD(91,-59, out var x, out var y, out var g);

        // buses.Aggreg(id =>
        // {
        //     BigInteger period = 1;
        //     W($"{id} {period}");
        //     return result;
        // }).Consume();


        // 13y = 7x + 1
        // 7x - 13y = -1

        // 7x - t = 0 => t = 7x
        // 13y - t = 1 => t = 13y -1

        // 7x = 13y - 1

        // 7x - 13y = -1

        // t = p1 * x - o1
        // t = p2 * y - o2
        // p1 * x - o1 = p2 * y - o2
        // p1 * x - p2 * y = o1 - o2
        // 7 * x - 13 * y = 0 - 1

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


        // 12996139, 10494562  with  19, 32

        // t = 12996139 * x - 10494562
        // t = 19 * y - 32
        // 12996139 * x - 10494562 = 19 * y - 32
        // 12996139 * x - 19 * y = 10494562 - 32 = 10494530

        // p1 * x - p2 * y = o1 - o2
        // 12996139 * x - 19 * y = 10494562 - 32


    }

    private static void step(BigInteger bus1period, BigInteger bus1offset, BigInteger bus2period, BigInteger bus2offset, out BigInteger newbusperiod, out BigInteger newbusoffset)
    {
        W($"equation for bus1: t = {bus1period} * x - {bus1offset}");
        W($"equation for bus2: t = {bus2period} * y - {bus2offset}");
        solve(bus1period, -bus2period, bus1offset - bus2offset, out BigInteger x0, out BigInteger y0);
        W($"One solution is x0 = {x0}, y0 = {y0}");
        BigInteger t0 = bus1period * x0 - bus1offset;
        W($"  t0 = {bus1period} * {x0} - {bus1offset} = {bus1period * x0 - bus1offset} = {bus2period} * {y0} - {bus2offset} = {bus2period * y0 - bus2offset} = {t0}");

        BigInteger xperiod = bus2period;
        W($"Other values for x = {x0} + {xperiod} * n");
        // W($"Other values for y = {y0} + {bus1period} * n");
        // ExtendedGCD(bus1period, bus2period, out BigInteger _1, out BigInteger _2, out BigInteger periodgcd);
        // W($"The GCD of {bus1period} and {bus2period} is {periodgcd}");
        // BigInteger periodlcm = bus1period * bus2period / periodgcd;
        // W($"The LCM of {bus1period} and {bus2period} is {periodlcm}");
        W($"Other values for t using bus1 equation: t = {bus1period} * ({x0} + {xperiod} * n) - {bus1offset}");
        W($"Other values for t using bus1 equation: t = {bus1period} * {x0} + {bus1period} * {xperiod} * n - {bus1offset}");
        W($"Other values for t using bus1 equation: t = {bus1period} * {xperiod} * n + {bus1period} * {x0} - {bus1offset}");
        W($"Other values for t using bus1 equation: t = {bus1period} * {xperiod} * n + {t0}");

        newbusperiod = bus1period * xperiod;
        W($"So new bus period is {bus1period} * {xperiod} = {newbusperiod}");
        W($"times matching are t = {newbusperiod} * n + {t0}");
        W($"t0 = {t0}");
        BigInteger tmin = t0 % newbusperiod;
        W($"tmin = {t0} % {newbusperiod} = {tmin}");
        if (tmin < 0)
        {
            W($"tmin is negative, adding period");
            tmin = tmin + newbusperiod;
        }

        W($"tmin = {tmin} should fit equation for bus1: {tmin} = {bus1period} * x - {bus1offset}");
        W($"tmin = {tmin} should fit equation for bus1: {tmin} + {bus1offset} = {bus1period} * x");
        W($"tmin = {tmin} should fit equation for bus1: ({tmin} + {bus1offset}) % {bus1period} = {(tmin + bus1offset) % bus1period} should be 0, x = {(tmin + bus1offset) / bus1period}");

        W($"tmin = {tmin} should fit equation for bus2: {tmin} = {bus2period} * x - {bus2offset}");
        W($"tmin = {tmin} should fit equation for bus2: {tmin} + {bus2offset} = {bus2period} * x");
        W($"tmin = {tmin} should fit equation for bus2: ({tmin} + {bus2offset}) % {bus2period} = {(tmin + bus2offset) % bus2period} should be 0, x = {(tmin + bus2offset) / bus2period}");

        W($"smallest positive t is tmin = {tmin}");
        W($"checking bus1 against tmin"); checkBus((bus1period, bus1offset), tmin);
        W($"checking bus2 against tmin"); checkBus((bus2period, bus2offset), tmin);
        W($"checking bus1 against tmin + newbusperiod"); checkBus((bus1period, bus1offset), tmin + newbusperiod);
        W($"checking bus2 against tmin + newbusperiod"); checkBus((bus2period, bus2offset), tmin + newbusperiod);

        W($"New bus equation: t = {newbusperiod} * x + {tmin}");

        newbusoffset = newbusperiod - tmin;
        W($"So new bus offset is {newbusperiod} - t0 = {newbusoffset}");
    }

    private static bool check(BigInteger t0, IEnumerable<(BigInteger, BigInteger)> busesPO)
    {
        bool ok = busesPO.All(bus =>
        {
            bool result = checkBus(bus, t0);
            return result;
        });
        return ok;
    }

    private static bool checkBus((BigInteger, BigInteger) bus, BigInteger t0)
    {
        var (period, offset) = bus;
        bool result;
        W($"Checking bus {period},{offset} against {t0}…");
        BigInteger need_departure_at = t0 + offset;
        W($" Need a departure at {t0} + {offset} = {need_departure_at}");
        if (need_departure_at % period == 0)
        {
            W($"  bus leaves at {need_departure_at} = {need_departure_at / period} * {period}, all good");
            result = true;
        }
        else
        {
            result = false;
        }
        W($" check is {(result ? "OK" : "KO")}");
        return result;
    }
}