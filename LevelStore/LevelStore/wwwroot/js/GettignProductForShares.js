

function RunAjaxSearch(e, mustClick) {
    if (e.keyCode === 13 || mustClick === true) {
        $("#AjaxSearchProductForShares").click();
    }
}

function search() {
    var searchString = $("#search-input").val();
    var subCategoryId = $("#category-input").val();
    var categoryId = $("#category-input").find('option:selected').attr("CategoryId");
    var searchData = {
        "searchString": searchString,
        "categoryId": categoryId,
        "subCategoryId": subCategoryId
    }

    $.ajax({
        url: "/Shares/SharesAjaxSearch/",
        type: "POST",
        dataType: "text json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(searchData),
        success: function (products) {
            ShowProductList(products);
        }
    });
};

function ShowProductList(products) {
    var productBlock = $("#SelectedProductBlock");
    productBlock.empty();
    if (products.length > 0) {
        for (var i = 0; i < products.length; i++) {
            productBlock.append(
                "<div>" +
                "<input checked=\"checked\" productAttr=\"yes\" type=\"checkbox\" value=\"" + products[i].productID + "\" name=\"Products[" + i + "]\" /> " +
                products[i].name +
                " <button type=\"button\" onclick=\"RecalcIndexOfProducts(this)\">Удалить</button>" +
                "</div>"
            );
        }
    }
}


function RecalcIndexOfProducts(element) {
    DeleteElem((element).parentElement);
    var products = $("[productAttr]");
    for (var i = 0; i < products.length; i++) {
        $(products[i]).attr('name', "Products[" + i + "]");
    }

}