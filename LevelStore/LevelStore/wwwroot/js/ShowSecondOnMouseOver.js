
var BlockImage;


function mouseOver(element) {
    for (var i = 0; i < BlockImage.length; i += 3) {
        if ($(BlockImage[i]).attr('id') === $(element).attr('id')) {
            var newSrc = $(BlockImage[i + 2]).attr("imageName");
            var newAlternative = $(BlockImage[i + 2]).attr("imageAlternative");
            $(element).prop("src", newSrc);
            $(element).prop("alt", newAlternative);
        }
    }
}

function mouseOut(element) {
    for (var i = 0; i < BlockImage.length; i += 3) {
        if ($(BlockImage[i]).attr('id') === $(element).attr('id')) {
            var newSrc = $(BlockImage[i + 1]).attr("imageName");
            var newAlternative = $(BlockImage[i + 1]).attr("imageAlternative");
            $(element).prop("src", newSrc);
            $(element).prop("alt", newAlternative);
        }
    }
}

$(document).ready(function() {
    GetBlockImage();
});

function GetBlockImage() {
    BlockImage = $("[name='FirstAndSecondImages']");
}

