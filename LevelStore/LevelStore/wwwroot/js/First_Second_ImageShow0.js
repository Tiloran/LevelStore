
var CounterForIdFirst = 0;
var CounterForIdSecond = 0;
var CounterForIdThird = 0;
var first;
var second;
var third;
var DontChange = false;

function First_Second_ImageShow(element) {
    $(document).ready(function () {

        var FirstValueForAspNet = $("#FirstOnScreen");
        var SecondValueForAspNet = $("#SecondOnScreen");

        first = $("[name='FirstOnScreen']");
        second = $("[name='SecondOnScreen']");
        third = $("[name='NoneChecked']");
        var index;
        var firstElem = false;
        var secondElem = false;
        var commonElem = false;
        if ($(element).attr('id').indexOf("First") >= 0) {
            index = GetIndexOfSelectedElement(first, element);
            if (index != -1) {
                firstElem = true;
            }
        }
        else if ($(element).attr('id').indexOf("Second") >= 0) {
            index = GetIndexOfSelectedElement(second, element);
            if (index != -1) {
                secondElem = true;
            }
        }
        else if ($(element).attr('id').indexOf("Third") >= 0) {
            index = GetIndexOfSelectedElement(third, element);
            if (index != -1) {
                commonElem = true;
            }
        }

        if (firstElem) {
            UnSetChecked(second[index]);
            UnSetChecked(third[index]);
            SetChecked(first[index]);
            for (var i = 0; i < first.length; i++) {
                if (index != i) {
                    UnSetChecked(first[i]);
                    if ($(second[i]).attr("checked") != 'checked') {
                        SetChecked(third[i]);
                    }
                }
            }
        }
        else if (secondElem) {
            UnSetChecked(first[index]);
            UnSetChecked(third[index]);
            SetChecked(second[index]);
            for (var i = 0; i < second.length; i++) {
                if (index != i) {
                    UnSetChecked(second[i]);
                    if ($(first[i]).attr("checked") != 'checked') {
                        SetChecked(third[i]);
                    }
                }
            }
        }
        else if (commonElem) {
            SetChecked(third[index]);
            UnSetChecked(first[index]);
            UnSetChecked(second[index]);
            if (third.length > 1) {
                var firstWas = false;
                var secondWas = false;
                for (var i = 0; i < third.length; i++) {
                    if ($(second[i]).attr("checked") == 'checked') {
                        secondWas = true;
                    }
                    if ($(first[i]).attr("checked") == 'checked') {
                        firstWas = true;
                    }
                }

                if (firstWas != true) {
                    if ($(second[0]).attr("checked") != 'checked') {
                        SetChecked(first[0]);
                        UnSetChecked(second[0]);
                        UnSetChecked(third[0]);
                    } else {
                        SetChecked(first[1]);
                        UnSetChecked(second[1]);
                        UnSetChecked(third[1]);
                    }
                }

                if (secondWas != true) {
                    if ($(first[0]).attr("checked") != 'checked') {
                        SetChecked(second[0]);
                        UnSetChecked(first[0]);
                        UnSetChecked(third[0]);
                    } else {
                        SetChecked(second[1]);
                        UnSetChecked(first[1]);
                        UnSetChecked(third[1]);
                    }
                }
            }
        }

        for (var i = 0; i < first.length; i++) {
            if ($(second[i]).attr("checked") == 'checked') {
                $(SecondValueForAspNet).val($(second[i]).val());
            }
            if ($(first[i]).attr("checked") == 'checked') {
                $(FirstValueForAspNet).val($(first[i]).val());
            }
        }
    });
}


function SetChecked(element) {
    element.setAttribute("checked", "checked");
    element.checked = true;
    $(element).prop("disabled", true);
}

function UnSetChecked(element) {
    element.removeAttribute('checked');
    element.checked = false;
    $(element).prop("disabled", false);
}

function GetIndexOfSelectedElement(arr, element) {
    var index = -1;
    for (var i = 0; i < arr.length; i++) {
        if ($(element).attr('id') === $(arr[i]).attr('id')) {
            index = i;
            break;
        }
    }
    return index;
}