/*
Function: N/A
Descrption: Initalizer for the calendar for time entries (time cards)
Date: 5/15/2017
Author: Jordan Pitner
*/
function summaryFrameResize () {
    document.getElementById("summaryFrame").height = window.document.body.scrollHeight + "px";
}

function closePanel() {
    document.getElementById("resize").style.display = 'none';
}

function CancelDelete() {
    $("#deletePromptModal").modal('hide');
    $('#timeEditModal').modal('show');
}

function DeleteTimeEntryPrompt() {
    $("#deletePromptModal").modal('show');
    $('#timeEditModal').modal('hide');
}

/*
Function: GetTimeEntry()
Description: Convert milliseconds/time string to readable time format
Date: 9/9/2018
Author: Jordan Pitner
*/
function convertToReadableFormat(timeString) {
    var timeIndicator = "AM";
    var time = timeString.split(" ")[0];
    var splitTime = time.split(":");

    var hours = splitTime[0];
    var mins = splitTime[1];

    var hourInt = parseInt(hours);

    if (hourInt > 12) {
        hourInt -= 12;
        timeIndicator = "PM";

        if (hourInt < 10) {
            hourInt = "0" + hourInt;
        }
    }
    else if (hourInt < 10) {
        hourInt = "0" + hourInt;
    }

    return hourInt + ":" + mins + " " + timeIndicator;
}

/*
Function: ProcessDate()
Description: Process the selected calendar date to get the proper format to post to the calendar
and display a new entry
Date: 5/15/2017
Author: Jordan Pitner
*/
function ProcessDate(element) {
    // Get the date from the modal, get the substring for actual date
    var dateText = element.innerText;
    var dateSub = dateText.substring(dateText.indexOf('- '), dateText.length);
    var text = dateSub.substring(dateSub.indexOf(' '), dateSub.length);

    var date = new Date(text);
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var day = date.getDate();

    var mm = ''
    var dd = ''

    if (month < 10) {
        mm = '0' + month;
    }
    else {
        mm = '' + month;
    }

    if (day < 10) {
        1
        dd = '0' + day;
    }
    else {
        dd = '' + day;
    }
    // Return the newly formatted date
    return year + '-' + mm + '-' + dd + 'T';
}

/*
Function: ProcessTime(time)
Descrption: Format the time so that the calendar can read in the data and populate the cells
Date: 5/15/2017
Author: Jordan Pitner
*/
function ProcessTime(time) {
    // Break up the minutes from the hours to process
    var sHours = time.slice(0, time.indexOf(":"));
    var sMinutes = time.slice(time.indexOf(":") + 1, time.indexOf(" "));
    var iHours = 0;
    var iMinutes = 0;

    // Change to military time if needed (is a PM time)
    if (sHours.charAt(0) == '0' && time.includes('PM')) {
        sHours = sHours.charAt(1);
        iHours = parseInt(sHours) + 12;
    }
    else if (time.includes('PM')) {
        iHours = parseInt(sHours) + 12;
    }
    else {
        // Return if no military conversion needed to be done
        return sHours + ':' + sMinutes + ':' + '00';
    }

    // Return if a military conversion needed to be done
    return iHours.toString() + ':' + sMinutes + ':' + '00';
}

/*
Function: generateTotalHours(start, end)
Descrption: Find the time difference between start and end for total hours
Date: 5/15/2017
Author: Jordan Pitner
*/
function generateTotalHours(start, end) {
    var startHours = 0.0;
    var endHours = 0.0;
    var date = new Date(start);
    var date2 = new Date(end);
    var diff = (date2 - date) / 60000 / 60;

    return diff.toFixed(2) + " hrs";
}

/*
Function: convertEpoch
Descrption: Convert time to a readable date format, including date of the week
Date: 5/15/2017
Author: Jordan Pitner
*/
function convertEpoch(time) {
    var d = new Date(time);

    // Get month, day, date, year
    var dd = d.getUTCDate();
    var mm = d.getUTCMonth() + 1;
    var yyyy = d.getUTCFullYear();
    var day = d.getUTCDay();

    // Days of the week in non-numeric form
    var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

    return days[day] + " " + mm + "/" + dd + "/" + yyyy;
}

/*
Function: getCurrentDate()
Descrption: Return the current date in yyyy-mm-dd format
Date: 5/15/2017
Author: Jordan Pitner
*/
function getCurrenteDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }

    today = yyyy + '-' + mm + '-' + dd;

    return today;
}

