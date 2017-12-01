using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public enum State { Good, Bad, Error };

    public class Node
    {
        public IGrammar Grammar; 
        public ISyntax Syntax;
        public Token Token;
        public State State;
        public List<Node> Children;

        public Node this[int index] => Children[index];
        
        public Node(IGrammar grammar, Token token)
        {
            this.Grammar = grammar;
            this.Syntax = null;
            this.Token = token;
            this.State = State.Good;
        }
        public Node(IGrammar grammar, ISyntax syntax, Token token)
        {
            this.Grammar = grammar;
            this.Syntax = syntax;
            this.Token = token;
            this.State = State.Good;
        }

        public Node(IGrammar grammar, ISyntax syntax, Token token, State state)
        {
            this.Grammar = grammar;
            this.Syntax = syntax;
            this.Token = token;
            this.State = state;
        }

        private void InitChildren()
        {
            if (Children == null) Children = new List<Node>();
        }

        public void Append(Node node)
        {
            InitChildren();
            Children.Add(node);
        }

        public override string ToString()
        {
            string output = $"{Grammar}: {Token}";
            if (State != State.Good) output +=  $" [{State}]";
            return output;
        }
    }

  

   
}