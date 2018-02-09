function HideAndShowImages(select) {
    var i;
    var images = $("[color]");
    if ($(select).val() == 0) {
        for (i = 0; i < images.length; i++) {
            $($(images)[i]).show();
        }
    }
    else {
        for (i = 0; i < images.length; i++) {
            if ($($(images)[i]).attr('color') == $(select).val()) {
                $($(images)[i]).show();
            } else {
                $($(images)[i]).hide();
            }
        }
    }
}

