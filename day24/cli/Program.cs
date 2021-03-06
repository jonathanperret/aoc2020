using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using static Util;
using System.Diagnostics;
using System.Reflection;
public static class Program
{
    public static (int, int)[] Part1(string[] lines)
    {
        var flipped = lines.Select(line =>
        {
            string[] dirs = Regex.Matches(line, "e|w|ne|nw|se|sw").Select(m => m.Value).ToArray();

            (int, int) coord =
                dirs.Aggregate((0, 0), (c, dir) =>
                {
                    var (x, y) = c;
                    return dir switch
                    {
                        "e" => (x + 1, y),
                        "w" => (x - 1, y),
                        "ne" => (x, y + 1),
                        "nw" => (x - 1, y + 1),
                        "sw" => (x, y - 1),
                        "se" => (x + 1, y - 1),
                        _ => throw new Exception("bad dir")
                    };
                });

            // W($"{dirs.Str(",")} => {coord}");
            return coord;
        });

        var black = flipped.GroupBy(c => c).Select(g => g.ToArray()).Where(g => g.Length % 2 == 1).Select(g => g[0]);

        // W(black.OrderBy(c => c).Str(","));

        return black.ToArray();
    }

    static int BitCount(this BitArray a)
    {
        return a.Cast<bool>().Count(b => b);
    }

    static BitArray SetTo(this BitArray a, BitArray other)
    {
        int[] aArray = arrayField.GetValue(a) as int[];
        other.CopyTo(aArray, 0);
        return a;
    }

    public static int Part2Sets((int x, int y)[] black, int days)
    {
        static IEnumerable<int> neighbors(int c)
        {
            yield return c + 1;
            yield return c - 1;
            yield return c + 1000;
            yield return c + 999;
            yield return c - 1000;
            yield return c - 999;
        }

        var blackInts = black.Select(c => (x: c.x + 500, y: c.y + 500)).Assert(c => c.x > 0 && c.x < 1000 && c.y > 0 && c.y < 1000)
            .Select(c => c.y * 1000 + c.x);

        var blackSet = Enumerable.ToHashSet(blackInts);

        for (int i = 0; i < days; i++)
        {
            var neighborList = blackSet.SelectMany(neighbors);
            var newBlack = neighborList
                .CountBy(c => c)
                .Choose(kv =>
                    (kv.Value == 2 || (kv.Value == 1 && blackSet.Contains(kv.Key)), kv.Key)
                );
            blackSet = Enumerable.ToHashSet(newBlack);

            // W($"Day {i + 1}: {blackSet.Count}");
        }

        return blackSet.Count;
    }

    static BitArray or(BitArray a, BitArray b)
    {
        return new BitArray(a).Or(b);
    }

    static BitArray xor(BitArray a, BitArray b)
    {
        return new BitArray(a).Xor(b);
    }

    static BitArray and(BitArray a, BitArray b)
    {
        return new BitArray(a).And(b);
    }
    const int stride = 128;
    const int bitcount = 4 * stride * stride;

    private static FieldInfo GetArrayField()
    {
        Type type = typeof(BitArray);
        return type.GetField("m_array", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    private static readonly FieldInfo arrayField = GetArrayField();

    public static int Part2BitsSlow((int x, int y)[] black, int days)
    {
        var blackInts = black.Select(c => (x: c.x + stride, y: c.y + stride))
            .Assert(c => c.x > 0 && c.x < 2 * stride && c.y > 0 && c.y < 2 * stride)
            .Select(c => c.y * 2 * stride + c.x);

        var blackSet = new BitArray(bitcount);
        blackInts.ForEach(i => blackSet.Set(i, true));

        for (int i = 0; i < days; i++)
        {
            var a = new BitArray(blackSet).RightShift(1);
            var b = new BitArray(blackSet).LeftShift(1);
            var c = new BitArray(blackSet).LeftShift(2 * stride - 1);
            var d = new BitArray(blackSet).LeftShift(2 * stride);
            var e = new BitArray(blackSet).RightShift(2 * stride - 1);
            var f = new BitArray(blackSet).RightShift(2 * stride);

            var ab = or(a, b);
            var ac = or(a, c);
            var bc = or(b, c);
            var ad = or(a, d);
            var bd = or(b, d);
            var cd = or(c, d);
            var ae = or(a, e);
            var be = or(b, e);
            var ce = or(c, e);
            var de = or(d, e);
            var af = or(a, f);
            var bf = or(b, f);
            var cf = or(c, f);
            var df = or(d, f);
            var ef = or(e, f);

            var abcd = or(ab, cd);
            var abce = or(ab, ce);
            var abde = or(ab, de);
            var acde = or(ac, de);
            var bcde = or(bc, de);
            var abcf = or(ab, cf);
            var abdf = or(ab, df);
            var acdf = or(ac, df);
            var bcdf = or(bc, df);
            var abef = or(ab, ef);
            var acef = or(ac, ef);
            var bcef = or(bc, ef);
            var adef = or(ad, ef);
            var bdef = or(bd, ef);
            var cdef = or(cd, ef);

            var bcdef = or(bcde, f);
            var acdef = or(acde, f);
            var abdef = or(abde, f);
            var abcef = or(abce, f);
            var abcdf = or(abcd, f);
            var abcde = or(abcd, e);

            var two =
                    and(a, b).And(cdef.Not())
                .Or(and(b, c).And(adef.Not()))
                .Or(and(a, c).And(bdef.Not()))
                .Or(and(c, d).And(abef.Not()))
                .Or(and(b, d).And(acef.Not()))
                .Or(and(a, d).And(bcef.Not()))
                .Or(and(d, e).And(abcf.Not()))
                .Or(and(c, e).And(abdf.Not()))
                .Or(and(b, e).And(acdf.Not()))
                .Or(and(a, e).And(bcdf.Not()))
                .Or(and(e, f).And(abcd.Not()))
                .Or(and(d, f).And(abce.Not()))
                .Or(and(c, f).And(abde.Not()))
                .Or(and(b, f).And(acde.Not()))
                .Or(and(a, f).And(bcde.Not()));

            var one =
                a.And(bcdef.Not())
                .Or(b.And(acdef.Not()))
                .Or(c.And(abdef.Not()))
                .Or(d.And(abcef.Not()))
                .Or(e.And(abcdf.Not()))
                .Or(f.And(abcde.Not()));

            blackSet.And(one).Or(two);

            // W($"Day {i + 1}: {blackSet.BitCount()}");
        }

        return blackSet.BitCount();
    }

    public static int Part2BitSums((int x, int y)[] black, int days)
    {
        var blackInts = black.Select(c => (x: c.x + stride, y: c.y + stride))
            .Assert(c => c.x > 0 && c.x < 2 * stride && c.y > 0 && c.y < 2 * stride)
            .Select(c => c.y * 2 * stride + c.x);

        var blackSet = new BitArray(bitcount);
        blackInts.ForEach(i => blackSet.Set(i, true));

        for (int i = 0; i < days; i++)
        {
            var a = new BitArray(blackSet).RightShift(1);
            var b = new BitArray(blackSet).LeftShift(1);
            var c = new BitArray(blackSet).LeftShift(2 * stride - 1);
            var d = new BitArray(blackSet).LeftShift(2 * stride);
            var e = new BitArray(blackSet).RightShift(2 * stride - 1);
            var f = new BitArray(blackSet).RightShift(2 * stride);

            var ab_0 = xor(a, b);
            var ab_1 = a.And(b);

            var abc_0 = xor(ab_0, c);
            var abc_1 = ab_0.And(c).Or(ab_1);

            var de_0 = xor(d, e);
            var de_1 = d.And(e);

            var def_0 = xor(de_0, f);
            var def_1 = de_0.And(f).Or(de_1);

            var abcdef_0 = xor(abc_0, def_0);
            var abcdef_0c = abc_0.And(def_0);
            var abcdef_1 = xor(abc_1, def_1).Xor(abcdef_0c);
            var abcdef_1c = and(abc_1, def_1);
            var abcdef_2 = abc_1.Or(def_1).And(abcdef_0c).Or(abcdef_1c);

            var one = or(abcdef_1, abcdef_2).Not().And(abcdef_0);
            var two = abcdef_0.Or(abcdef_2).Not().And(abcdef_1);

            blackSet.And(one).Or(two);

            // W($"Day {i + 1}: {blackSet.BitCount()}");
        }

        return blackSet.BitCount();
    }


    public static int Part2BitSumsNoAlloc((int x, int y)[] black, int days)
    {
        var blackInts = black.Select(c => (x: c.x + stride, y: c.y + stride))
            .Assert(c => c.x > 0 && c.x < 2 * stride && c.y > 0 && c.y < 2 * stride)
            .Select(c => c.y * 2 * stride + c.x);


        var blackSet = new BitArray(bitcount);
        blackInts.ForEach(i => blackSet.Set(i, true));

        var a = new BitArray(bitcount);
        var b = new BitArray(bitcount);
        var c = new BitArray(bitcount);
        var d = new BitArray(bitcount);
        var e = new BitArray(bitcount);
        var f = new BitArray(bitcount);
        var g = new BitArray(bitcount);
        var h = new BitArray(bitcount);

        for (int i = 0; i < days; i++)
        {
            a.SetTo(blackSet).RightShift(1);
            b.SetTo(blackSet).LeftShift(1);
            c.SetTo(a).LeftShift(2 * stride);
            d.SetTo(blackSet).LeftShift(2 * stride);
            e.SetTo(b).RightShift(2 * stride);
            f.SetTo(blackSet).RightShift(2 * stride);

            var ab_0 = g.SetTo(a).Xor(b);
            var ab_1 = a.And(b);

            var abc_0 = b.SetTo(ab_0).Xor(c);
            var abc_1 = ab_0.And(c).Or(ab_1);

            var de_0 = c.SetTo(d).Xor(e);
            var de_1 = d.And(e);

            var def_0 = e.SetTo(de_0).Xor(f);
            var def_1 = de_0.And(f).Or(de_1);

            var abcdef_0 = f.SetTo(abc_0).Xor(def_0);
            var abcdef_0c = abc_0.And(def_0);

            var abcdef_1 = a.SetTo(abc_1).Xor(def_1).Xor(abcdef_0c);
            var abcdef_1c = e.SetTo(abc_1).And(def_1);
            var abcdef_2 = abc_1.Or(def_1).And(abcdef_0c).Or(abcdef_1c);

            var one = h.SetTo(abcdef_1).Or(abcdef_2).Not().And(abcdef_0);
            var two = abcdef_0.Or(abcdef_2).Not().And(abcdef_1);

            blackSet.And(one).Or(two);

            // W($"Day {i + 1}: {blackSet.BitCount()}");
        }

        return blackSet.BitCount();
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");
        var lines = File.ReadAllLines("input.txt");

        var black = Part1(lines);
        W($"{black.Length}");
        var result = 4200; //Part2Sets(black, 100);
        const int repeats = 10;
        const int days = 100;
        for (int i = 0; i < repeats; i++)
        {
            long allocBefore = GC.GetTotalAllocatedBytes(precise: true);
            var sw = Stopwatch.StartNew();
            var resultbits = Part2BitSumsNoAlloc(black, 100);
            W($"day time: {(double)sw.ElapsedMilliseconds / days}ms allocated: {GC.GetTotalAllocatedBytes(precise: true) - allocBefore:e}");
            Debug.Assert(result == resultbits);
        }
        W($"{result}");
    }
}
