namespace Harthoorn.Dixit
{
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
                    whitespace.Parse(ref lexer);
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

}