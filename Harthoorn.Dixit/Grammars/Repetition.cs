﻿using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public class Repetition : IGrammar
    {
        public string Name { get; }
        
        ISyntax whitespace;
        int mincount;
        IGrammar item;

        public Repetition(string name, IGrammar item, ISyntax whitespace, int mincount)
        {
            this.Name = name;
            this.whitespace = whitespace;
            this.mincount = mincount;
            this.item = item;
        }

        public bool Parse(ref Lexer lexer, out SyntaxNode node)
        {
            whitespace.Parse(ref lexer);
            int count = 0;
            
            node = new SyntaxNode(this, lexer.Here);

            while(true)
            {
                count++;
                bool parsed = item.Parse(ref lexer, out SyntaxNode n);
                if (parsed)
                {
                    node.Append(n, errorproliferation: State.Valid);
                }
                else if (count > mincount)
                {
                    n.Mark(State.Dismissed);
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

        public IEnumerable<IGrammar> GetChildren()
        {
            yield return item;
        }
    }
}