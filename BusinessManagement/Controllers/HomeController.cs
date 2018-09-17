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

namespace BusinessManagement.Controllers
{
    public class HomeController : Controller
    {
        private BusinessDataEntities db = new BusinessDataEntities();

        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult UserPane()
        {
            return View();
        }

        public ActionResult Timecard()
        {
            return View();
        }

        // Login/Logout Actions
        #region Login/Logout

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                User obj = db.Users.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(user.Password)).FirstOrDefault();

                if (obj != null)
                {
                    Session["UserID"] = obj.Id.ToString();
                    Session["Email"] = obj.Email.ToString();

                    return RedirectToAction("Time", "TimeCard");
                }
            }

            ViewBag.Error = "true";
            return View();
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["Email"] = null;

            return RedirectToAction("Login");
        }

        #endregion
    }
} 
