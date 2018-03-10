$(function () {
    
    $(window).scroll(function () {
        if ($(window).scrollTop() ==
            $(document).height() - $(window).height()) {
                FetchDataFromServer();
        }
    });
});

function FetchDataFromServer() {
    
    var Data = {
        "SearchString": $("#SearchField").val(),
        "CategoryId": $("#CategoryId").text(),
        "SubCategoryId": $("#SubCategoryId").text(),
        "FirstInOrder": $("#LastItem").text()
    }
    $.ajax("/Product/ListAjax", {
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(Data),
        dataType: "json",
        success: function (data) {
            ShowNewData(data);
        },
        error: function () {
            
        }
    });
}

function searchShare(shares, shareID) {
    for (var i = 0; i < shares.length; i++) {
        if (shares[i].shareId == shareID) {
            return i;
        }
    }
    return -1;
}

function searchImageFirst(images) {
    for (var i = 0; i < images.length; i++) {
        if (images[i].firstOnScreen == true) {
            return i;
        }
    }
    return -1;
}

function searchImageSecond(images) {
    for (var i = 0; i < images.length; i++) {
        if (images[i].secondOnScreen == true) {
            return i;
        }
    }
    return -1;
}

var ololo;

function ShowNewData(data) {
    ololo = data;
    var lastItem = $("#LastItem").text();
    $("#LastItem").text(parseInt(lastItem) + data.productAndImages.length);
    var holder = $("#itemHolder");
    for (var i = 0; i < data.productAndImages.length; i++) {

        var product = data.productAndImages[i];
        var startString = "<div>";

        var price = product.product.price;
        var newPrice = price;


        if (product.product.newProduct === true) {
            startString += "<div style=\"background-color: red; width: 100px;\">NEW</div>";
        }
        if (product.product.shareID != null) {
            
            var share = searchShare(data.shares, product.product.shareID);


            if (share >= 0) {
                share = data.shares[share];
                if (new Date(share.dateOfStart) <= $.now() && share.enabled) {
                    startString += "<div> Акция </div>";
                    var timeOfEnd = new Date(share.dateOfEnd);
                    var seconds = '' + timeOfEnd.getSeconds();
                    var minutes = '' + timeOfEnd.getMinutes();
                    var hours = '' + timeOfEnd.getHours();
                    var month = '' + (timeOfEnd.getMonth() + 1);
                    var day = '' + timeOfEnd.getDate();
                    var year = timeOfEnd.getFullYear();

                    if (seconds.length < 2) seconds = '0' + seconds;
                    if (minutes.length < 2) minutes = '0' + minutes;
                    if (hours.length < 2) hours = '0' + hours;
                    if (month.length < 2) month = '0' + month;
                    if (day.length < 2) day = '0' + day;

                    var timeOfEndString = year + "/" + month + "/" + day + " " + hours + ":" + minutes + ":" + seconds;
                    startString += "<div data-countdown=\"" + timeOfEndString + "\"></div>";
                    if (share.fake) {
                        price = price / 100 * (share.koefPrice + 100);
                    } else {
                        newPrice = price / 100 * (100 - share.koefPrice);
                    }
                }
            }
        }
        startString += "<span>" + product.product.name + " " + "</span>";
        if (newPrice != price) {
            startString += "<span><del>" + price.toFixed(2).replace(".", ",") + "</del> </span>";
            startString += "<span>" + newPrice.toFixed(2).replace(".", ",") + " </span>";
        }
        else {
            startString += "<span>" + price.toFixed(2).replace(".", ",") + " </span>";
        }

        startString += "<div>" + product.product.description + "</div>";

        if (product.images.length > 0 && searchImageFirst(product.images) >= 0 && searchImageSecond(product.images) >= 0) {
            var firstImageName = product.images[searchImageFirst(product.images)].name;
            var firstImageAlt = product.images[searchImageFirst(product.images)].alternative;
            var secondImageName = product.images[searchImageSecond(product.images)].name;
            var secondImageAlternative = product.images[searchImageSecond(product.images)].alternative;
            startString += "<img src=\"/images/" +
                firstImageName +
                "\" id=\"" +
                product.product.productID +
                "\" name=\"FirstAndSecondImages\" onmouseover=\"mouseOver(this)\" onmouseout=\"mouseOut(this)\" style=\"width: 100px; height: 100px;\" alt=\"" +
                firstImageAlt +
                "\">";
            startString += "<input type=\"hidden\" name=\"FirstAndSecondImages\" imageName=\"/images/" + firstImageName +  "\"imageAlternative=\"" + firstImageAlt + "\"/>";
            startString += "<input type=\"hidden\" name=\"FirstAndSecondImages\" imageName=\"/images/" + secondImageName + "\"imageAlternative=\"" + secondImageAlternative + "\"/>";
        }
        else
        {
            startString += "<img src=\"/images/None.jpg\" style=\"width: 100px; height: 100px;\" alt=\"\">";
        }

        startString += "<a href=\"/Product/ViewSingleProduct?productId=" +
            product.product.productID +
            "\" class=\"btn btn-primary\">Выбрать</a>";
        startString += "</div>";
        ololo = startString;
        $(holder).append(startString);
    }
    GetBlockImage();
    UpdateShareCounter();
}


