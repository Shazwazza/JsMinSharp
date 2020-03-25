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
        private readonly Func<IMinifier> _minifier;


        public TestHelper(ITestOutputHelper output, Func<IMinifier> minifier = null)
        {
            _output = output;
            _minifier = minifier ?? (() => new JsMin()) ;
        }

        public void AssertException(FileInfo inputFile, FileInfo expectedFile)
        {
            var jsmin = _minifier();
            Assert.Throws<Exception>(() => DoMinify(jsmin, inputFile));
        }

        public void AssertFileMatch(FileInfo inputFile, FileInfo expectedFile, bool replaceLineBreaks = false)
        {
            var jsmin = _minifier();
            var input = DoMinify(jsmin, inputFile);            
            var expected = File.ReadAllText(expectedFile.FullName, Encoding.UTF8);
            if (replaceLineBreaks)
            {
                expected = expected.Replace("\r\n", "\n");
            }

            _output.WriteLine("Expected: " + expected);

            Assert.Equal(expected, input);
        }

        public string DoMinify(IMinifier minifier, string input)
        {
            using (var reader = new StringReader(input))
            {
                var result = minifier.Minify(reader);
                _output.WriteLine("Minified: " + result);
                return result;
            }
        }

        public string DoMinify(IMinifier minifier, FileInfo input)
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