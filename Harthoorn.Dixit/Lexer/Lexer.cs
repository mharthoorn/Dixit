
using System;

namespace Harthoorn.Dixit
{

    public struct Lexer
    {
        public ISourceFile File;
        
        public string Text => File.Text;
        public char Current => (head < File.Text.Length) ? File.Text[head] : '\0';

        private int head;
        private int cursor;

        public Lexer(ISourceFile file, int cursor)
        {
            this.File = file;
            this.cursor = cursor;
            this.head = cursor;
        }
        public Lexer Clone()
        {
            return new Lexer { File = File, head = head, cursor = cursor };
        }

        public Lexer(ISourceFile file) : this(file, 0) { }

        public static string Span(Lexer left, Lexer right)
        {
            var file = left.File;
            return file.Span(left.head, right.head);
        }

        public bool Advance(char character)
        {
            bool ok = (Current == character);
            if (ok) Advance();
            return ok;
        }

        public bool Advance(int count = 1)
        {
            if (Text.Length >= head + count)
            {
                head += count;
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Advance(Predicate<char> predicate)
        {
            int i = 0;
            
            while (predicate(Current))
            {
                i++;
                var advance = Advance();
                if (!advance) break; 
            }
            return i;
        }

        public bool Advance(string literal, bool ignoreCase = false)
        {
            int match = string.Compare(Text, head, literal, 0, literal.Length, ignoreCase);
            Advance(literal.Length);
            return match == 0;
        }

        public int Consumable => head - cursor;

        public Token Capture(bool valid)
        {
            var token = new Token(File, cursor, head, valid);
            if (valid)
            {
                this.cursor = this.head;
            }
            else
            {
                this.head = this.cursor;
            }
            return token;
        }

        public Token Here 
        {
            get
            {
                if (this.head != this.cursor) throw new InvalidOperationException("The lexer was in an ambiguous state.");
                return new Token(this.File, this.head, this.head, valid: true);
            }
        }

        public bool IsAtEnd => this.Text.Length <= this.cursor;
        

        public override string ToString()
        {
            var start = Math.Max(0, this.cursor-10);
            var stop = Math.Min(File.Text.Length, this.head + 30);

            var cpos = this.cursor - start;
            var hpos = this.head - start;
            string s = this.File.Span(start, stop);
            s = s.Insert(hpos, "|").Insert(cpos, "|"); // in this order, because they shift
            return "..." + s + "...";
        }

    }

    public static class LexerExtensions
    {
        public static Token CaptureIfAdvanced(this Lexer lexer)
        {
            return lexer.Capture(lexer.Consumable > 0);
        }

        public static Token Consume(this Lexer lexer, int n, bool valid)
        {
            var ok = lexer.Advance(n);
            return lexer.Capture(ok);
        }
    }
}