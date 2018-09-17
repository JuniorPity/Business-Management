/*
Purpose: Toggle show/hide for the user information panel
Author: Jordan Pitner 9/16/2018
*/
function ToggleUserPane() {
    var frame = $("#userFrame");

    // Show/hide logic
    if (frame.is(":hidden")) {
        frame.show();
    }
    else {
        frame.hide();
    }
}