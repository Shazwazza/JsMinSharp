using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsMinSharp
{
    public class CssMin : IMinifier
    {
        private const int Eof = -1;

        private TextReader _tr;
        private StringBuilder _sb;
        int _theA;
        int _theB;
        int _theLookahead = Eof;


        /// <summary>
        /// Minify the input script
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public string Minify(TextReader reader)
        {
            _sb = new StringBuilder();
            _tr = reader;
            _theA = '\n';
            _theB = 0;
            _theLookahead = Eof;
            ExecuteCssMin();
            return _sb.ToString();
        }

        /// <summary>
        /// Excute the actual minify
        /// </summary>
        private void ExecuteCssMin()
        {
            Action(3);
            while (_theA != Eof)
            {
                switch (_theA)
                {
                    case ' ':
                        {
                            switch (_theB)
                            {
                                case ' ':        //body.Replace("  ", String.Empty);
                                case '{':        //body = body.Replace(" {", "{");
                                case ':':        //body = body.Replace(" {", "{");
                                case '\n':       //body = body.Replace(" \n", "\n");
                                case '\r':       //body = body.Replace(" \r", "\r");
                                case '\t':       //body = body.Replace(" \t", "\t");
                                    Action(2);
                                    break;
                                default:
                                    Action(1);
                                    break;
                            }
                            break;
                        }
                    case '\t':              //body = body.Replace("\t", "");
                    case '\r':              //body = body.Replace("\r", "");
                        Action(2);
                        break;
                    case '\n':              //body = body.Replace("\n", "");
                        if (char.IsWhiteSpace((char)_theB))
                        {
                            //skip over whitespace
                            Action(3);
                        }
                        else
                        {
                            //convert the line break to a space except when in the beginning
                            //TODO: this isn't the best place to put this logic since all puts are done
                            // in the action, but i don't see any other way to do this,
                            //we could set theA = ' ' and call action(1) ?
                            if (_sb.Length > 0) Put(' ');
                            Action(2);
                        }
                        break;
                    case '}':
                    case '{':
                    case ':':
                    case ',':
                    case ';':
                        //skip over whitespace
                        Action(char.IsWhiteSpace((char)_theB) ? 3 : 1);
                        break;
                    default:
                        Action(1);
                        break;
                }
            }
        }
        /* action -- do something! What you do is determined by the argument:
                1   Output A. Copy B to A. Get the next B.
                2   Copy B to A. Get the next B. (Delete A).
                3   Get the next B. (Delete B).
        */

        private void Action(int d)
        {
            if (d <= 1)
            {
                Put(_theA);
            }
            if (d <= 2)
            {
                _theA = _theB;
                if (_theA == '\'' || _theA == '"')
                {
                    for (;;)
                    {
                        Put(_theA);
                        _theA = Get();
                        if (_theA == _theB)
                        {
                            break;
                        }
                        if (_theA <= '\n')
                        {
                            throw new FormatException(string.Format("Error: unterminated string literal: {0}\n", _theA));
                        }
                        if (_theA == '\\')
                        {
                            Put(_theA);
                            _theA = Get();
                        }
                    }
                }
            }
            if (d <= 3)
            {
                _theB = Next();
                if (_theB == '/' && (_theA == '(' || _theA == ',' || _theA == '=' ||
                                    _theA == '[' || _theA == '!' || _theA == ':' ||
                                    _theA == '&' || _theA == '|' || _theA == '?' ||
                                    _theA == '{' || _theA == '}' || _theA == ';' ||
                                    _theA == '\n'))
                {
                    Put(_theA);
                    Put(_theB);
                    for (;;)
                    {
                        _theA = Get();
                        if (_theA == '/')
                        {
                            break;
                        }
                        else if (_theA == '\\')
                        {
                            Put(_theA);
                            _theA = Get();
                        }
                        else if (_theA <= '\n')
                        {
                            throw new FormatException(string.Format("Error: unterminated Regular Expression literal : {0}.\n", _theA));
                        }
                        Put(_theA);
                    }
                    _theB = Next();
                }
            }
        }
        /* next -- get the next character, excluding comments. peek() is used to see
                if a '/' is followed by a '*'.
        */

        private int Next()
        {
            int c = Get();
            if (c == '/')
            {
                switch (Peek())
                {
                    case '*':
                        {
                            Get();
                            for (;;)
                            {
                                switch (Get())
                                {
                                    case '*':
                                        {
                                            if (Peek() == '/')
                                            {
                                                Get();
                                                return ' ';
                                            }
                                            break;
                                        }
                                    case Eof:
                                        {
                                            throw new FormatException("Error: Unterminated comment.\n");
                                        }
                                }
                            }
                        }
                    default:
                        {
                            return c;
                        }
                }
            }
            return c;
        }
        /* peek -- get the next character without getting it.
        */

        private int Peek()
        {
            _theLookahead = Get();
            return _theLookahead;
        }
        /* get -- return the next character from stdin. Watch out for lookahead. If
                the character is a control character, translate it to a space or
                linefeed.
        */

        private int Get()
        {
            int c = _theLookahead;
            _theLookahead = Eof;
            if (c == Eof)
            {
                c = _tr.Read();
            }
            if (c >= ' ' || c == '\n' || c == Eof)
            {
                return c;
            }
            if (c == '\r')
            {
                return '\n';
            }
            return ' ';
        }

        private void Put(int c)
        {
            _sb.Append((char)c);
        }
    }

}
