using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace JsMinSharp.Tests
{
    public class LineBreaks
    {
        private readonly TestHelper _helper;

        public LineBreaks(ITestOutputHelper output)
        {
            _helper = new TestHelper(output);
        }

        [Fact]
        public void PreserveBreak()
        {
            var fileName = "PreserveBreak.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "LineBreaks", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "LineBreaks", fileName));
            _helper.AssertFileMatch(input, expected);
        }

    }
}