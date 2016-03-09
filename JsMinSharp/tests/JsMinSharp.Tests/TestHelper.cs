using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace JsMinSharp.Tests
{
    public class TestHelper
    {
        private readonly ITestOutputHelper _output;

        public TestHelper(ITestOutputHelper output)
        {
            _output = output;
        }

        public void AssertException(FileInfo inputFile, FileInfo expectedFile)
        {
            var jsmin = new JsMin();
            Assert.Throws<Exception>(() => DoMinify(jsmin, inputFile));
        }

        public void AssertFileMatch(FileInfo inputFile, FileInfo expectedFile)
        {
            var jsmin = new JsMin();
            var input = DoMinify(jsmin, inputFile);
            var expected = File.ReadAllText(expectedFile.FullName, Encoding.UTF8);

            _output.WriteLine("Expected: " + expected);

            Assert.Equal(expected, input);
        }

        public string DoMinify(JsMin minifier, string input)
        {
            using (var reader = new StringReader(input))
            {
                var result = minifier.Minify(reader);
                _output.WriteLine("Minified: " + result);
                return result;
            }
        }

        public string DoMinify(JsMin minifier, FileInfo input)
        {
            using (var reader = File.OpenText(input.FullName))
            {
                var result = minifier.Minify(reader);
                _output.WriteLine("Minified: " + result);
                return result;
            }
        }

        public static string TestDataFolder => 
            Path.Combine(Environment.CurrentDirectory, "TestData");
    }
}