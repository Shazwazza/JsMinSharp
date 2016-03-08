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

    public class Comments
    {
        private readonly TestHelper _helper;

        public Comments(ITestOutputHelper output)
        {
            _helper = new TestHelper(output);
        }

        [Fact]
        public void Comment()
        {
            var fileName = "Comment.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Comments", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Comments", fileName));
            _helper.AssertFileMatch(input, expected);
        }

        [Fact]
        public void UnterminatedComment()
        {
            var fileName = "UnterminatedComment.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Comments", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Comments", fileName));
            _helper.AssertException(input, expected);
        }

    }

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