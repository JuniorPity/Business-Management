using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessManagement.Controllers
{
    public class ErrorController : Controller
    {
        private const string PROFILE_ERROR = "We can't find the profile that you requested. It is currently unavailable, or does not exist. Please try again later";

        public ActionResult Error(int error)
        {
            string message = ""; 

            switch (error)
            {
                case 1:
                    message = PROFILE_ERROR;
                    break;
                default:
                    break;
            }

            return View((object)message);
        }
    }
}