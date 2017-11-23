using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ace;
using Ace.Sql;

namespace SpanTest
{
    class Program
    {
        public static void DumpAst(Node root)
        {
            Console.WriteLine("AST: ");
            root.Visit(print, 0, n => n+1);

            void print(Node node, int depth) => Console.WriteLine($"{new string(' ', depth*2)}- {node}");
        }

        public static void DumpErrors(Node ast)
        {
            Console.WriteLine("Errors: ");
            foreach(var e in ast.GetErrors())
            {
                Console.WriteLine($"Error: Found {e.Token}. Expected: {e.Syntax}");
            }
        }

        public static void Test(string text)
        {
            var compiler = new SqlCompiler();
            (var ast, var success) = compiler.Compile(text);
            if (success)
            {
                Console.WriteLine("Compilation succeeded.");
            }
            else 
            {
                Console.WriteLine("Compilation failed.");
                DumpErrors(ast);
            }
            DumpAst(ast);
        }

        static void Main(string[] args)
        {
            if (args.Length > 0) 
            {
                string path = args[0];
                Console.WriteLine($"Input file: {path}");
                string text = File.ReadAllText(path);
                Test(text);
            }
            else 
            {
                Console.WriteLine("File parameter missing.");
            }
        }
    }
}
