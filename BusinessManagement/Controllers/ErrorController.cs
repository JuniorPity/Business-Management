using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessManagement.Controllers
{
    public class ErrorController : Controller
    {
        // Error Constants
        #region Constants

        private const string PROFILE_ERROR = "We can't find the profile that you requested. It is currently unavailable, or does not exist. Please try again. \n ERROR: PROFILE_ERROR";
        private const string PROFILE_EDIT_ERROR = "There was an error processing your edit request. Please try again. \n ERROR: PROFILE_EDIT_ERROR";

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
                case 1:
                    message = PROFILE_ERROR;
                    break;
                case 2:
                    message = PROFILE_EDIT_ERROR;
                    break;
                default:
                    break;
            }

            return View((object)message);
        }

        #endregion
    }
}