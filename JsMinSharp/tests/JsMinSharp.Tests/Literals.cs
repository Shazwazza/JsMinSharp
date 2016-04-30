using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace JsMinSharp.Tests
{
    public class Literals
    {
        private readonly TestHelper _helper;

        public Literals(ITestOutputHelper output)
        {
            _helper = new TestHelper(output);
        }        

        [Fact]
        public void ArrayLiteral()
        {
            var fileName = "ArrayLiteral.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            _helper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void Strings()
        {
            var fileName = "Strings.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            _helper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void GetterSetter()
        {
            var fileName = "GetterSetter.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            _helper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void RegExp()
        {
            var fileName = "RegExp.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            _helper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void ReturnRegExp()
        {
            var fileName = "ReturnRegExp.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            _helper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void TemplateLiterals()
        {
            var fileName = "TemplateLiterals.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            _helper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void ObjectLiteral()
        {
            var fileName = "ObjectLiteral.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            _helper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void Member()
        {
            var fileName = "Member.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            _helper.AssertFileMatch(input, expected);
        }


    }
}