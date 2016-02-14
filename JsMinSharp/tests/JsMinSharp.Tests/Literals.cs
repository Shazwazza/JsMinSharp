using System.IO;
using Xunit;

namespace JsMinSharp.Tests
{
    public class Literals
    {
        [Fact]
        public void ArrayLiteral()
        {
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", "ArrayLiteral.js"));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", "ArrayLiteral.js"));
            TestHelper.AssertFileMatch(input, expected);
        }
    }
}