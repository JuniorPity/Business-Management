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
                string pass = GenerateHashedPassword(login.Salt, user.Password);
  
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
                    string salt = GenerateSalt();
                    string hashedPassword = GenerateHashedPassword(salt, user.Password);

                    user.Password = hashedPassword;
                    user.Salt = salt;
                    user.Role = "Standard";
                    user.Position = "N/A";
                    user.EmployeeID = user.FirstName[0] + user.LastName[0] + GenerateIdNumber(10);
                    user.RegDate = DateTime.Now;

                    if (string.IsNullOrEmpty(inviteCode))
                    {
                        Organization org = new Organization();

                        org.Label = user.Organization.Label;
                        org.Registered = DateTime.Now;
                        org.CodesCount = 1;
                        org.OrganizationID = org.Label + "#" + GenerateIdNumber(8);

                        db.Organizations.Add(org);
                        db.SaveChanges();

                        user.Organization = org;
                        user.OrganizationID = org.Id;
                    }
                    else
                    {
                        InviteCode code = db.InviteCodes.FirstOrDefault(i => i.Code == inviteCode);

                        if(code != null && !code.IsExpired)
                        {
                            user.OrganizationID = code.OrganizationID;

                            code.IsExpired = true;
                            code.DateExpired = DateTime.Now;

                            db.SaveChanges();
                        }
                    }

                    // Commit changes
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }

            return View("Login");
        }
        #endregion

        // Hashing and Random Algorithsm
        #region Hashing and Random Algorithms
        /*
        * Function: GenerateSalt()
        * Purpose: Generate a random salt to help hash passwords
        * Author: Jordan Pitner 9/20/2018
        */
        private string GenerateSalt()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 15)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        /*
        * Function: GenerateHashedPassword(string salt, string password)
        * Purpose: Generate a salted and hashed password for storage in the database
        * Author: Jordan Pitner 9/20/2018
        */
        private string GenerateHashedPassword(string salt, string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string hash = salt + password + salt;
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(hash));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }


        /*
        * Function: GenerateIdNumber()
        * Purpose: Generate a random series of numbers for EmployerID numbers
        * Author: Jordan Pitner 9/20/2018
        */
        private string GenerateIdNumber(int size)
        {
            Random random = new Random();
            const string chars = "0123456789";

            // Get random numerics from the char string
            return new string(Enumerable.Repeat(chars, size)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
    }
} 
