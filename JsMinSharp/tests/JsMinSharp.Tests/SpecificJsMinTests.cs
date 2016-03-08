using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace JsMinSharp.Tests
{
    /// <summary>
    /// Tests ported from CDF project to test specific fixes
    /// </summary>
    public class SpecificJsMinTests
    {
        private readonly ITestOutputHelper _output;
        private readonly TestHelper _helper;

        public SpecificJsMinTests(ITestOutputHelper output)
        {
            _output = output;
            _helper = new TestHelper(output);
        }

        [Fact]
        public void JsMinify_Escaped_Quotes_In_String_Literal()
        {
            var script = "var asdf=\"Some string\\\'s with \\\"quotes\\\" in them\"";

            var minifier = new JsMin();

            //Act            

            var output = _helper.DoMinify(minifier, script);

            Assert.Equal("var asdf=\"Some string\\\'s with \\\"quotes\\\" in them\"", output);
        }      

        [Fact]
        public void JsMinify_Minify()
        {
            //Arrange
            var script =
                @"var Messaging = {
    GetMessage: function(callback) {
        $.ajax({
            type: ""POST"",
            url: ""/Services/MessageService.asmx/HelloWorld"",
            data: ""{}"",
            contentType: ""application/json; charset=utf-8"",
            dataType: ""json"",
            success: function(msg) {
                callback.apply(this, [msg.d]);
            }
        });
    }
    var blah = 1;
    blah++;
    blah = blah + 2;
    var newBlah = ++blah;
    newBlah += 234 +4;
};";
            var minifier = new JsMin();
            //Act
            var output = _helper.DoMinify(minifier, script);
            Assert.Equal(
                "var Messaging={GetMessage:function(callback){$.ajax({type:\"POST\",url:\"/Services/MessageService.asmx/HelloWorld\",data:\"{}\",contentType:\"application/json; charset=utf-8\",dataType:\"json\",success:function(msg){callback.apply(this,[msg.d]);}});}var blah=1;blah++;blah=blah+2;var newBlah=++blah;newBlah+=234+4;};",
                output);
        }

        [Fact]
        public void JsMinify_Minify_With_Unary_Operator()
        {
            //see: http://clientdependency.codeplex.com/workitem/13162

            //Arrange

            var script =
                @"var c = {};
var c.name = 0;
var i = 1;
c.name=i+ +new Date;
alert(c.name);";

            var minifier = new JsMin();

            //Act

            var output = _helper.DoMinify(minifier, script);

            //Assert

            Assert.Equal("var c={};var c.name=0;var i=1;c.name=i+ +new Date;alert(c.name);", output);
        }

        [Fact]
        public void JsMinify_Backslash_Line_Escapes()
        {
            var script = @"function Test() {
jQuery(this).append('<div>\
  <div>\
    <a href=""http://google.com"" /></a>\
  </div>\
</div>');
}";

            var minifier = new JsMin();

            //Act

            var output = _helper.DoMinify(minifier, script);

            //Assert

            Assert.Equal("function Test(){jQuery(this).append('<div>  <div>    <a href=\"http://google.com\" /></a>  </div></div>');}", output);

        }

        [Fact]
        public void JsMinify_TypeScript_Enum()
        {
            var script = @"$(""#TenderListType"").val(1 /* Calendar */.toString());";

            var minifier = new JsMin();

            var output = _helper.DoMinify(minifier, script);

            Assert.Equal("$(\"#TenderListType\").val(1..toString());", output);
        }

        [Fact]
        public void JsMinify_Function()
        {
            var script = @"function(el,args)
{
    if ( !args ) { args = {}; }
}
";
            var minifier = new JsMin();

            var output = _helper.DoMinify(minifier, script);

            Assert.Equal("function(el,args){if(!args){args={};}}", output);
        }
    }
}
