using System;
using System.Collections.Generic;
using System.Linq;

namespace Ace
{
    public enum State { Good, Bad, Error };

    public class Node
    {
        public IGrammar Grammar; 
        public ISyntax Syntax;
        public Token Token;
        public State State;
        public List<Node> Children;

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

    public static class Nodes
    {
        public static Node CreateRootSyntaxNode()
        {
            return new Node(null, null, Token.Empty());
        }

        public static Node Create(IGrammar grammar, ISyntax syntax, Token token)
        {
            var state = token.IsValid ? State.Good : State.Error;
            var node = new Node(grammar, syntax, token, state);
            return node;
        }

        public static IEnumerable<Node> GetErrors(this Node root)
        {
            return root.Descendants().Where(n => n.Token.IsValid == false);
        }

        public static IEnumerable<Node> Descendants(this Node node)
        {
            if (node.Children == null) yield break;

            foreach(var n in node.Children)
            {
                 yield return n;
                 var descendants = n.Descendants();
                 foreach (var d in descendants)
                 {
                     yield return d;
                 }
            }
        }

        public static void Visit(this Node node, Action<Node> action)
        {
            action(node);
            if (node.Children == null) return;
            foreach(var n in node.Children) Visit(n, action);
        }

        public static void Visit<T>(this Node node, Action<Node, T> action, T covariable, Func<T, T> cofunc)
        {
            action(node, covariable);
            if (node.Children == null) return;
            foreach(var n in node.Children) Visit(n, action, cofunc(covariable), cofunc);
        }
    }

  

   
}