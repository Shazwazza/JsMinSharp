using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace JsMinSharp.Tests
{
    /// <summary>
    /// Tests ported from CDF project to test specific fixes
    /// </summary>
    public class SpecificJsMinTests
    {
        private string DoMinify(JsMin minifier, string input)
        {
            using (var reader = new StringReader(input))
            {
                return minifier.Minify(reader);
            }
        }

        [Fact]
        public void JsMinify_Escaped_Quotes_In_String_Literal()
        {
            var script = "var asdf=\"Some string\\\'s with \\\"quotes\\\" in them\"";

            var minifier = new JsMin();

            //Act            

            var output = DoMinify(minifier, script);

            Assert.Equal("\nvar asdf=\"Some string\\\'s with \\\"quotes\\\" in them\"", output);
        }

        [Fact]
        public void JsMinify_Handles_Regex()
        {
            var script1 = @"b.prototype._normalizeURL=function(a){return/^https?:\/\//.test(a)||(a=""http://""+a),a}";
            var script2 = @"var ex = +  /w$/.test(resizing),
    ey = +/^ n /.test(resizing); ";
            var script3 = @"return /["",\n]/.test(text)
      ? ""\"""" + text.replace(/\"" / g, ""\""\"""") + ""\""""
      : text; ";
            var minifier = new JsMin();
            //Act

            var output1 = DoMinify(minifier, script1);
            Assert.Equal("\n" + script1, output1);
            var output2 = DoMinify(minifier, script2);
            Assert.Equal("\n" + @"var ex=+/w$/.test(resizing),ey=+/^ n /.test(resizing);", output2);
            var output3 = DoMinify(minifier, script3);
            Assert.Equal("\n" + @"return /["",\n]/.test(text)?""\""""+text.replace(/\"" /g,""\""\"""")+""\"""":text;", output3);
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
            var output = DoMinify(minifier, script);
            Assert.Equal(
                "\nvar Messaging={GetMessage:function(callback){$.ajax({type:\"POST\",url:\"/Services/MessageService.asmx/HelloWorld\",data:\"{}\",contentType:\"application/json; charset=utf-8\",dataType:\"json\",success:function(msg){callback.apply(this,[msg.d]);}});}\nvar blah=1;blah++;blah=blah+2;var newBlah=++blah;newBlah+=234+4;};",
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

            var output = DoMinify(minifier, script);

            //Assert

            Assert.Equal("\nvar c={};var c.name=0;var i=1;c.name=i+ +new Date;alert(c.name);", output);
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

            var output = DoMinify(minifier, script);

            //Assert

            Assert.Equal("function Test(){jQuery(this).append('<div>\\  <div>\\   <a href=\"http://google.com\" /></a>\\  </div>\\ </div>');}",
                output.Replace("\n", string.Empty));

        }

        [Fact]
        public void JsMinify_TypeScript_Enum()
        {
            var script = @"$(""#TenderListType"").val(1 /* Calendar */.toString());";

            var minifier = new JsMin();

            var output = DoMinify(minifier, script);

            Assert.Equal("\n$(\"#TenderListType\").val(1..toString());", output);
        }

    }
}
