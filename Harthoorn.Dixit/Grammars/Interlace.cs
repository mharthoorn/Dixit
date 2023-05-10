using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

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

        public bool Parse(ref Lexer lexer, out SyntaxNode interlace)
        {
            whitespace.Parse(ref lexer);
            int count = 0;
            
            interlace = new SyntaxNode(this, lexer.Here);
            var lx = lexer.Clone();
            while (true)
            {
                SyntaxNode glue = null, item;
                
                if (count > 0)
                {
                    glue = ParseGlue(ref lx);
                    if (glue is null) break;
                }

                item = ParseItem(ref lx);
                if (item is null) break;
                
                // if they are both there
                if (count > 0) interlace.Append(glue);
                interlace.Append(item);
                lexer = lx;
                count++;
            }

            if (count >= mincount) return true; 
            else return false;

            //if (parsed)
            //{
            //    node.Append(item, errorproliferation: State.Valid);

            //}
            //else if (count > mincount)
            //{
            //    node.Append(item, errorproliferation: State.Valid);
            //    return true;
            //}
            //else
            //{
            //    // always append (or you will lose your error)
            //    node.Append(item, errorproliferation: State.Error);
            //    return false;
            //}

        }

        SyntaxNode ParseGlue(ref Lexer lexer)
        {
            whitespace.Parse(ref lexer);
            if (Glue.Parse(ref lexer, out SyntaxNode glue))
            {
                return glue;
            }
            else
            {
                return null;
            }

        }
        SyntaxNode ParseItem(ref Lexer lexer)
        {
            whitespace.Parse(ref lexer);
            if (Item.Parse(ref lexer, out SyntaxNode item))
            {
                return item;
            }
             else
            {
                return null;
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