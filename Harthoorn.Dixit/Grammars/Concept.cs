using System;

namespace Harthoorn.Dixit
{

    public class Concept : Node
    {

        public Language Language;

        public Concept(string name, Language language) : base (name, language)
        {
            this.Language = language;
        }

        public override string ToString()
        {
            return $"{Name} (Concept)";
        }
    }
}