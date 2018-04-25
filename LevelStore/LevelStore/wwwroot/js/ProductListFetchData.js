var lastTimeOrder = '0';
$(function () {
    
    $(window).scroll(function () {
        if (($(window).scrollTop() >=
                ($(document).height() - $(window).height()) - 150) &&
            lastTimeOrder !== $("#LastItem").text()) {

            FetchDataFromServer();
        }
    });
});

function AllowDownloadData()
{
    lastTimeOrder = '0';
}

function FetchDataFromServer() {

    lastTimeOrder = $("#LastItem").text();
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
        if (shares[i].shareId === shareID) {
            return i;
        }
    }
    return -1;
}

function searchImageFirst(images) {
    for (var i = 0; i < images.length; i++) {
        if (images[i].firstOnScreen === true) {
            return i;
        }
    }
    return -1;
}

function searchImageSecond(images) {
    for (var i = 0; i < images.length; i++) {
        if (images[i].secondOnScreen === true) {
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
        var startString = "<li class=\"Product-wrapper\">";

        var price = product.product.price;
        var newPrice = price;

        startString += "<a class=\"Product\" href=\"/Product/ViewSingleProduct?productId=" + product.product.productID + "\">";
        if (product.product.shareID != null) {
            
            var share = searchShare(data.shares, product.product.shareID);


            if (share >= 0) {
                share = data.shares[share];
                if (new Date(share.dateOfStart) <= $.now() && share.enabled) {
                    startString += "<div class=\"ShareDisplay\">";
                    startString += "<div>Акция! До конца:</div>";
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
                    startString += "</div>";
                    if (share.fake) {
                        price = price / 100 * (share.koefPrice + 100);
                    } else {
                        newPrice = price / 100 * (100 - share.koefPrice);
                    }
                }
            }
        }

        if (product.product.newProduct === true) {
            startString += "<img class=\"NewIcon\" src=\"/images/img/main-page/new.svg\" alt=\"NEW\">";
        }
        

        if (product.images.length > 0 && searchImageFirst(product.images) >= 0 && searchImageSecond(product.images) >= 0) {
            var firstImageName = product.images[searchImageFirst(product.images)].name;
            var firstImageAlt = product.images[searchImageFirst(product.images)].alternative;
            var secondImageName = product.images[searchImageSecond(product.images)].name;
            var secondImageAlternative = product.images[searchImageSecond(product.images)].alternative;
            startString += "<img src=\"/images/" +
                firstImageName +
                "\" id=\"" +
                product.product.productID +
                "\" name=\"FirstAndSecondImages\" onmouseover=\"mouseOver(this)\" onmouseout=\"mouseOut(this)\" alt=\"" + firstImageAlt + "\">";
            startString += "<input type=\"hidden\" name=\"FirstAndSecondImages\" imageName=\"/images/" + firstImageName +  "\"imageAlternative=\"" + firstImageAlt + "\"/>";
            startString += "<input type=\"hidden\" name=\"FirstAndSecondImages\" imageName=\"/images/" + secondImageName + "\"imageAlternative=\"" + secondImageAlternative + "\"/>";
        }
        else
        {
            startString += "<img src=\"/images/None.jpg\" alt=\"None\">";
        }

        startString += "<div>" + product.product.name + " " + "</div>";
        if (newPrice != price) {
            startString += "<div style=\"display:inline-block; font-weight:bold; color: red;\"><del style=\"font-weight: normal; color: black;\">" + price.toFixed(2).replace(".", ",") + "</del>" + newPrice.toFixed(2).replace(".", ",") + "</div>";
            startString += "<div style=\"display:inline-block;\">грн.</div>";
            startString += "<br />";
        }
        else {
            startString += "<div>" + price.toFixed(2).replace(".", ",") + "грн. </div>";
        }
        
        startString += "<div type=\"button\" class=\"btn btn-success\">Посмотреть</div>";
        startString += "</li>";
        ololo = startString;
        $(holder).append(startString);
    }
    GetBlockImage();
    UpdateShareCounter();
    AllowDownloadData();
}


