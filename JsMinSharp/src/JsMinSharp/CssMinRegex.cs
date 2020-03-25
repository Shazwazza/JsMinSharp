//using System.IO;
//using System.Text.RegularExpressions;

//namespace JsMinSharp
//{
//    public class CssMinRegex : IMinifier
//    {
//        public string Minify(TextReader reader)
//        {
//            return MinifyCss(reader.ReadToEnd());
//        }

//        public static string MinifyCss(string body)//        {//            body = Regex.Replace(body, @"[\n\r]+\s*", string.Empty);//            body = Regex.Replace(body, @"\s+", " ");//            body = Regex.Replace(body, @"\s?([:,;{}])\s?", "$1");//            body = Regex.Replace(body, @"([\s:]0)(px|pt|%|em)", "$1");//            body = Regex.Replace(body, @"/\*[\d\D]*?\*/", string.Empty);//            return body;
//        }

//    }
//}