using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public enum State { Good, Invalid, Error };

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

        public Node(IGrammar grammar)
        {
            this.Grammar = grammar;
            this.Syntax = null;
            this.Token = default(Token);
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
            if (node != null)
            {
                Children.Add(node);
                this.Expand(node);
                if (node.State != State.Good) this.State = State.Invalid;
            }
        }

        public void Fail(Node node)
        {

        }

        public void Expand(Node node)
        {
            this.Token.Expand(node.Token);
        }

        

        public override string ToString()
        {
            string output = $"{Grammar}";
            if (!Token.IsEmpty) output += $": {Token}";
            if (State != State.Good) output +=  $" [{State}]";
            return output;
        }
    }

  

   
}