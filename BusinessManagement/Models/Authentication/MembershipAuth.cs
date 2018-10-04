using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using BusinessManagement.Models;

namespace BusinessManagement.Models.Authentication
{
    public class MembershipAuth
    {
        private static BusinessDataEntities db = new BusinessDataEntities();

        /*
        * Function: GetCurrentUser((HttpRequestBase request)
        * Purpose: Return the user name of the current logged in user
        * Author: Jordan Pitner 10/3/2018
        */
        public static string GetCurrentUser(HttpRequestBase request)
        {
            string cookieName = FormsAuthentication.FormsCookieName; 

            HttpCookie authCookie = request.Cookies[cookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            return ticket.Name;
        }

        /*
        * Function: IsAdmin((HttpRequestBase request)
        * Purpose: Return the user's DB id
        * Author: Jordan Pitner 10/3/2018
        */
        public static int GetCurrentUserID(HttpRequestBase request)
        {
            string cookieName = FormsAuthentication.FormsCookieName;

            HttpCookie authCookie = request.Cookies[cookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            return db.Users.SingleOrDefault(u => u.Email == ticket.Name).Id;
        }

        /*
        * Function: GetCurrentUserOrganizationID((HttpRequestBase request)
        * Purpose: Return the user's registered org ID
        * Author: Jordan Pitner 10/3/2018
        */
        public static int GetCurrentUserOrganizationID(HttpRequestBase request)
        {
            string cookieName = FormsAuthentication.FormsCookieName;

            HttpCookie authCookie = request.Cookies[cookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            return db.Users.SingleOrDefault(u => u.Email == ticket.Name).OrganizationID;
        }

        /*
        * Function: IsAdmin((HttpRequestBase request)
        * Purpose: Return if the user is an admin
        * Author: Jordan Pitner 10/3/2018
        */
        public static bool IsAdmin(HttpRequestBase request)
        {
            string cookieName = FormsAuthentication.FormsCookieName;

            HttpCookie authCookie = request.Cookies[cookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            User user = db.Users.SingleOrDefault(u => u.Email == ticket.Name);

            if(user != null && user.Role == "Administrator")
            {
                return true;
            }

            return false;
        }

        /*
        * Function: IsSiteAdmin((HttpRequestBase request)
        * Purpose: Return if the user is an site admin
        * Author: Jordan Pitner 10/3/2018
        */
        public static bool IsSiteAdmin(HttpRequestBase request)
        {
            string cookieName = FormsAuthentication.FormsCookieName;

            HttpCookie authCookie = request.Cookies[cookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            User user = db.Users.SingleOrDefault(u => u.Email == ticket.Name);

            if (user != null && user.Role == "System Administrator")
            {
                return true;
            }

            return false;
        }
    }
}