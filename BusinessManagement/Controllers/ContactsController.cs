using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessManagement.Models;
using BusinessManagement.Models.Authentication;
namespace BusinessManagement.Controllers
{
    public class ContactsController : Controller
    {
        BusinessDataEntities db = new BusinessDataEntities();
        MembershipAuth membership = new MembershipAuth();

        [Authorize]
        public ActionResult AllContacts()
        {
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