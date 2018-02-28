function HideAndShowImages(select) {
    var i;
    var images = $("[color]");
    if ($(select).val() === 0) {
        for (i = 0; i < images.length; i++) {
            $($(images)[i]).show();
        }
    }
    else {
        var wasColor = false;
        for (i = 0; i < images.length; i++) {
            if ($($(images)[i]).attr('color') === $(select).val()) {
                $($(images)[i]).show();
                wasColor = true;
            } else {
                $($(images)[i]).hide();
            }
        }
        if (wasColor === false) {
            for (i = 0; i < images.length; i++) {
                $($(images)[i]).show();
            }
        }
    }
}

