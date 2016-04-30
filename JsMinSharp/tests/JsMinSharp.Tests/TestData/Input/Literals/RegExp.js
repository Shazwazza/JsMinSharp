function test1(text)
{
    // this regular expression do NOT have the global flag (g) on it,
    // so we can treat it like a constant and get rid of the variable
    // middle-man.
    var re = /the/;
    return re.exec(text);
}

function test2(text)
{
    // this regular expression DOES have the global flag (g) on it,
    // so we CANNOT treat it like a constant and get rid of the variable
    // middle-man. If we were to replace the variable in the while condition
    // with the literal, it would cause an infinite loop because the exec()
    // method would restart from the beginning each and every time.
    var re = /the/g;
    var count = 0;
    var match;
    while((match = re.exec(text)))
    {
        ++count;
    }
    return count;
}

var foo = location.href.replace(/[\s?!@#$%^&*()_=+,.<>'":;\[\]/|]/g, '-');


var re = /\w+/;
alert("hi");

//this is still valid js even though it's pointless
/android.*safari.*/i.test(i);
//this is still valid js even though the regex is pointless
var i = 0;/android.*safari.*/i.test(i);

(function (global) {
    if (typeof global === "object") {
        void (0);
    }
    // Do not consider this regex
}(typeof window !== "undefined"));