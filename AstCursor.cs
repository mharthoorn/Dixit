using System.Collections.Generic;
using System.Linq;

namespace Ace
{

    public struct NodeWriter 
    {
        Node Parent;

        public NodeWriter(Node node)
        {
            Parent = node;
        }

        public void Append(Node node)
        {
            Parent.Append(node);
        }
       
        public NodeWriter Clone()
        {
            return new NodeWriter { Parent = this.Parent };
        }

        public void Undo(Node node)
        {
            this.Parent.Children.Remove(node);
        }

        private void ClearChildren()
        {
            this.Parent.Children = new List<Node>();
        }

        public void Nest(Node node)
        {
            Parent = node;
        }

    }
}