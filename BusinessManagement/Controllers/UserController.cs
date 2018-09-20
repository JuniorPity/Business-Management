using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessManagement.Models;
using System.Globalization;

namespace BusinessManagement.Controllers
{
    public class UserController : Controller
    {
        private BusinessDataEntities db = new BusinessDataEntities();

        // Profile Pages
        #region Profile Pages
        /*
        * Function: ProfilePage(string id = null)
        * Purpose: Get the user information for the profile page display
        * Author: Jordan Pitner 9/20/2018
        */
        public ActionResult ProfilePage(string id = null)
        {
            int userID;
            User user;

            // Checks to see if you are viewing yourself, or another user
            if (id == null)
            {
                // Validate that a session exists, or re-route to login
                userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

                if (userID == -1)
                {
                    return RedirectToAction("Login", "Home", null);
                }

                // No user was passed in, get your own information
                user = db.Users.SingleOrDefault(u => u.Id == userID);
            }
            else
            {
                // Another user was passed in, find them
                user = db.Users.SingleOrDefault(u => u.EmployeeID == id);
            }

            // If the user is found, display the page. Otherwise, display an error
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { error = 1 });
            }
        }
        #endregion

        public ActionResult Badges()
        {
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if (userID == -1)
            {
                return RedirectToAction("Login", "Home", null);
            }

            return View();
        }
    }
}