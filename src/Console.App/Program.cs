using System;
using System.IO;
using Harthoorn.Dixit;
using Harthoorn.Dixit.Sql;
using System.Linq;

namespace SpanTest
{
    class Program
    {
        public static void DumpAst(Node root)
        {
            Console.WriteLine();
            Console.WriteLine("Dump of expression tree: ");
            root.Visit(print, 0, n => n+4);
            Console.WriteLine();

            void print(Node node, int depth) => Console.WriteLine($"{new string(' ', depth)}- {node}");
        }
 
        public static void DumpErrors(Node ast)
        {
            Console.WriteLine("Errors: ");
            foreach(var e in ast.GetErrors())
            {
                Console.WriteLine($"Error: Found {e.Token}. Expected: {e.Syntax}");
            }
        }

        public static void Compile(string text)
        {
            Console.WriteLine("Query:\n" + text);
            Console.WriteLine();

            var compiler = new SqlCompiler();
            (var ast, var success) = compiler.Compile(text);
            if (success)
            {
                Console.WriteLine("Compilation succeeded.");
                DumpAst(ast);

                var query = compiler.GetQuery(ast);
                Console.WriteLine($"Query.Fields: {string.Join(", ", query.Fields)}");
                Console.WriteLine($"Query.Resource: {string.Join(", ", query.Resource)}");
                Console.WriteLine($"Where: ");
                foreach(var filter in query.Where)
                {
                    Console.WriteLine($" - {filter.Name} {filter.Operator} {filter.Value}");
                }
            }
            else 
            {
                Console.WriteLine("Compilation failed.");
                DumpErrors(ast);
                DumpAst(ast);
            }
            
        }

        static void Main(string[] args)
        {
            if (args.Length > 0) 
            {

                string path = args[0];
                if (!Path.HasExtension(path)) path = Path.ChangeExtension(path, "sql");
                
                if (File.Exists(path)) 
                { 
                    Console.WriteLine($"Input file: {path}");
                }
                else 
                {
                    Console.WriteLine($"File not found: {path}");
                    return;
                }
                string text = File.ReadAllText(path);
                Compile(text);

                if (args.Contains("-wait"))
                {
                    Console.Write("Press any key...");
                    Console.ReadKey();
                    Console.WriteLine();
                }
            }
            else 
            {
                Console.WriteLine("File parameter missing.");
            }
        }
    }
}
