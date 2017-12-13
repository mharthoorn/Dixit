using System.Collections.Generic;

namespace Harthoorn.Dixit.Sql
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