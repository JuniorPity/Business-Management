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

namespace BusinessManagement.Controllers
{
    public class HomeController : Controller
    {
        private BusinessDataEntities db = new BusinessDataEntities();

        public ActionResult UserPane()
        {
            // Validate that a session exists, or re-route to login
            int userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

            if (userID == -1)
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
        public ActionResult Login(User user)
        {
            // Check that credentials were entered
            if (user != null)
            {
                User userFirst = db.Users.FirstOrDefault(a => a.Email.Equals(user.Email));
                string pass = GenerateHashedPassword(userFirst.Salt, user.Password);

                User obj = db.Users.FirstOrDefault(a => a.Email.Equals(user.Email) && a.Password.Equals(pass));

                // If a user was found, log them in
                if (obj != null)
                {
                    Session["UserID"] = obj.Id.ToString();
                    Session["Email"] = obj.Email.ToString();

                    return RedirectToAction("Time", "TimeCard");
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
            Session["UserID"] = null;
            Session["Email"] = null;

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
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user)
        {
            if (user != null)
            {
                user.OrganizationID = 1;

                if (ModelState.IsValid)
                {
                    string salt = GenerateSalt();
                    string hashedPassword = GenerateHashedPassword(salt, user.Password);

                    user.Password = hashedPassword;
                    user.Salt = salt;
                    user.Role = "SiteAdmin";
                    user.Position = "CEO";
                    user.EmployeeID = "JP" + GenerateIdNumber();
                    user.RegDate = DateTime.Now;

                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }

            return View();
        }
        #endregion

        #region Hashing Algorithms

        private string GenerateSalt()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 15)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

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

        private string GenerateIdNumber()
        {
            Random random = new Random();
            const string chars = "0123456789";

            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
    }
} 
