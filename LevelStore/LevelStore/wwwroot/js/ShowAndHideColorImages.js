



//$('select[name=selectedColor] :nth-child(4)').prop('selected', true);


function HideAndShowImages() {
    var select = 0;
    var answers = $(".sel__box__options");
    var i;
    for (i = 0; i < answers.length; i++)
    {
        if ($(answers[i]).hasClass("selected")) {
            select = i;
            break;
        } 
    }
    var images = $("[color]");
    if (select === 0) {
        for (i = 0; i < images.length; i++) {
            $($(images)[i]).show();
        }
    }
    else {
        var wasColor = false;
        for (i = 0; i < images.length; i++) {
            if ($($(images)[i]).attr('color') == select) {
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

