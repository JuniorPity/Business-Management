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
Purpose: Change window content when logout is selected
Author: Jordan Pitner 9/10/2018
*/
$(function () {
    $("#logout").on('click', function () {
        window.parent.location.href = $("#logoutValue").val();
    });
});

/*
Purpose: Change window content when logout is selected
Author: Jordan Pitner 9/10/2018
*/
$(function () {
    $("#profile").on('click', function () {
        window.parent.location.href = $("#profileValue").val();
    });
});

/*
Purpose: Change window content when logout is selected
Author: Jordan Pitner 9/10/2018
*/
$(function () {
    $("#badges").on('click', function () {
        window.parent.location.href = $("#badgesValue").val();
    });
});

/*
Purpose: Mouseup function to make the user pane disappear on click away
Author: Jordan Pitner 9/10/2018
*/
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

/*
Purpose: Prevent sign up submission if fields don't validate
         Each field will get errorField class added where error exists
         Each field with error will trigger an error message
Author: Jordan Pitner 9/20/2018
*/
$(function () {
    $("#signUpForm").on('submit', function (e) {
        PasswordValidation();
        NameValidation();
        ContactValidation();
        OrganizationValidation();
    });
});

/*
Purpose: Prevent sign up submission if passwords don't validate
Author: Jordan Pitner 9/20/2018
*/
function PasswordValidation()
{
    // Password matching and population
    if ($("#Password").val() != $("#confirmPassword").val()) {
        e.preventDefault();
        $("#Password").addClass("errorField");
        $("#confirmPassword").addClass("errorField");
        $("#errorRow").show();
    } else if ($("#Password").val() == "" && $("#confirmPassword").val() == "") {
        e.preventDefault();
        $("#Password").addClass("errorField");
        $("#confirmPassword").addClass("errorField");
        $("#errorRow2").show();
    } else if ($("#Password").val() == "") {
        e.preventDefault();
        $("#Password").addClass("errorField");
        $("#errorRow2").show();
    } else if ($("#confirmPassword").val() == "") {
        e.preventDefault();
        $("#confirmPassword").addClass("errorField");
        $("#errorRow2").show();
    } else {
        $("#Password").removeClass("errorField");
        $("#confirmPassword").removeClass("errorField");
        $("#errorRow").hide();
        $("#errorRow2").hide();
    }
}

/*
Purpose: Prevent sign up submission if first/last name don't validate
Author: Jordan Pitner 9/20/2018
*/
function NameValidation()
{
    // First Name field population
    if ($("#FirstName").val() == "") {
        e.preventDefault();
        $("#FirstName").addClass("errorField");
        $("#errorRow2").show();
    }
    else {
        $("#FirstName").removeClass("errorField");
        $("#errorRow2").hide();
    }

    // Last Name field population
    if ($("#LastName").val() == "") {
        e.preventDefault();
        $("#LastName").addClass("errorField");
        $("#errorRow2").show();
    }
    else {
        $("#LastName").removeClass("errorField");
        $("#errorRow2").hide();
    }
}

/*
Purpose: Prevent sign up submission if contact information doesn't validate
Author: Jordan Pitner 9/20/2018
*/
function ContactValidation()
{
    // Email field population
    if ($("#Email").val() == "") {
        e.preventDefault();
        $("#Email").addClass("errorField");
        $("#errorRow2").show();
    } else {
        $("#Email").removeClass("errorField");
        $("#errorRow2").hide();
    }

    // Phone field population
    if ($("#Phone").val() == "") {
        e.preventDefault();
        $("#Phone").addClass("errorField");
        $("#errorRow2").show();
    } else {
        $("#Phone").removeClass("errorField");
        $("#errorRow2").hide();
    }

    // DOB field population
    if ($("#DOB").val() == "") {
        e.preventDefault();
        $("#DOB").addClass("errorField");
        $("#errorRow2").show();
    } else {
        $("#DOB").removeClass("errorField");
        $("#errorRow2").hide();
    }
}

/*
Purpose: Prevent sign up submission if Organization doesn't validate
Author: Jordan Pitner 9/20/2018
*/
function OrganizationValidation()
{
    // Organization Name field population
    if ($("#Organization_Label").val() == "" || $("#Organization_Label").is(":visible")) {
        e.preventDefault();
        $("#Organization_Label").addClass("errorField");
        $("#errorRow2").show();
    } else {
        $("#Organization_Label").removeClass("errorField");
        $("#errorRow2").hide();
    }

    // Confirm Organization Name field popluation
    if ($("#confirmName").val() == "" || $("#confirmName").is(":visible")) {
        e.preventDefault();
        $("#confirmName").addClass("errorField");
        $("#errorRow2").show();
    } else {
        $("#confirmName").removeClass("errorField");
        $("#errorRow2").hide();
    }

    // Organization name matching
    if ($("#confirmName").val() != $("#Organization_Label").val() && $("#confirmName").is(":visible") && $("#Organization_Label").is(":visible")) {
        e.preventDefault();
        $("#errorRow3").show();
    }
}