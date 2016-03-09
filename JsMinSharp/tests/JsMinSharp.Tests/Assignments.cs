using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace JsMinSharp.Tests
{
    public class Assignments
    {
        private readonly TestHelper _helper;

        public Assignments(ITestOutputHelper output)
        {
            _helper = new TestHelper(output);
        }

        [Fact]
        public void Assign()
        {
            var fileName = "Assign.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Assignments", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Assignments", fileName));
            _helper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void CompoundAssign()
        {
            var fileName = "CompoundAssign.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Assignments", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Assignments", fileName));
            _helper.AssertFileMatch(input, expected);
        }

    }
}