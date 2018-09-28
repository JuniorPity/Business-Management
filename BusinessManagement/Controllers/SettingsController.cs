using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessManagement.Models;
using BusinessManagement.Models.Authentication;

namespace BusinessManagement.Controllers
{
    public class SettingsController : Controller
    {
        BusinessDataEntities db = new BusinessDataEntities();
        MembershipAuth membership = new MembershipAuth();

        // GET: Settings
        [Authorize]
        public ActionResult UserSettings()
        {
            return View();
        }

        // Profile Actions
        #region Profile Actions


        /*
        * Function: EditProfile()
        * Purpose: Update user information
        * Author: Jordan Pitner 9/27/2018
        */
        [Authorize]
        [HttpGet]
        public ActionResult EditProfile()
        {
            string userName = membership.GetCurrentUser(HttpContext.Request);
            int userID = db.Users.FirstOrDefault(u => u.Email == userName).Id;

            if (!(userID > 0))
            {
                return RedirectToAction("Login", "Home", null);
            }

            // Grab the user information for display
            User user = db.Users.FirstOrDefault(u => u.Id == userID);

            return View(user);
        }


        /*
        * Function: EditTimeEntry(User user)
        * Purpose: Post event to push the data to the database
        * Author: Jordan Pitner 9/27/2018
        */
        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(User user)
        {
            if(ModelState.IsValid)
            {
                // Validation that some other user isn't trying to access information
                string userName = membership.GetCurrentUser(HttpContext.Request);
                int userID = db.Users.FirstOrDefault(u => u.Email == userName).Id;

                // If so, return back to login
                if (!(userID > 0))
                {
                    return RedirectToAction("Login", "Home", null);
                }

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        #endregion
    }
}