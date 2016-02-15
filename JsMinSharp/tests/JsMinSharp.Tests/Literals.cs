using System.IO;
using Xunit;

namespace JsMinSharp.Tests
{
    public class Comments
    {
        [Fact]
        public void Comment()
        {
            var fileName = "Comment.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Comments", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Comments", fileName));
            TestHelper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void UnterminatedComment()
        {
            var fileName = "UnterminatedComment.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Comments", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Comments", fileName));
            TestHelper.AssertException(input, expected);
        }

    }

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

        [Fact]
        public void TemplateLiterals()
        {
            var fileName = "TemplateLiterals.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            TestHelper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void ObjectLiteral()
        {
            var fileName = "ObjectLiteral.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            TestHelper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void Member()
        {
            var fileName = "Member.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Literals", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Literals", fileName));
            TestHelper.AssertFileMatch(input, expected);
        }


    }
}