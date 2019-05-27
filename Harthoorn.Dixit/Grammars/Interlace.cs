namespace Harthoorn.Dixit
{
    public class Interlace : IGrammar
    {
        public string Name { get; }
        IGrammar item;
        ISyntax whitespace;
        IGrammar glue;

        public Interlace(string name, IGrammar glue, IGrammar item, ISyntax whitespace)
        {
            this.Name = name;
            this.whitespace = whitespace;
            this.item = item;
            this.glue = glue;
        }

       

        public bool Parse(ref Lexer lexer, out SyntaxNode node)
        {
            whitespace.Parse(ref lexer); // consume whitespace.

            node = new SyntaxNode(this, lexer.Here);

            bool ok = true, first = true;
            while (ok)
            {
                if (!first)
                {
                    whitespace.Parse(ref lexer);
                    
                    if (glue.Parse(ref lexer, out SyntaxNode nglue))
                    {
                        node.Append(nglue);
                    }
                    else 
                    {
                        ok = true; // we are at the end of the list
                        break; 
                    }
                }
                first = false;

                whitespace.Parse(ref lexer);
                ok = item.Parse(ref lexer, out SyntaxNode n);
                node.Append(n); // always append (or you will lose your error)
            }
            return ok;
        }

        public override string ToString()
        {
            return $"{Name} ({nameof(Interlace)})";
        }
    }
}