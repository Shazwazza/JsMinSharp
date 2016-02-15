using System;
using System.IO;
using System.Text;
using Xunit;

namespace JsMinSharp.Tests
{
    public class TestHelper
    {
        public static void AssertException(FileInfo inputFile, FileInfo expectedFile)
        {
            var jsmin = new JsMin();
            Assert.Throws<Exception>(() => DoMinify(jsmin, inputFile));
        }

        public static void AssertFileMatch(FileInfo inputFile, FileInfo expectedFile)
        {
            var jsmin = new JsMin();
            var input = DoMinify(jsmin, inputFile);
            var expected = File.ReadAllText(expectedFile.FullName, Encoding.UTF8);

            Assert.Equal(expected, input);
        }

        public static string DoMinify(JsMin minifier, string input)
        {
            using (var reader = new StringReader(input))
            {
                return minifier.Minify(reader);
            }
        }

        public static string DoMinify(JsMin minifier, FileInfo input)
        {
            using (var reader = File.OpenText(input.FullName))
            {
                return minifier.Minify(reader);
            }
        }

        public static string TestDataFolder => 
            Path.Combine(Environment.CurrentDirectory, "TestData");
    }
}