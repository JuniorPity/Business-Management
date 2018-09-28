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