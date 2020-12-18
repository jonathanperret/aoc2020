using System;
using System.IO;
using System.Linq;
using Sprache;

public static class Program
{
    static Parser<char> Add = Parse.Char('+').Token();
    static Parser<char> Multiply = Parse.Char('*').Token();
    static Parser<long> Constant = Parse.Number.Select(long.Parse);

    static Parser<long> Factor =
        (from lparen in Parse.Char('(')
         from expr in Product
         from rparen in Parse.Char(')')
         select expr)
         .XOr(Constant);

    static Parser<long> Sum = Parse.ChainOperator(Add, Factor, (_op, a, b) => a + b);
    static Parser<long> Product = Parse.ChainOperator(Multiply, Sum, (_op, a, b) => a * b);

    public static long Compute(string expr)
    {
        return Product.Parse(expr);
    }
    public static long Solve(string[] lines)
    {
        return lines.Select(Compute).Sum();
    }

    static void Main(string[] args)
    {
        Console.WriteLine(Solve(File.ReadAllLines("input.txt")));
    }
}