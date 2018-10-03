using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

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
    }
}