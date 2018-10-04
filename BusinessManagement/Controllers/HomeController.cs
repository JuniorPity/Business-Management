using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Events.Month;
using DayPilot.Web.Mvc.Enums;
using BusinessManagement.Models;
using System.Diagnostics;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using BusinessManagement.Models.Authentication;
using BusinessManagement.Models.Logging;
using BusinessManagement.Models.StringBuilding;

namespace BusinessManagement.Controllers
{
    public class HomeController : Controller
    {
        private BusinessDataEntities db = new BusinessDataEntities();

        public ActionResult UserPane()
        {
            // Validate that a session exists, or re-route to login
            string userName = MembershipAuth.GetCurrentUser(HttpContext.Request);
            int userID = db.Users.FirstOrDefault(u => u.Email == userName).Id;

            if (!(userID > 0))
            {
                return RedirectToAction("Login", "Home", null);
            }

            return View();
        }

        // Login/Logout Actions
        #region Login/Logout

        /*
        * Function: Login()
        * Purpose: Display the login view
        * Author: Jordan Pitner 9/20/2018
        */
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /*
       * Function: Login(User user)
       * Purpose: Display the login view with error OR login the user in
       * Author: Jordan Pitner 9/20/2018
       */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user, string ReturnUrl)
        {
            // Check that credentials were entered
            if (user != null || !string.IsNullOrEmpty(user.Email))
            {
                // Find the user with the appropriate username, and check hashed password
                User login = db.Users.FirstOrDefault(a => a.Email.Equals(user.Email));
                string pass = StringManipulator.GenerateHashedPassword(login.Salt, user.Password);
  
                // If a user was found, log them in
                if (login != null && login.Password == pass)
                {
                    FormsAuthentication.SetAuthCookie(login.Email.Trim(), false);
                    
                    // If user was previouly logged in, redirect them to their previous page. 
                    // Otherwise send them to the default page.
                    if (this.Url.IsLocalUrl(ReturnUrl))
                    {
                        EventLogger.LogNewEvent(login.Id, login.OrganizationID, LoggingEventType.UserLogin, "");
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        EventLogger.LogNewEvent(login.Id, login.OrganizationID, LoggingEventType.UserLogin, "");
                        return RedirectToAction("Time", "TimeCard");
                    }            
                }
            }

            // A user wasn't found, try again
            ViewBag.Error = true;

            return View();
        }

        /*
       * Function: Logout()
       * Purpose: Return to login page and clear the session
       * Author: Jordan Pitner 9/20/2018
       */
        public ActionResult Logout()
        {
            // Log sign out event
            EventLogger.LogNewEvent(MembershipAuth.GetCurrentUserID(HttpContext.Request), MembershipAuth.GetCurrentUserOrganizationID(HttpContext.Request), LoggingEventType.UserLogout, "");

            // Signout
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

        #endregion

        // Sign Up Actions
        #region Sign Up

        /*
         * Function: SignUp()
        * Purpose: Display the signup page
        * Author: Jordan Pitner 9/20/2018
        */
        [HttpGet]
        public ActionResult SignUp(string invite)
        {
            return View();
        }


        /*
         * Function: SignUp(User user)
        * Purpose: Create the user with all the pertinent information
        * Author: Jordan Pitner 9/20/2018
        */
        [HttpPost]
        public ActionResult SignUp(User user, string inviteCode)
        {
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    // Generate salt and salted/hashed password for db storage
                    string salt = StringManipulator.GenerateSalt();
                    string hashedPassword = StringManipulator.GenerateHashedPassword(salt, user.Password);

                    // Set user properties
                    user.Password = hashedPassword;
                    user.Salt = salt;
                    user.Role = "Standard";
                    user.Position = "N/A";
                    user.EmployeeID = user.FirstName[0] + user.LastName[0] + StringManipulator.GenerateIdNumber(8);
                    user.RegDate = DateTime.Now;

                    // If an invite code was present, join that org. If not, create a new one
                    if (string.IsNullOrEmpty(inviteCode))
                    {
                        Organization org = new Organization();

                        // Set Organization properties
                        org.Label = user.Organization.Label;
                        org.Registered = DateTime.Now;
                        org.CodesCount = 1;
                        org.OrganizationID = org.Label + "#" + StringManipulator.GenerateIdNumber(8);

                        // Add new org to database
                        db.Organizations.Add(org);
                        db.SaveChanges();

                        // Link the user to the newly created org
                        user.Organization = org;
                        user.OrganizationID = org.Id;
                    }
                    else
                    {
                        // Find the organization relating to the invite code
                        InviteCode code = db.InviteCodes.FirstOrDefault(i => i.Code == inviteCode);

                        // If the code is valid, 
                        if(code != null && !code.IsExpired)
                        {
                            user.OrganizationID = code.OrganizationID;

                            code.IsExpired = true;
                            code.DateExpired = DateTime.Now;

                            // Commit invite code changes
                            db.SaveChanges();
                        }
                    }

                    // Commit user changes
                    db.Users.Add(user);
                    db.SaveChanges();

                    // Log the user creation event
                    EventLogger.LogNewEvent(user.Id, user.OrganizationID, LoggingEventType.UserCreated, "");
                }
            }

            return View("Login");
        }
        #endregion
    }
} 
