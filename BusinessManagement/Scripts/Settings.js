/*
Purpose: Change window content when my profile is selected
Author: Jordan Pitner 9/10/2018
*/
$(function () {
    $("#myProfile").on('click', function () {
        $(".settingsPanel li").removeClass("settingsActive");
        $(".settingsPanel li").addClass("settings");
        $("#myProfile").removeClass("settings").addClass("settingsActive");
        $("#settingsFrame").attr('src', $("#profileValue").val());
    });
});

/*
Purpose: Change window content when my profile is selected
Author: Jordan Pitner 9/10/2018
*/
$(function () {
    $("#appearance").on('click', function () {
        $(".settingsPanel li").removeClass("settingsActive");
        $(".settingsPanel li").addClass("settings");
        $("#appearance").removeClass("settings").addClass("settingsActive");
        $("#settingsFrame").attr('src', "www.google.com");
    });
});


/*
Purpose: Change window content when my profile is selected
Author: Jordan Pitner 9/10/2018
*/
$(function () {
    $("#invites").on('click', function () {
        $(".settingsPanel li").removeClass("settingsActive");
        $(".settingsPanel li").addClass("settings");
        $("#invites").removeClass("settings").addClass("settingsActive");
        $("#settingsFrame").attr('src', $("#inviteValue").val());
    });
});

function showChangesActive()
{
    $("#saveChanges").removeClass('disabled');
    $("#alertChanges").show();
}

/*
Purpose: Detect last name input changes
Author: Jordan Pitner 10/3/2018
*/
$(function () {
    $("#LastName").on("input", function () {
        showChangesActive();
    });
});

/*
Purpose: Detect last name input changes
Author: Jordan Pitner 10/3/2018
*/
$(function () {
    $("#FirstName").on("input", function () {
        showChangesActive();
    });
});

/*
Purpose: Detect skills input changes
Author: Jordan Pitner 10/3/2018
*/
$(function () {
    $("#Skills").on("input", function () {
        showChangesActive();
    });
});

/*
Purpose: Detect about me input changes
Author: Jordan Pitner 10/3/2018
*/
$(function () {
    $("#AboutMe").on("input", function () {
        showChangesActive();
    });
});

/*
Purpose: Detect email input changes
Author: Jordan Pitner 10/3/2018
*/
$(function () {
    $("#Email").on("input", function () {
        showChangesActive();
    });
});

/*
Purpose: Detect phone input changes
Author: Jordan Pitner 10/3/2018
*/
$(function () {
    $("#Phone").on("input", function () {
        showChangesActive();
    });
});

/*
Purpose: Detect phone input changes
Author: Jordan Pitner 10/3/2018
*/
$(function () {
    $("#profilePicture").on("input", function () {
        showChangesActive();
    });
});