using System;
using System.IO;
using Harthoorn.Dixit;
using Harthoorn.FQuery;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace SpanTest
{
    class Program
    {

        public static string Project(Node node)
        {
            var b = new StringBuilder();
            
            visit(node);

            return b.ToString();

            void visitall(IEnumerable<Node> nodes)
            {
                if (nodes == null) return;

                foreach (var n in nodes)
                {
                    visit(n);
                }
            }

            void fieldlist(IEnumerable<Node> nodes)
            {
                if (nodes == null) return;
                var first = true;

                foreach (var n in nodes)
                {
                    if (!first) b.Append(",\n");
                    first = false;
                    visit(n);
                }
            }

            void visit(Node n) 
            { 
                if (n.Grammar == FQL.FieldName)
                {
                    b.Append(n.Text + ": ");
                }
                else if (n.Grammar == FQL.FieldExpression)
                {
                    b.Append(n.Text);
                }
                else if (n.Grammar == FQL.Object)
                {
                    b.Append("{\n");
                    visitall(n.Children);
                    b.Append("\n}");
                }
                else if (n.Grammar == FQL.FieldList)
                {
                    fieldlist(n.Children);
                }
                else
                {
                    visitall(n.Children);
                }
            }

        }

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
            foreach(var e in ast.GetErrors()) Console.WriteLine($"Error: Found {e.Token}. Expected: {e.Syntax}");
        }

        public static void DumpQuery(Query query)
        {
            Console.WriteLine($"Query.Fields: {string.Join(", ", query.Fields)}");
            Console.WriteLine($"Query.Resource: {string.Join(", ", query.Resource)}");
            Console.WriteLine($"Where: " + string.Join(", ", query.Where.Select(f => $"({f.Name} {f.Operator} {f.Value})")));
        }

        public static void Compile(string text)
        {
            Console.WriteLine("Query:\n" + text);
            Console.WriteLine();

            var compiler = new FQLCompiler();
            var success = compiler.Compile(text, out Node ast);

            if (success)
            {
                Console.WriteLine("Compilation succeeded.");
                //DumpAst(ast);
                var query = compiler.ConstructQuery(ast);
                DumpQuery(query);
                var fieldlist = ast.Find(FQL.FieldList);
                var result = Project(fieldlist);
                Console.WriteLine(result);



            }
            else 
            {
                Console.WriteLine("Compilation failed.");
                DumpErrors(ast);
                DumpAst(ast);
            }
        }

        static bool TryGetFileText(string path, out string text)
        {
            if (!Path.HasExtension(path)) path = Path.ChangeExtension(path, "sql");

            if (File.Exists(path))
            {
                Console.WriteLine($"Input file: {path}");
                text = File.ReadAllText(path);
                return true;
            }
            else
            {
                Console.WriteLine($"File not found: {path}");
                text = null;
                return false;
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("You need to provide a filename.");
                return;
            }
            
            if (TryGetFileText(args[0], out string text))
            {
                Compile(text);
            }
          
            if (args.Contains("-wait"))
            {
                Console.Write("Press any key...");
                Console.ReadKey();
                Console.WriteLine();
            }
        }
    }
}
