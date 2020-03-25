using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace JsMinSharp.Tests
{
    public class Libs
    {
        private readonly TestHelper _helper;

        public Libs(ITestOutputHelper output)
        {
            _helper = new TestHelper(output);
        }

        [Fact]
        public void NestedContentController()
        {
            var fileName = "nested-content-controller.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Libs", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Libs", fileName));
            _helper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void GodModeDataTypeBrowserController()
        {
            var fileName = "GodMode.DataTypeBrowser.Controller.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Libs", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Libs", fileName));
            _helper.AssertFileMatch(input, expected);
        }

    }
}