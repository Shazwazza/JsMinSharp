using System.IO;
using Xunit;

namespace JsMinSharp.Tests
{
    public class Literals
    {
        [Fact]
        public void ArrayLiteral()
        {
            var fileName = "ArrayLiteral.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            TestHelper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void Strings()
        {
            var fileName = "Strings.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            TestHelper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void GetterSetter()
        {
            var fileName = "GetterSetter.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            TestHelper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void RegExp()
        {
            var fileName = "RegExp.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            TestHelper.AssertFileMatch(input, expected);
        }

    }
}