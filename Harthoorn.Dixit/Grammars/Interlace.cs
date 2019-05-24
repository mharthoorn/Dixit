namespace Harthoorn.Dixit
{
    public class Interlace : IGrammar
    {
        public string Name { get; }
        IGrammar item;
        ISyntax whitespace;
        ISyntax glue;

        public bool ExpectingConcept { get; } = false;

        public Interlace(string name, ISyntax glue, ISyntax whitespace)
        {
            this.Name = name;
            this.whitespace = whitespace;
            this.glue = glue;
        }

        public void Define(IGrammar item)
        {
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
            var bm = lexer;

            bool ok = true, first = true;
            while (ok)
            {
                if (!first)
                {
                    whitespace.Parse(ref lexer);
                    ok = ParseGlue(ref lexer);
                    if (!ok)
                    {
                        ok = true; // we are at the end of the list
                        break; 
                    }
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
            return $"{Name} ({nameof(Interlace)})";
        }
    }

    public class Concept : IGrammar
    {
        public string Name { get; }

        internal Language Language;

        public Concept(Language language, string name)
        {
            this.Name = name;
            this.Language = language;
        }

        public IGrammar Grammar { get; internal set; }

        public bool ExpectingConcept { get; } = true;

        public bool Parse(ref Lexer lexer, out Node node)
        {
            return this.Grammar.Parse(ref lexer, out node);
        }
    }
}