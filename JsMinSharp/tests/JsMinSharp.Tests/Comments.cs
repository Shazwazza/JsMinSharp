using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace JsMinSharp.Tests
{
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

        [Fact]
        public void ImportantComment()
        {
            var fileName = "ImportantComment.js";
            var input = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Input", "Comments", fileName));
            var expected = new FileInfo(Path.Combine(TestHelper.TestDataFolder, "Expected", "Comments", fileName));
            _helper.AssertFileMatch(input, expected);
        }

    }
}