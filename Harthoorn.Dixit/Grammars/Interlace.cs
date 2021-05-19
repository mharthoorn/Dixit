using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public class Interlace : IGrammar
    {
        public string Name { get; }
        public IGrammar Item { get; }
        ISyntax whitespace;
        public IGrammar Glue;
        int mincount;

        public Interlace(string name, IGrammar glue, IGrammar item, ISyntax whitespace, int mincount)
        {
            this.Name = name;
            this.whitespace = whitespace;
            this.Item = item;
            this.Glue = glue;
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
                    
                    if (Glue.Parse(ref lexer, out SyntaxNode nglue))
                    {
                        node.Append(nglue);
                    }
                    else 
                    {
                        return true;
                    }
                }

                whitespace.Parse(ref lexer);
                bool parsed = Item.Parse(ref lexer, out SyntaxNode n);
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

        public IEnumerable<IGrammar> GetChildren()
        {
            yield return Item;
        }
    }
}