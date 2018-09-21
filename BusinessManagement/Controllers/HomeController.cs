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

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                User obj = db.Users.FirstOrDefault(a => a.Email.Equals(username) && a.Password.Equals(password));

                if (obj != null)
                {
                    Session["UserID"] = obj.Id.ToString();
                    Session["Email"] = obj.Email.ToString();

                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
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
