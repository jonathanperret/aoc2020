using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;

namespace cli
{

    public static class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("day4.txt");

            string[] required = {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid",
             };
            int result = lines.Segment(line => string.IsNullOrEmpty(line)).Select(ls => ls.ToDelimitedString(" "))
            .Select(p => p.Trim().Split(" "))
            .Where(kvs => kvs.Select(kv => kv.Split(":")[0]).Intersect(required).Count() == required.Count())
            .Where(kvs => kvs.All(kv =>
            {
                var (k, v) = (kv.Split(":"));

                int x;
                switch (k)
                {
                    case "byr": // (Birth Year) - four digits; at least 1920 and at most 2002.
                        return int.TryParse(v, out x) && x >= 1920 && x <= 2002;
                    case "iyr": // (Issue Year) - four digits; at least 2010 and at most 2020.
                        return int.TryParse(v, out x) && x >= 2010 && x <= 2020;
                    case "eyr": // (Expiration Year) - four digits; at least 2020 and at most 2030.
                        return int.TryParse(v, out x) && x >= 2020 && x <= 2030;
                    case "hgt": // (Height) - a number followed by either cm or in:
                                //cm, the number must be at least 150 and at most 193.
                                //in, the number must be at least 59 and at most 76.
                        var (num, unit) = v.Partition(c => char.IsDigit(c));
                        switch (unit.ToDelimitedString(""))
                        {
                            case "cm":
                                return int.TryParse(num.ToDelimitedString(""), out x) && x >= 150 && x <= 193;
                            case "in":
                                return int.TryParse(num.ToDelimitedString(""), out x) && x >= 59 && x <= 76;
                            default:
                                return false;
                        }
                    case "hcl": // (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                        return v[0] == '#' && v.Length == 7 && v.Substring(1).All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f'));
                    case "ecl": // (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                        return v == "amb" || v == "blu" || v == "brn" || v == "gry" || v == "grn" || v == "hzl" || v == "oth";
                    case "pid": // (Passport ID) - a nine-digit number, including leading zeroes.
                        return v.Length == 9 && v.All(c => char.IsDigit(c));
                    case "cid": // (Country ID) - ignored, missing or not.
                        return true;
                    default:
                        return false;
                }
            }))
            .Pipe(kvs => Console.WriteLine(kvs.OrderBy(kv => kv).ToDelimitedString("\t")))
            .Count();
            // .ForEach(v => Console.WriteLine(v.ToDelimitedString(",")));

            Console.WriteLine($"{result}");
        }
    }
}
