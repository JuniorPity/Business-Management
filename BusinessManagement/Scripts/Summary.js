/*
Purpose: Change the text and IFrame content when week view is selected
Author: Jordan Pitner 9/10/2018
*/
$(function () {
    $("#weekSelector").on('click', function () {
        $("#timeFrame").html("Week View <span class='caret'></span>");
        $("#monthButton").css("display", "none");
        $("#yearButton").css("display", "none");

        $("#summaryFrame").attr('src', $("#weekValue").val());
    });
});

/*
Purpose: Change the text and IFrame content when month view is selected
Author: Jordan Pitner 9/10/2018
*/
$(function () {
    $("#monthSelector").on('click', function () {
        $("#timeFrame").html("Monthly View <span class='caret'></span>");
        $("#monthButton").css("display", "inline-block");
        $("#yearButton").css("display", "inline-block");

        $("#summaryFrame").attr('src', $("#monthValue").val().replace("January", $("#monthDropDown").text()).replace("1999", $("#yearDropDown").text()));
    });
});

/*
Purpose: Change the text and IFrame content when year view is selected
Author: Jordan Pitner 9/10/2018
*/
$(function () {
    $("#yearSelector").on('click', function () {
        $("#timeFrame").html("Yearly View <span class='caret'></span>");
        $("#yearButton").css("display", "inline-block");
        $("#monthButton").css("display", "none");

        $("#summaryFrame").attr('src', $("#yearValue").val().replace("1999", $("#yearDropDown").text()));
    });
});

/*
Purpose: Get the needed URL's and data to show the monthly summaries
Author: Jordan Pitner 9/14/2018
*/
function ChangeMonth(button)
{
    var month = button.textContent.trim();
    var year = document.getElementById("yearDropDown").textContent.trim();

    // Update dropdown value
    $("#monthDropDown").html(month + " <span class='caret'></span>");

    // Update the IFrame to display the correct data
    $("#summaryFrame").attr('src', $("#monthValue").val().replace("January", month).replace("1999", year));
}

/*
Purpose: Change the year, based on if monthly is also active, to display yearly data
Author: Jordan Pitner 9/14/2018
*/
function ChangeYear(button)
{
    var monthButton = $("#monthDropDown");
    var year = button.textContent.trim();
    var dropdown = $("#yearDropDown");

    // Update dropdown value
    $("#yearDropDown").html(year + " <span class='caret'></span>");

    // If month is active, then pass to monthly server code; else call the yearly server code
    if(monthButton.is(":visible"))
    {
        var month = monthButton[0].textContent.trim();
        $("#summaryFrame").attr('src', $("#monthValue").val().replace("January", month).replace("1999", year));
    }
    else
    {
        $("#summaryFrame").attr('src', $("#yearValue").val().replace("1999", year));
    }
}
