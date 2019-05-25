using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public enum State
    {
        Valid,      // this node and all sub nodes are valid
        Dismissed,    // One of the child nodes, contains an error, but this is an optional/either branch.
        Error       // This node contains an error
    };

    public static class StateExtensions
    {
        
    }

    public class Node
    {
        public IGrammar Grammar; 
        public ISyntax Syntax;
        public Token Token;
        public State State;
        public Node Parent;
        public List<Node> Children;

        public string Text => Token.Text;

        public int Start => Token.Start;
        public int End => Token.End;
        public int Length => Token.Length;

        public Node this[int index] => Children[index];

        public Node(IGrammar grammar, Token token)
        {
            this.Grammar = grammar;
            this.Syntax = null;
            this.Token = token;
            this.State = State.Valid;
        }

        public Node(IGrammar grammar, ISyntax syntax, Token token)
        {
            this.Grammar = grammar;
            this.Syntax = syntax;
            this.Token = token;
            this.State = State.Valid;
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

        public void Append(Node child, bool dismiss = false)
        {
            InitChildren();
            if (child != null)
            {
                Children.Add(child);
                child.Parent = this;

                switch (child.State)
                {
                    case State.Valid:
                        Token.ExpandWith(child.Token); break;

                    case State.Error:
                        Token.ExpandWith(child.Token);
                        this.State = dismiss ? State.Dismissed : State.Error;
                        break;

                    case State.Dismissed:
                        break;
                }
            }
        }

        public override string ToString()
        {
            string output = $"{Grammar}";

            if (!Token.IsEmpty)
                output += $": {Token}";

                output +=  $" [{State.ToString().ToUpper()}]";

            return output;
        }
    }

  

   
}