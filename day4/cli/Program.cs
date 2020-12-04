using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;
using System.Text.RegularExpressions;

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

            int x;

            (string pattern, Func<string[], bool> check)[] rules = {
                ("^byr:([0-9]{4})$", cs => int.TryParse(cs[0], out x) && x >= 1920 && x <= 2002), // (Birth Year) - four digits; at least 1920 and at most 2002.
                ("^iyr:([0-9]{4})$", cs => int.TryParse(cs[0], out x) && x >= 2010 && x <= 2020), // (Issue Year) - four digits; at least 2010 and at most 2020.
                ("^eyr:([0-9]{4})$", cs => int.TryParse(cs[0], out x) && x >= 2020 && x <= 2030), // (Expiration Year) - four digits; at least 2020 and at most 2030.
                ("^hgt:([0-9]+)(cm|in)$", cs => int.TryParse(cs[0], out x) && x >= (cs[1]=="cm" ? 150:59) && x <= (cs[1]=="cm" ? 193:76)), // (Height) - a number followed by either cm or in:
                                //cm, the number must be at least 150 and at most 193.
                                //in, the number must be at least 59 and at most 76.
                ("^hcl:(#[0-9a-f]{6})$", _ => true), // (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                ("^ecl:(amb|blu|brn|gry|grn|hzl|oth)$", _ => true), // (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                ("^pid:([0-9]{9})$", _ => true), // (Passport ID) - a nine-digit number, including leading zeroes.
                ("^cid:.*$", _ => true), // (Country ID) - ignored, missing or not.
            };
            int result = lines.Segment(line => string.IsNullOrEmpty(line)).Select(ls => ls.ToDelimitedString(" "))
            .Select(p => p.Trim().Split(" "))
            .Where(kvs => kvs.Select(kv => kv.Split(":")[0]).Intersect(required).Count() == required.Count())
            .Where(kvs => kvs.All(kv => rules.Any(rule =>
            {
                var m = Regex.Match(kv, rule.pattern);
                if (!m.Success)
                {
                    Console.WriteLine($"{kv} fails regex {rule.pattern}");
                    return false;
                }
                bool checkd = rule.check(m.Groups.Values.Skip(1).Select(g => g.Value).ToArray());
                if (!checkd)
                {
                    Console.WriteLine($"{kv} fails check");
                }
                return checkd;
            })))
            .Pipe(kvs => Console.WriteLine(kvs.OrderBy(kv => kv).ToDelimitedString("\t")))
            .Count();

            Console.WriteLine($"{result}");
        }
    }
}
