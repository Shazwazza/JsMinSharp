using System.IO;

namespace JsMinSharp
{
    public interface IMinifier
    {
        string Minify(TextReader reader);
    }
}