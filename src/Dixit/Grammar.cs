using System.Collections.Generic;
using System.Linq;

namespace Ace
{

    public interface IGrammar
    {
        string Name { get; }
        bool Parse(ref Lexer lexer, out Node node);
    }

    public class Unit : IGrammar
    {
        public string Name { get; }
        ISyntax syntax;

        public Unit(string name, ISyntax syntax)
        {
            this.Name = name;
            this.syntax = syntax;
        }

        public bool Parse(ref Lexer lexer, out Node node)
        {
            var token = syntax.Parse(ref lexer);
            node = Nodes.Create(this, syntax, token);
            return token.IsValid;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public class Sequence : IGrammar
    {
        public string Name { get; }
        List<IGrammar> list;
        ISyntax whitespace;

        public Sequence(string name, IEnumerable<IGrammar> items, ISyntax whitespace)
        {
            this.Name = name;
            this.whitespace = whitespace;
            list = items.ToList();
        }


        public bool Parse(ref Lexer lexer, out Node node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.

            node = new Node(this, lexer.Consume());
            

            foreach (var grammar in list)
            {
                whitespace.Parse(ref lexer);

                bool ok = grammar.Parse(ref lexer, out Node n);
                if (ok) node.Append(n); else return false;
            }
            return true;
        }

        public override string ToString()
        {
            return $"{Name}({nameof(Sequence)})";
        }
    }

    public class Any : IGrammar
    {
        public string Name { get; }
        List<IGrammar> items;
        ISyntax whitespace;

        public Any(string name, IEnumerable<IGrammar> items, ISyntax whitespace)
        {
            this.Name = name;
            this.whitespace = whitespace;
            this.items = items.ToList();
        }

        public bool Parse(ref Lexer lexer, out Node node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.

            node = new Node(this, lexer.Consume());
            var lexerbm = lexer;

            foreach (var grammar in items)
            {
                lexer.Reset(lexerbm);

                bool ok = grammar.Parse(ref lexer, out Node n);
                if (ok)
                {
                    node.Append(n);
                    return true;
                }

            }
            node.State = State.Error;
            return false;
        }

        public override string ToString()
        {
            return $"{Name}({nameof(Any)})";
        }
    }

    public class GluedSequence : IGrammar
    {
        public string Name { get; }
        IGrammar item;
        ISyntax whitespace;
        ISyntax glue;

        public GluedSequence(string name, IGrammar item, ISyntax glue, ISyntax whitespace)
        {
            this.Name = name;
            this.whitespace = whitespace;
            this.glue = glue;
            this.item = item;
        }

        private bool ParseGlue(ref Lexer lexer)
        {
            var bookmark = lexer;

            whitespace.Parse(ref lexer);
            var token = glue.Parse(ref lexer);
            if (token.IsValid)
            {
                return true;
            }
            else
            {
                lexer.Reset(bookmark);
                return false;

            }
        }

        public bool Parse(ref Lexer lexer, out Node node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.

            node = new Node(this, lexer.Consume());

            bool ok = true, first = true;
            while (ok)
            {
                if (!first)
                {
                    ok = ParseGlue(ref lexer);
                    if (!ok) return true; // we are at the end of the list
                }
                first = false;

                whitespace.Parse(ref lexer);
                ok = item.Parse(ref lexer, out Node n);
                node.Append(n); // always append (or you will lose your error)
            }
            return ok;
        }

        public override string ToString()
        {
            return $"{Name}({nameof(GluedSequence)})";
        }
    }

    public class Optional : IGrammar
    {
        public string Name {get;}
        IGrammar grammar;

        public Optional(string name, IGrammar grammar)
        {
            this.Name = name;
            this.grammar = grammar;
        }

        public bool Parse(ref Lexer lexer, out Node node)
        {
            var bookmark = lexer;
            node = new Node(this, Token.Empty());

            var ok = grammar.Parse(ref lexer, out Node n);
            if (ok) // could be improved by: if we're too deep, the optional is probably the case, but just wrong. It's not misbranched
            {
                node.Append(n);
            }
            return true;
        }


    }

}