using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessManagement.Models;

namespace BusinessManagement.Models.Logging
{ 
    public class EventLogger
    {
        private static BusinessDataEntities db = new BusinessDataEntities();

        /*
        * Function: LogNewEvent(int userID, int organizationID, LoggingEventType type, string details)
        * Purpose: Log a new event based on the parameters passed to it
        * Author: Jordan Pitner 10/3/2018
        */
        public static void LogNewEvent(int userID, int organizationID, LoggingEventType type, string details)
        {
            // ID from the enum type
            int eventType = (int)type;

            // Get the label/type of the event
            LogEventType et = db.LogEventTypes.SingleOrDefault(e => e.Id == eventType);
            string detail = et.Label.Trim() + " " + details.Trim();

            // Create the new log event
            LogEvent newEvent = new LogEvent {
                OrganizationID = organizationID,
                UserID = userID,
                EventType = eventType,
                Date = DateTime.Now,
                Details = detail
            };

            // Save to the database
            db.LogEvents.Add(newEvent);
            db.SaveChanges();
        }
    }
}