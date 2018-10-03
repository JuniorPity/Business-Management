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

        public static void LogNewEvent(int userID, int organizationID, LoggingEventType type, string details)
        {
            int eventType = (int)type;

            LogEventType et = db.LogEventTypes.SingleOrDefault(e => e.Id == eventType);

            LogEvent newEvent = new LogEvent {
                OrganizationID = organizationID,
                UserID = userID,
                EventType = eventType,
                Date = DateTime.Now,
                Details = et.Label + " " + details
            };

            db.LogEvents.Add(newEvent);
            db.SaveChanges();
        }
    }
}