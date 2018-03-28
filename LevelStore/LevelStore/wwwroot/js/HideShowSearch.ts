


function HideOrShow() {
    let searchField = $("#SearchField");
    if ($("#SearchField").is(":visible")) {
        searchField.hide(1000);
    } else {
        searchField.show(1000);
    }
}


window.onload = () => {
    let searchField = $("#SearchField");
    $("#SearchIcon").click(() => { HideOrShow(); });
    if ($(searchField).val() !== "") {
        searchField.show(0);
    }
};