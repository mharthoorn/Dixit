
using System;

namespace Harthoorn.Dixit
{
    public class MemoryFile : ISourceFile
    {
        public string Text { get; }

        public MemoryFile(string text)
        {
            this.Text = text;
        }

    }
}