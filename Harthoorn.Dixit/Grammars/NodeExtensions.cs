namespace Harthoorn.Dixit
{
    public static class NodeExtensions
    {
        public static void RebaseTo(this SyntaxNode node, Concept grammar)
        {
            if (node.Token.IsValid)
            {
                if (!(node.Grammar is Concept))
                    node.Grammar = grammar;
            }
        }
    }
   
}