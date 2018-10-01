using System.Linq;

namespace Harthoorn.Dixit
{
    public static class SyntaxHelper
    {
        public static char[] CharArray(params string[] charsets)
        {
            return charsets.SelectMany(s => s.ToCharArray()).ToArray();
        }
        
    }


}