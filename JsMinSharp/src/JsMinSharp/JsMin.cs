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
        private const int Eof = -1;
        private TextReader _sr;
        private TextWriter _sw;
        private int _theA;
        private int _theB;
        private int _theLookahead = Eof;
        private int _theX = Eof;
        private int _theY = Eof;

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
                        Action(IsAlphanum(_theB) ? 1 : 2);
                        break;
                    case '\n':
                        Action(2);

                        //TODO: I don't understand why this was here, no need to keep 
                        // new lines, we'll see when adding more tests

                        //switch (_theB)
                        //{
                        //    case '{':
                        //    case '[':
                        //    case '(':
                        //    case '+':
                        //    case '-':
                        //    case '!':
                        //    case '~':
                        //        Action(1);
                        //        break;                            
                        //    case ' ':                                     
                        //        Action(3);
                        //        break;
                        //    default:
                        //        Action(isAlphanum(_theB) ? 1 : 2);
                        //        break;
                        //}
                        break;
                    default:
                        switch (_theB)
                        {
                            
                            case ' ':
                                Action(IsAlphanum(_theA) ? 1 : 3);
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
                                        Action(IsAlphanum(_theA) ? 1 : 3);
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

                    //TODO: What is this checking for ?
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
                    
                    //Check for a string literal and process it if it is found
                    if (IsStringLiteral(_theA))
                    {
                        HandleStringLiteral();
                    }
                    else if (IsEndOfStatement(_theA))
                    {
                        HandleEndOfStatement();
                    }
                    goto case 3;
                case 3:
                    _theB = NextCharExcludingComments();

                    //Check for a regex literal and process it if it is found
                    if (IsRegexLiteral(_theA, _theB))
                    {                        
                        HandleRegexLiteral();                        
                    }
                    goto default;
                default:
                    break;
            }
        }

        /// <summary>
        /// Is the char an end of statement character == }
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private static bool IsEndOfStatement(int current)
        {
            return current == '}';
        }

        /// <summary>
        /// write the end and skip all whitespace
        /// </summary>
        private void HandleEndOfStatement()
        {
            //write the } and move next
            Put(_theA);
            do
            {
                _theA = Get();
            } while (char.IsWhiteSpace((char)_theA));
        }

        /// <summary>
        /// Used to determine if the sequence is a string literal
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private static bool IsStringLiteral(int current)
        {
            if (current == '\'' || current == '"' || current == '`')
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Iterates through a string literal
        /// </summary>
        private void HandleStringLiteral()
        {
            //only allowed with template strings
            var allowLineFeed = _theA == '`';

            //write the start quote
            Put(_theA);
            _theA = Get(replaceCr: !allowLineFeed); //don't replace CR here, if we need to deal with that

            for (;;)
            {
                //If the A matches B it means the string literal is done
                // since at this moment B was the original A string literal (" or ')
                if (_theA == _theB)
                {
                    //write the end quote
                    Put(_theA);
                    _theA = Get();
                    break;
                }

                Put(_theA);
                _theA = Get(replaceCr: !allowLineFeed); //don't replace CR here, if we need to deal with that

                switch (_theA)
                {
                    case '\r':
                        if (!allowLineFeed)
                            throw new Exception($"Error: JSMIN unterminated string literal: {_theA}\n");
                        //if we're allowing line feeds, then just continue to write it
                        break;
                    case '\n':
                        if (!allowLineFeed)
                            throw new Exception($"Error: JSMIN unterminated string literal: {_theA}\n");
                        //if we're allowing line feeds, then just continue to write it
                        break;
                    case '\\':
                        //check for escaped chars

                        //This scenario needs to cater for backslash line escapes (i.e. multi-line JS strings)
                        if (Peek() == '\n')
                        {
                            //this is a multi-line string so we don't want to insert a line break here,
                            // just get the next char that is not a line break/eof/or string termination
                            do
                            {
                                _theA = Get();
                            } while (_theA == '\n' && _theA != Eof && _theA != _theB);
                        }
                        else
                        {
                            Put(_theA);
                            _theA = Get();
                        }
                        break;
                    case '$':
                        //check for string templates (i.e. ${ } )
                        if (Peek() == '{')
                        {
                            HandleStringTemplateBlock();
                        }
                        break;
                }
                
                if (_theA == Eof)
                {
                    throw new Exception($"Error: JSMIN unterminated string literal: {_theA}\n");
                }
            }
        }

        /// <summary>
        /// Iterates through a string template block - and caters for nested blocks
        /// </summary>
        private void HandleStringTemplateBlock()
        {
            //This is a string template block
            for (;;)
            {
                Put(_theA);
                _theA = Get();

                switch (_theA)
                {
                    case '}':
                        //write the end bracket and read
                        Put(_theA);
                        _theA = Get();
                        //exit!
                        return;
                    case '$':
                        //check for inner string templates (i.e. ${ } )
                        if (Peek() == '{')
                        {
                            //recurse
                            HandleStringTemplateBlock();
                        }
                        break;
                    case Eof:
                        throw new Exception($"Error: JSMIN unterminated string template block: {_theA}\n");
                }
            }
        }

        /// <summary>
        /// Determines if the sequence is a regex literal
        /// </summary>
        /// <returns></returns>
        private static bool IsRegexLiteral(int current, int next)
        {
            //This is supposed to be testing for regex literals, however it doesn't actually work in many cases,
            // for example see this bug report: https://github.com/douglascrockford/JSMin/issues/11
            // or this: https://github.com/Shazwazza/ClientDependency/issues/73                    
            if (next == '/')
            {
                //This is the original logic from JSMin, but it doesn't cater for the above issue mentioned
                if (current == '(' || current == ',' || current == '=' || current == ':' ||
                    current == '[' || current == '!' || current == '&' || current == '|' ||
                    current == '?' || current == '+' || current == '-' || current == '~' ||
                    current == '*' || current == '/' || current == '{' || current == '\n' ||
                    //We've now added these additional characters and tests pass, the 'n' is specifically relating
                    // to the term 'return', the space is there because a regex literal can always begin after a space
                    current == '+' || current == 'n' || current == ' ')
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Used to iterate over and output the content of a Regex literal
        /// </summary>
        private void HandleRegexLiteral()
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
                            throw new Exception($"Error: JSMIN Unterminated set in Regular Expression literal: {_theA}\n");
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
                            throw new Exception($"Error: JSMIN Unterminated set in Regular Expression literal: {_theA}\n");
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
                    throw new Exception($"Error: JSMIN Unterminated Regular Expression literal: {_theA}\n");
                }
                Put(_theA);
            }
            _theB = NextCharExcludingComments();

        }

        /// <summary>
        /// next -- get the next character, excluding comments. peek() is used to see
        ///  if a '/' is followed by a '/' or '*'.
        /// </summary>
        /// <returns></returns>
        private int NextCharExcludingComments()
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
                            if (IsLineSeparator(c))
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
        private int Peek()
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
        private int Get(bool replaceCr = true)
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
            if (c == '\r' && !replaceCr)
            {
                return c;
            }
            if (c == '\r' && replaceCr)
            {
                return '\n';
            }
            if (c == '\u2028' || c == '\u2029')
            {
                return '\n';
            }
            return ' ';
        }

        private void Put(int c)
        {
            _sw.Write((char)c);
        }

        /// <summary>
        /// isAlphanum -- return true if the character is a letter, digit, underscore,
        /// dollar sign, or non-ASCII character.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsAlphanum(int c)
        {
            return ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') ||
                    (c >= 'A' && c <= 'Z') || c == '_' || c == '$' || c == '\\' ||
                    c > 126);
        }

        private bool IsLineSeparator(int c)
        {
            return c <= '\n' || c == '\u2028' || c == '\u2029';
        }

    }
}