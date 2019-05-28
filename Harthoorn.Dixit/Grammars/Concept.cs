using System;

namespace Harthoorn.Dixit
{

    public class Concept : Node
    {
       
        public Concept(Language language, string name) : base (language, name)
        {
        }

        public override string ToString()
        {
            return $"{Name} (Concept)";
        }
    }
}