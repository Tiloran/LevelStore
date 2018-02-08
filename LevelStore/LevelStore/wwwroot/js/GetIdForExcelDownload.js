function CollectId() {
    var ids = $("[name='checkedOId']");
    $("#excelIdContainer").empty();
    for (var i = 0; i < ids.length; i++) {
        if ($(ids[i]).is(":checked")) {
            $("#excelIdContainer").append("<input name=\"checkedOrderId\" value=\"" + $(ids[i]).val() + "\"/>");
        }
    }
    $("#ExcelSubmitButton").click();
}