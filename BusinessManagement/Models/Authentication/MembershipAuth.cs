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
        public static string GetCurrentUser(HttpRequestBase request)
        {
            string cookieName = FormsAuthentication.FormsCookieName; 

            HttpCookie authCookie = request.Cookies[cookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            return ticket.Name;
        }

        public static bool IsAdmin(HttpRequestBase request, BusinessDataEntities db)
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
    }
}