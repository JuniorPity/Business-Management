using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessManagement.Models;
using System.Globalization;
using BusinessManagement.Models.Authentication;
namespace BusinessManagement.Controllers
{
    public class UserController : Controller
    {
        private BusinessDataEntities db = new BusinessDataEntities();
        private MembershipAuth membership = new MembershipAuth();

        // Profile Pages
        #region Profile Pages
        /*
        * Function: ProfilePage(string id = null)
        * Purpose: Get the user information for the profile page display
        * Author: Jordan Pitner 9/20/2018
        */
        [Authorize]
        public ActionResult ProfilePage(string id = null)
        {
            int userID;
            User user;

            // Checks to see if you are viewing yourself, or another user
            if (id == null)
            {
                // Validate that a session exists, or re-route to login
                string userName = membership.GetCurrentUser(HttpContext.Request);
                userID = db.Users.FirstOrDefault(u => u.Email == userName).Id;

                if (!(userID > 0))
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

        [Authorize]
        public ActionResult Badges()
        {
            // Validate that a session exists, or re-route to login
            string userName = membership.GetCurrentUser(HttpContext.Request);
            int userID = db.Users.FirstOrDefault(u => u.Email == userName).Id;

            if (!(userID > 0))
            {
                return RedirectToAction("Login", "Home", null);
            }

            return View();
        }
    }
}