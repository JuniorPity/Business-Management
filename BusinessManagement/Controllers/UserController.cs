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

        // GET: User
        public ActionResult ProfilePage(string id = null)
        {
            int userID;
            User user;

            if (id == null)
            {
                // Validate that a session exists, or re-route to login
                userID = Session["UserID"] != null ? int.Parse(Session["UserID"].ToString()) : -1;

                if (userID == -1)
                {
                    return RedirectToAction("Login", "Home", null);
                }

                user = db.Users.SingleOrDefault(u => u.Id == userID);
            }
            else
            {
                user = db.Users.SingleOrDefault(u => u.EmployeeID == id);
            }

            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { error = 1 });
            }
        }
    }
}