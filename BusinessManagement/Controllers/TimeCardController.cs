using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessManagement.Models;
using System.Globalization;

namespace BusinessManagement.Controllers
{
    public class TimeCardController : Controller
    {
        private BusinessDataEntities db = new BusinessDataEntities();

        // Time Card Functions
        #region Time Card
        // GET: Time
        public ActionResult Time()
        {
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if (userID == -1)
            {
                return RedirectToAction("Login", "Home", null);
            }

            return View();
        }

        /*
         * Function: CreateTimeEntry(TimeEvent timeEvent)
         * Purpose: Input a new time card entry into the database
         * Author: Jordan Pitner 9/9/2018
         */
        public ActionResult CreateTimeEntry(TimeEvent timeEvent)
        {
            TimeEvent te = new TimeEvent();
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if (userID == -1)
            {
                return RedirectToAction("Login", "Home", null);
            }

            te.title = timeEvent.title;
            te.start = timeEvent.start;
            te.end = timeEvent.end;
            te.userID = userID;

            // Commit to database
            db.TimeEvents.Add(te);
            db.SaveChanges();

            return Json(new { success = true, id = te.id });
        }

        /*
        * Function: EditTimeEntry(TimeEvent timeEvent)
        * Purpose: Update a time entry record with new information
        * Author: Jordan Pitner 9/9/2018
        */
        public ActionResult EditTimeEntry(TimeEvent timeEvent)
        {
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if (userID == -1)
            {
                return RedirectToAction("Login", "Home", null);
            }

            TimeEvent timeEntry = db.TimeEvents.Where(t => t.id == timeEvent.id).FirstOrDefault();

            if (timeEntry != null)
            {
                timeEntry.start = timeEvent.start;
                timeEntry.end = timeEvent.end;
                timeEntry.title = timeEvent.title;

                // Commit to database
                db.SaveChanges();

                return Json(new { success = true, id = timeEntry.id });
            }

            return Json(new { sucess = false });
        }

        /*
        * Function: DeleteTimeEntry(int id) 
        * Purpose: Delete a time entry record from the database
        * Author: Jordan Pitner 9/9/2018
        */
        public ActionResult DeleteTimeEntry(int id)
        {
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if (userID == -1)
            {
                return RedirectToAction("Login", "Home", null);
            }

            TimeEvent timeEntry = db.TimeEvents.Where(t => t.id == id).FirstOrDefault();

            if (timeEntry != null)
            {
                // Commit to database
                db.TimeEvents.Remove(timeEntry);
                db.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { sucess = false });
        }

        /*
        * Function: GetTimeEntry(int id)
        * Purpose: Retrieve a time entry record from the database for display
        * Author: Jordan Pitner 9/9/2018
        */
        public ActionResult GetTimeEntry(int id)
        {
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if (userID == -1)
            {
                return RedirectToAction("Login", "Home", null);
            }

            TimeEvent timeEntry = db.TimeEvents.Where(t => t.id == id).FirstOrDefault();

            if (timeEntry != null)
            {
                return Json(new { success = true, start = timeEntry.start, end = timeEntry.end });
            }

            return Json(new { sucess = false });
        }

        /*
        * Function: GetTimeEntries()
        * Purpose: Return all entries for a given user (all time card entries)
        * Author: Jordan Pitner 9/9/2018
        */
        public ActionResult GetTimeEntries()
        {
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if (userID == -1)
            {
                return RedirectToAction("Login", "Home", null);
            }

            List<Event> timeEntries = new List<Event>();
            List<TimeEvent> events = db.TimeEvents.Where(t => t.userID == userID).ToList();

            foreach(TimeEvent ev in events)
            {
                timeEntries.Add(new Event
                {
                    id = ev.id,
                    start = ev.start,
                    end = ev.end,
                    title = ev.title
                });
            }

            return Json(timeEntries, JsonRequestBehavior.AllowGet);
        }

        #endregion

        // Time Summary Functions
        #region Time Summary
        
        // GET: Summary
        public ActionResult Summary()
        {
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if (userID == -1)
            {
                return RedirectToAction("Login", "Home", null);
            }

            return View();
        }

        /*
        * Function: WeeklySummary()
        * Purpose: Build the model for the weekly summary and return its view + data
        * Author: Jordan Pitner 9/10/2018
        */
        public ActionResult WeeklySummary()
        {
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if(userID == -1)
            {
                return RedirectToAction("Login", "Home", null);
            }

            User user = db.Users.Where(u => u.Id == userID).FirstOrDefault();
            string name = user.FirstName + " " + user.LastName;

            // Get the data from the database
            Summary summary = new Summary();
            List<TimeEvent> times = db.TimeEvents.Where(t => t.userID == userID).ToList();
           
            summary.name = name;
            summary.payRate = 9.50;
            summary.events = new Dictionary<DateTime, string>();

            // Add the appropriate date/times to the dictionary for hours processing
            foreach (TimeEvent ev in times)
            {
                DateTime start = Convert.ToDateTime(ev.start);

                if (start > DateTime.Now.AddDays(-7) && start < DateTime.Now)
                {
                    DateTime end = Convert.ToDateTime(ev.end);
                    double hours = (end - start).TotalHours;

                    summary.totalHours += Math.Round(hours, 2);
                    summary.events.Add(start.Date, Math.Round(hours, 2).ToString());
                }
            }

            // Sort and set any remaining variables
            summary.events = summary.events.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            summary.totalPay = summary.totalHours * summary.payRate;
            summary.totalPay = Math.Round(summary.totalPay, 2);

            return View(summary);
        }

        /*
        * Function: MonthlySummary(string month)
         * Purpose: Build the model for the monthly summary and return its view + data
        * Author: Jordan Pitner 9/10/2018
        */
        public ActionResult MonthlySummary(string month, string year)
        {
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if (userID == -1)
            {
                return RedirectToAction("Login", "Home", null);
            }

            User user = db.Users.Where(u => u.Id == userID).FirstOrDefault();
            string name = user.FirstName + " " + user.LastName;

            // Get the data from the database
            Summary summary = new Summary();
            List<TimeEvent> times = db.TimeEvents.Where(t => t.userID == userID).ToList().Where(t => 
                                    Convert.ToDateTime(t.start).Month == DateTime.ParseExact(month.Trim(), "MMMM", CultureInfo.CurrentCulture).Month && 
                                    Convert.ToDateTime(t.start).Year == new DateTime(int.Parse(year.Trim()), 1, 1).Year).ToList();

            summary.name = name;
            summary.payRate = 9.50;
            summary.events = new Dictionary<DateTime, string>();

            // Add the appropriate date/times to the dictionary for hours processing
            foreach (TimeEvent ev in times)
            {
                DateTime start = Convert.ToDateTime(ev.start);

                DateTime end = Convert.ToDateTime(ev.end);
                double hours = (end - start).TotalHours;

                summary.totalHours += Math.Round(hours, 2);
                summary.events.Add(start.Date, Math.Round(hours, 2).ToString());
            }

            // Sort and set any remaining variables
            summary.events = summary.events.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            summary.totalPay = summary.totalHours * summary.payRate;
            summary.totalPay = Math.Round(summary.totalPay, 2);

            return View(summary);
        }

        public ActionResult YearlySummary(string year)
        {
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if (userID == -1)
            {
                return RedirectToAction("Login", "Home", null);
            }

            User user = db.Users.Where(u => u.Id == userID).FirstOrDefault();
            string name = user.FirstName + " " + user.LastName;

            // Get the data from the database
            Summary summary = new Summary();
            List<TimeEvent> times = db.TimeEvents.Where(t => t.userID == userID).ToList().Where(t => 
                                    Convert.ToDateTime(t.start).Year == new DateTime(int.Parse(year.Trim()), 1, 1).Year).ToList();

            summary.name = name;
            summary.payRate = 9.50;
            summary.yearlyEvents = new Dictionary<int, string>();

            // Add the appropriate date/times to the dictionary for hours processing
            foreach (TimeEvent ev in times)
            {
                DateTime start = Convert.ToDateTime(ev.start);
                DateTime end = Convert.ToDateTime(ev.end);

                int month = start.Month;
                double hours = (end - start).TotalHours;

                summary.totalHours += Math.Round(hours, 2);

                if(summary.yearlyEvents.ContainsKey(month))
                {
                    summary.yearlyEvents[month] = (double.Parse(summary.yearlyEvents[month]) + hours).ToString("0.00") ;
                }
                else
                {
                    summary.yearlyEvents.Add(month, Math.Round(hours, 2).ToString());
                }
            }

            // Sort and set any remaining variables
            summary.yearlyEvents = summary.yearlyEvents.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            summary.totalPay = summary.totalHours * summary.payRate;
            summary.totalPay = Math.Round(summary.totalPay, 2);

            return View(summary);
        }
        #endregion
    }
}