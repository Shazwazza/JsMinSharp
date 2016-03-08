//test return statement right before regex literal
b.prototype._normalizeURL=function(a){return/^https?:\/\//.test(a)||(a="http://"+a),a};

//test return statement after line break right before regex literal
b.prototype._normalizeURL=function(a){
    return/^https?:\/\//.test(a)||(a="http://"+a),a};

//Test + before regex literal
var ex = +/w$/.test(resizing),
    ey = +/^n/.test(resizing);

//Test returning regex literal
function test() {
    var text = "asdfasdf";
	
    return /[",\n]/.test(text)
          ? "\"" + text.replace(/\" /g, "\"\"") + "\""
          : text;     
}
