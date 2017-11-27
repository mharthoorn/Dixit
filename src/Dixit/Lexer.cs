
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ace
{
    public class SourceFile
    {
        public string Text;

        public SourceFile(string text)
        {
            this.Text = text;
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
            return left.Text.Substring(left.Cursor, right.Cursor - left.Cursor);
        }

        public static Token CreateToken(Lexer left, Lexer right)
        {
            return new Token(Lexer.Span(left, right), left.Cursor, right.Cursor);
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

        public bool Advance(string literal)
        {
            int match = string.Compare(Text, Cursor, literal, 0, literal.Length);
            Advance(literal.Length);
            return match == 0;
        }

        public int Consumable => Cursor - Bookmark;

        private Token CreateToken()
        {
            string s = Text.Substring(Bookmark, Cursor - Bookmark);
            return new Token(s, Bookmark, Cursor - Bookmark);
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

        public void Reset(Lexer bookmark)
        {
            this = bookmark;
        }

    }
}