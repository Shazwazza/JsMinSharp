using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace JsMinSharp.Tests
{
    public class CssMinTests
    {
        private readonly TestHelper _helper;

        public CssMinTests(ITestOutputHelper output)
        {
            _helper = new TestHelper(output, () => new CssMin());
        }

        [Fact]
        public void Test1()
        {
            var fileName = "test1.css";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Css", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Css", fileName));
            _helper.AssertFileMatch(input, expected, replaceLineBreaks:true);
        }

        //[Fact]
        //public void Test1a()
        //{
        //    var fileName = "test1.css";
        //    var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Css", fileName));
        //    var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Css", fileName));
        //    _helper.AssertFileMatch(input, expected, replaceLineBreaks: true);
        //}

        [Fact]
        public void Test2()
        {
            var fileName = "bootstrap.min.css";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Css", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Css", fileName));
            _helper.AssertFileMatch(input, expected, replaceLineBreaks: true);
        }

    }
}