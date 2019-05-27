using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    public enum State
    {
        Valid,      // this node and all sub nodes are valid
        Dismissed,    // One of the child nodes, contains an error, but this is an optional/either branch.
        Error       // This node contains an error
    };

    

    public class SyntaxNode
    {
        public IGrammar Grammar; 
        public ISyntax Syntax;
        public Token Token;
        public State State;
        public SyntaxNode Parent;
        public List<SyntaxNode> Children;

        public string Text => Token.Text;

        public int Start => Token.Start;
        public int End => Token.End;
        public int Length => Token.Length;

        public SyntaxNode this[int index] => Children[index];

        public SyntaxNode(IGrammar grammar, Token token)
        {
            this.Grammar = grammar;
            this.Syntax = null;
            this.Token = token;
            this.State = State.Valid;
        }

        public SyntaxNode(IGrammar grammar, ISyntax syntax, Token token)
        {
            this.Grammar = grammar;
            this.Syntax = syntax;
            this.Token = token;
            this.State = State.Valid;
        }

        public SyntaxNode(IGrammar grammar, ISyntax syntax, Token token, State state)
        {
            this.Grammar = grammar;
            this.Syntax = syntax;
            this.Token = token;
            this.State = state;
        }

        private void InitChildren()
        {
            if (Children == null) Children = new List<SyntaxNode>();
        }

        public void Append(SyntaxNode child, bool dismiss = false)
        {
            InitChildren();
            if (child is object)
            {
                Children.Add(child);
                child.Parent = this;

                switch (child.State)
                {
                    case State.Valid:
                        Token.ExpandWith(child.Token); break;

                    case State.Error:
                        this.State = dismiss ? State.Dismissed : State.Error;
                        if (State != State.Dismissed) Token.ExpandWith(child.Token);
                        break;

                    case State.Dismissed:
                        break;
                }
            }
        }

        public override string ToString()
        {
            string output = $"{Grammar}";

            //if (!Token.IsEmpty)
                output += $": {Token}";

                output +=  $" [{State.ToString().ToUpper()}]";

            return output;
        }
    }

   
}