using System;
using System.Collections.Generic;

namespace Harthoorn.Dixit
{
    //public class Language : IGrammar
    //{
    //    public string Name { get; private set; }

    //    public Language(string name)
    //    {
    //        this.Name = name;
    //    }



    //    public bool Parse(ref Lexer lexer, out SyntaxNode node)
    //    {
    //        return Grammar.Parse(ref lexer, out node);
    //    }
    //}

    public class LanguageChecker
    {
        List<IGrammar> CheckList = new List<IGrammar>();

        public void Test(IGrammar grammar)
        {
            if (CheckList.Contains(grammar)) return;
            
            CheckList.Add(grammar);

            if (grammar is Concept c && c.Grammar is null)
            {
                Console.WriteLine($"Concept '{c.Name}' is not defined.");
            }
            else 
            {
                foreach(var n in grammar.GetChildren())
                {
                    Test(n);
                }
            }
        }
    }

}