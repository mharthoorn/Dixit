
using System;
using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Dixit
{
    public class SourceFile
    {
        public string Text;

        public SourceFile(string text)
        {
            this.Text = text;
        }

        public string Span(int start, int end)
        {
            int length = Math.Min(end - start, Text.Length - start);
            return Text.Substring(start, length);
        }
    }

    public struct Lexer
    {
        public SourceFile File;
        public int Bookmark;
        public int Cursor;

        public string Text => File.Text;
        public char Current 
        {
            get {
                if (Cursor < File.Text.Length) return File.Text[Cursor]; 
                else return '\0';
            }
        }

        public Lexer(SourceFile file, int cursor)
        {
            this.File = file;
            this.Bookmark = cursor;
            this.Cursor = cursor;
        }
        public Lexer Clone()
        {
            return new Lexer { File = File, Cursor = Cursor, Bookmark = Bookmark };
        }

        public Lexer(SourceFile file) : this(file, 0) { }

        public static string Span(Lexer left, Lexer right)
        {
            var file = left.File;
            return file.Span(left.Cursor, right.Cursor);
        }

        public static Token CreateToken(Lexer left, Lexer right)
        {
            return new Token(left.File, left.Cursor, right.Cursor);
        }

        public bool Advance(char character)
        {
            bool ok = (Current == character);
            if (ok) Advance();
            return ok;
        }
        public bool Advance(int count = 1)
        {
            if (Text.Length >= Cursor + count)
            {
                Cursor += count;
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
            int match = string.Compare(Text, Cursor, literal, 0, literal.Length, ignoreCase);
            Advance(literal.Length);
            return match == 0;
        }

        public int Consumable => Cursor - Bookmark;

        private Token CreateToken()
        {
            return new Token(this.File, Bookmark, Cursor);
        }

        public Token Consume()
        {
            var token = CreateToken();
            this.Bookmark = this.Cursor;
            return token;
        }

        public Token Consume(int n)
        {
            Advance(n);
            return Consume();
        }

        public Token Append(Token A, Token B)
        {
            return new Token(A.File, A.Start, B.End);
        }

        public Token Consume(Token B)
        { 
            this.Bookmark = B.End;
            return new Token(this.File, this.Cursor, B.End);
        }

        public Token Consume(Lexer previous, Token last)
        { 
            var start = previous.Bookmark;
            var end = last.End;
            this.Bookmark = this.Cursor;
            return new Token(this.File, start, end);
        }

        public static Token Encapsulate(Lexer previous, Lexer current)
        {
            return new Token(previous.File, previous.Current, current.Current);
        }

        public void Reset(Lexer bookmark)
        {
            this = bookmark;
        }

        public Token Here => new Token(this.File, this.Current, this.Current);

        public override string ToString()
        {
            string substring = this.File.Span(this.Bookmark, this.Current);
            return "...|" + substring + "...";
        }

    }
}