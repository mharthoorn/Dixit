namespace Harthoorn.Dixit
{
    public class Interlace : IGrammar
    {
        public string Name { get; }
        IGrammar item;
        ISyntax whitespace;
        IGrammar glue;
        int mincount;

        public Interlace(string name, IGrammar glue, IGrammar item, ISyntax whitespace, int mincount)
        {
            this.Name = name;
            this.whitespace = whitespace;
            this.item = item;
            this.glue = glue;
            this.mincount = mincount;
        }

        public bool Parse(ref Lexer lexer, out SyntaxNode node)
        {
            whitespace.Parse(ref lexer);
            int count = 0;

            node = new SyntaxNode(this, lexer.Here);
            
            while (true)
            {
                count++;

                if (count > 1)
                {
                    whitespace.Parse(ref lexer);
                    
                    if (glue.Parse(ref lexer, out SyntaxNode nglue))
                    {
                        node.Append(nglue);
                    }
                    else 
                    {
                        return true;
                    }
                }

                whitespace.Parse(ref lexer);
                bool parsed = item.Parse(ref lexer, out SyntaxNode n);
                if (parsed)
                {
                    node.Append(n, errorproliferation: State.Valid);

                }
                else if (count > mincount)
                {
                    node.Append(n, errorproliferation: State.Valid);
                    return true;
                }
                else
                {
                    // always append (or you will lose your error)
                    node.Append(n, errorproliferation: State.Error);
                    return false;
                }
                
            }
        }

        public override string ToString()
        {
            return $"{Name} ({nameof(Interlace)})";
        }
    }
}