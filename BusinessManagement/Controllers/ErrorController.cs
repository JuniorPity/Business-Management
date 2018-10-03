using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessManagement.Models.Logging;
using BusinessManagement.Models.Authentication;

namespace BusinessManagement.Controllers
{
    public class ErrorController : Controller
    {
        // Error Constants
        #region Constants

        private const string PROFILE_ERROR = "We can't find the profile that you requested. It is currently unavailable, or does not exist. Please try again. \n ERROR: PROFILE_ERROR";
        private const string PROFILE_EDIT_ERROR = "There was an error processing your edit request. Please try again. \n ERROR: PROFILE_EDIT_ERROR";
        private const string PERMISSION_ERROR = "You do not have permission to access this functionality. Please contact your system adminstartor. \n PERMISSION_ERROR";

        #endregion

        // Actions
        #region Actions

        /*
        * Function: Error(int error)
        * Purpose: Return errors based on values passed in server side
        * Author: Jordan Pitner 9/27/2018
        */
        public ActionResult Error(int error)
        {
            string message = ""; 

            // Determine which error should be fired
            switch (error)
            {
                // Profile Error
                case 1:
                    message = PROFILE_ERROR;
                    EventLogger.LogNewEvent(MembershipAuth.GetCurrentUserID(HttpContext.Request), 
                                MembershipAuth.GetCurrentUserOrganizationID(HttpContext.Request), 
                                LoggingEventType.Error, "PROFILE_ERROR");
                    break;
                // Profile Edit Error
                case 2:
                    message = PROFILE_EDIT_ERROR;
                    EventLogger.LogNewEvent(MembershipAuth.GetCurrentUserID(HttpContext.Request),
                                MembershipAuth.GetCurrentUserOrganizationID(HttpContext.Request),
                                LoggingEventType.Error, "PROFILE_EDIT_ERROR");
                    break;
                // Permission Error
                case 3:
                    message = PERMISSION_ERROR;
                    EventLogger.LogNewEvent(MembershipAuth.GetCurrentUserID(HttpContext.Request),
                                MembershipAuth.GetCurrentUserOrganizationID(HttpContext.Request),
                                LoggingEventType.Error, "PERMISSION_ERROR");
                    break;
                default:
                    break;
            }

            return View((object)message);
        }

        #endregion
    }
}