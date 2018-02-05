//function unicodeToChar(text) {
//    return text.replace(/\\u[\dA-F]{4}/gi,
//        function (match) {
//            return String.fromCharCode(parseInt(match.replace(/\\u/g, ''), 16));
//        });
//}


//String.prototype.hexDecode = function () {
//    var j;
//    var hexes = this.match(/.{1,4}/g) || [];
//    var back = "";
//    for (j = 0; j < hexes.length; j++) {
//        back += String.fromCharCode(parseInt(hexes[j], 16));
//    }

//    return back;
//}


function convertHexToString(input) {

    // split input into groups of two
    var hex = input.match(/[\s\S]{2}/g) || [];
    var output = '';

    // build a hex-encoded representation of your string
    for (var i = 0, j = hex.length; i < j; i++) {
        output += '%' + ('0' + hex[i]).slice(-2);
    }

    // decode it using this trick
    output = decodeURIComponent(output);

    return output;
}