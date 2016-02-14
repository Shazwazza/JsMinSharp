using System;
using System.IO;
using System.Text;

/* Over the years i've fixed various bugs that have come along, I've written unit
 * tests to show that they are solved... hopefully not causing more bugs along the
 * way. I haven't seen any other C based implementations of this with these fixes,
 * though there is a python implementation which is still actively developed...
 * though looks a whole lot different.
 * - Shannon Deminick
 */

/* Originally written in 'C', this code has been converted to the C# language.
 * The author's copyright message is reproduced below.
 * All modifications from the original to C# are placed in the public domain.
 */

/* jsmin.c
   2007-05-22

Copyright (c) 2002 Douglas Crockford  (www.crockford.com)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

The Software shall be used for Good, not Evil.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

namespace JsMinSharp
{
    public class JsMin
    {
        const int Eof = -1;

        TextReader _sr;
        TextWriter _sw;
        int _theA;
        int _theB;
        int _theLookahead = Eof;
        static int _theX = Eof;
        static int _theY = Eof;       

        public string Minify(TextReader reader)
        {
            _sr = reader;
            var sb = new StringBuilder();
            using (_sw = new StringWriter(sb))
            {
                ExecuteJsMin();
            }
            return sb.ToString();
        }

        /// <summary>
        /// jsmin -- Copy the input to the output, deleting the characters which are
        /// insignificant to JavaScript. Comments will be removed. Tabs will be
        /// replaced with spaces. Carriage returns will be replaced with linefeeds.
        /// Most spaces and linefeeds will be removed.
        /// </summary>
        private void ExecuteJsMin()
        {
            if (Peek() == 0xEF)
            {
                Get();
                Get();
                Get();
            }
            _theA = '\n';
            Action(3);
            while (_theA != Eof)
            {
                switch (_theA)
                {
                    case ' ':
                        Action(isAlphanum(_theB) ? 1 : 2);
                        break;
                    case '\n':
                        switch (_theB)
                        {
                            case '{':
                            case '[':
                            case '(':
                            case '+':
                            case '-':
                            case '!':
                            case '~':
                                Action(1);
                                break;
                            case ' ':
                                Action(3);
                                break;
                            default:
                                Action(isAlphanum(_theB) ? 1 : 2);
                                break;
                        }
                        break;
                    default:
                        switch (_theB)
                        {
                            case ' ':
                                Action(isAlphanum(_theA) ? 1 : 3);
                                break;
                            case '\n':
                                switch (_theA)
                                {
                                    case '}':
                                    case ']':
                                    case ')':
                                    case '+':
                                    case '-':
                                    case '"':
                                    case '\'':
                                    case '`':
                                        Action(1);
                                        break;
                                    default:
                                        Action(isAlphanum(_theA) ? 1 : 3);
                                        break;
                                }
                                break;
                            default:
                                Action(1);
                                break;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// action -- do something! What you do is determined by the argument:
        ///      1   Output A.Copy B to A.Get the next B.
        ///      2   Copy B to A. Get the next B. (Delete A).
        ///      3   Get the next B. (Delete B).
        /// action treats a string as a single character.Wow!
        /// action recognizes a regular expression if it is preceded by(or , or =.
        /// </summary>
        /// <param name="d"></param>
        void Action(int d)
        {
            switch (d)
            {
                case 1:
                    Put(_theA);
                    if (
                        (_theY == '\n' || _theY == ' ') &&
                        (_theA == '+' || _theA == '-' || _theA == '*' || _theA == '/') &&
                        (_theB == '+' || _theB == '-' || _theB == '*' || _theB == '/')
                        )
                    {
                        Put(_theY);
                    }
                    goto case 2;
                case 2:
                    _theA = _theB;
                    if (_theA == '\'' || _theA == '"' || _theA == '`')
                    {
                        //This is a string literal...
                        for (;;)
                        {
                            Put(_theA);
                            _theA = Get();
                            if (_theA == _theB)
                            {
                                break;
                            }
                            //check for escaped chars
                            if (_theA == '\\')
                            {
                                Put(_theA);
                                _theA = Get();
                            }
                            if (_theA == Eof)
                            {
                                throw new Exception(string.Format("Error: JSMIN unterminated string literal: {0}\n", _theA));
                            }
                        }
                    }
                    goto case 3;
                case 3:
                    _theB = Next();

                    //This is supposed to be testing for regex literals, however it doesn't actually work in many cases,
                    // for example see this bug report: https://github.com/douglascrockford/JSMin/issues/11
                    // or this: https://github.com/Shazwazza/ClientDependency/issues/73                    
                    if (_theB == '/')
                    {
                        //This is the original logic from JSMin, but it doesn't cater for the above issue mentioned
                        if (_theA == '(' || _theA == ',' || _theA == '=' || _theA == ':' ||
                            _theA == '[' || _theA == '!' || _theA == '&' || _theA == '|' ||
                            _theA == '?' || _theA == '+' || _theA == '-' || _theA == '~' ||
                            _theA == '*' || _theA == '/' || _theA == '{' || _theA == '\n' ||
                            //We've now added these additional characters and tests pass, the 'n' is specifically relating
                            // to the term 'return', the space is there because a regex literal can always begin after a space
                            _theA == '+' || _theA == 'n' || _theA == ' ')
                        {
                            Put(_theA);
                            if (_theA == '/' || _theA == '*')
                            {
                                Put(' ');
                            }
                            Put(_theB);
                            for (;;)
                            {
                                _theA = Get();
                                if (_theA == '[')
                                {
                                    for (;;)
                                    {
                                        Put(_theA);
                                        _theA = Get();
                                        if (_theA == ']')
                                        {
                                            break;
                                        }
                                        if (_theA == '\\')
                                        {
                                            Put(_theA);
                                            _theA = Get();
                                        }
                                        if (_theA == Eof)
                                        {
                                            throw new Exception(string.Format("Error: JSMIN Unterminated set in Regular Expression literal: {0}\n", _theA));
                                        }
                                    }
                                }
                                else if (_theA == '/')
                                {
                                    switch (Peek())
                                    {
                                        case 'i':
                                        case 'g':
                                            //regex modifiers, do we care?
                                            break;
                                        case ' ':
                                            //skip the space
                                            Put(_theA);
                                            Get();
                                            _theA = Get();
                                            break;
                                        case '/':
                                        case '*':
                                            throw new Exception(string.Format("Error: JSMIN Unterminated set in Regular Expression literal: {0}\n", _theA));
                                    }
                                    break;
                                }
                                else if (_theA == '\\')
                                {
                                    Put(_theA);
                                    _theA = Get();
                                }
                                if (_theA == Eof)
                                {
                                    throw new Exception(string.Format("Error: JSMIN Unterminated Regular Expression literal: {0}\n", _theA));
                                }
                                Put(_theA);
                            }
                            _theB = Next();
                        }
                    }
                    goto default;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// next -- get the next character, excluding comments. peek() is used to see
        ///  if a '/' is followed by a '/' or '*'.
        /// </summary>
        /// <returns></returns>
        private int Next()
        {
            int c = Get();
            if (c == '/')
            {
                switch (Peek())
                {
                    case '/':
                        for (;;)
                        {
                            c = Get();
                            if (c <= '\n')
                            {
                                break;
                            }
                        }
                        break;
                    case '*':
                        Get();
                        while (c != ' ')
                        {
                            switch (Get())
                            {
                                case '*':
                                    var currPeek = Peek();
                                    if (currPeek == '/')
                                    {
                                        Get();
                                        c = ' ';

                                        //In one very peculiar circumstance, if the JS value is like:
                                        // val(1 /* Calendar */.toString());
                                        // if we strip the comment out, JS will produce an error because
                                        // 1.toString() is not valid, however 1..toString() is valid and 
                                        // similarly keeping the comment is valid. So we can check if the next value
                                        // is a '.' and if the current value is numeric and perform this operation.
                                        // The reason why .. works is because the JS parser cannot do 1.toString() because it 
                                        // sees the '.' as a decimal

                                        if (char.IsDigit((char)_theY))
                                        {
                                            currPeek = Peek();
                                            if (currPeek == '.')
                                            {
                                                //we actually want to write another '.'
                                                return '.';
                                            }
                                        }

                                    }
                                    break;
                                case Eof:
                                    throw new Exception("Error: JSMIN Unterminated comment.\n");
                            }
                        }
                        break;
                }
            }
            //return c;
            _theY = _theX;
            _theX = c;
            return c;
        }

        /// <summary>
        /// peek -- get the next character without getting it.
        /// </summary>
        /// <returns></returns>
        int Peek()
        {
            _theLookahead = Get();
            return _theLookahead;
        }

        /// <summary>
        /// get -- return the next character from stdin. Watch out for lookahead. If
        /// the character is a control character, translate it to a space or
        /// linefeed.
        /// </summary>
        /// <returns></returns>
        int Get()
        {
            int c = _theLookahead;
            _theLookahead = Eof;
            if (c == Eof)
            {
                c = _sr.Read();
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

        void Put(int c)
        {
            _sw.Write((char)c);
        }

        /// <summary>
        /// isAlphanum -- return true if the character is a letter, digit, underscore,
        /// dollar sign, or non-ASCII character.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool isAlphanum(int c)
        {
            return ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') ||
                    (c >= 'A' && c <= 'Z') || c == '_' || c == '$' || c == '\\' ||
                    c > 126);
        }

    }
}