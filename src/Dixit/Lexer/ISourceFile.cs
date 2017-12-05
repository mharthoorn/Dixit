using System;

namespace Harthoorn.Dixit
{
    public interface ISourceFile
    {
        string Text { get; }
    }


    public static class FileExtensions
    {
        public static string Span(this ISourceFile file, int start, int end)
        {
            int length = Math.Min(end - start, file.Text.Length - start);
            return file.Text.Substring(start, length);
        }
    }
}