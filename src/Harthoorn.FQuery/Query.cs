using System.Collections.Generic;

namespace Harthoorn.FQuery
{ 
    public class Query
    {
        public List<string> Fields;
        public string Resource;
        public List<Filter> Where;
    }

    public struct Filter
    {
        public string Name;
        public string Operator;
        public string Value;
    }

}