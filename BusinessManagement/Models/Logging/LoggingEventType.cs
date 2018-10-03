using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessManagement.Models.Logging
{
    public enum LoggingEventType
    {
        // Event type that the user logged in successfully
        UserLogin = 1,

        // Event type that the user logged out successfully
        UserLogout,

        // Event type that the user was successfully created
        UserCreated,

        // Event type that the user was succesfully deleted
        UserDeleted,

        // Event type that the user successfully edited their profile
        ProfileEdited,

        // Event type that an organization was created
        OrganizationCreated,

        // Event type that an invite was sent out
        InviteSent,

        // Even type that there was an error
        Error
    }
}