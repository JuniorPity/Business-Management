/*
Purpose: Toggle show/hide for the user information panel
Author: Jordan Pitner 9/16/2018
*/
function ToggleUserPane() {
    var frame = $("#userFrame");
    var chevron = $("#userChevron");

    // Show/hide logic
    if (frame.is(":hidden")) {
        frame.show();
        chevron.removeClass("fa-chevron-up");
        chevron.addClass("fa-chevron-down");
    }
    else {
        frame.hide();
        chevron.addClass("fa-chevron-up");
        chevron.removeClass("fa-chevron-down");
    }
}

/*
Purpose: Change the text and IFrame content when week view is selected
Author: Jordan Pitner 9/10/2018
*/
$(function () {
    $("#logout").on('click', function () {
        window.parent.location.href = $("#logoutValue").val();
    });
});

/*
Purpose: Change the text and IFrame content when week view is selected
Author: Jordan Pitner 9/10/2018
*/
$(function () {
    $("#profile").on('click', function () {
        window.parent.location.href = $("#profileValue").val();
    });
});

$(document).mouseup(function (e) {
    var container = $("#userFrame");
    var picture = $("#userPhoto");
    var chevron = $("#userChevron");

    if (container.is(e.target) || picture.is(e.target) || chevron.is(e.target))
    {
        ToggleUserPane();
    }
    // if the target of the click isn't the container nor a descendant of the container
    else if ((!container.is(e.target) && container.has(e.target).length === 0)) {
        container.hide();
        chevron.addClass("fa-chevron-up");
        chevron.removeClass("fa-chevron-down");
    }
});