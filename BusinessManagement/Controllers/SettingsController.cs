using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessManagement.Models;
using BusinessManagement.Models.Authentication;
using System.Drawing;

namespace BusinessManagement.Controllers
{
    public class SettingsController : Controller
    {
        BusinessDataEntities db = new BusinessDataEntities();

        // GET: Settings
        [Authorize]
        public ActionResult UserSettings()
        {
            return View();
        }

        // Profile Actions
        #region Profile Actions


        /*
        * Function: EditProfile()
        * Purpose: Update user information
        * Author: Jordan Pitner 9/27/2018
        */
        [Authorize]
        [HttpGet]
        public ActionResult EditProfile()
        {
            string userName = MembershipAuth.GetCurrentUser(HttpContext.Request);
            int userID = db.Users.FirstOrDefault(u => u.Email == userName).Id;

            if (!(userID > 0))
            {
                return RedirectToAction("Login", "Home", null);
            }

            // Grab the user information for display
            User user = db.Users.FirstOrDefault(u => u.Id == userID);

            return View(user);
        }


        /*
        * Function: EditTimeEntry(User user)
        * Purpose: Post event to push the data to the database
        * Author: Jordan Pitner 9/27/2018
        */
        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(User user, HttpPostedFileBase profilePicture)
        {
            if(ModelState.IsValid)
            {
                // Validation that some other user isn't trying to access information
                string userName = MembershipAuth.GetCurrentUser(HttpContext.Request);
                User accessUser = db.Users.FirstOrDefault(u => u.Email == userName);

                ImageProcessor processor = new ImageProcessor();

                // If so, present error screen
                if (accessUser.Id != user.Id)
                {
                    return RedirectToAction("Error", "Error", new { error = 2 });
                }

                // Imaging processing for profile pictures (new or updated)
                if (profilePicture != null)
                {
                    UserImage image = db.UserImages.FirstOrDefault(i => i.UserID == accessUser.Id);
                    byte[] bytes = new byte[profilePicture.ContentLength];

                    // Process profile picture coming in
                    if (image == null)
                    {
                        UserImage newImg = new UserImage();
                        newImg.Binary = bytes;
                        profilePicture.InputStream.Read(bytes, 0, profilePicture.ContentLength);
                        newImg.UserID = accessUser.Id;

                        // Commit new image
                        db.UserImages.Add(newImg);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (!bytes.SequenceEqual(image.Binary))
                        {
                            image.Binary = bytes;
                            profilePicture.InputStream.Read(bytes, 0, profilePicture.ContentLength);
                        }
                    }
                }

                // Update the values from edit (some repeats may exist)
                accessUser.FirstName = user.FirstName.Trim();
                accessUser.LastName = user.LastName.Trim();
                accessUser.Email = user.Email.Trim();
                accessUser.AboutMe = user.AboutMe.Trim();
                accessUser.Skills = user.Skills.Trim();
                accessUser.Phone = user.Phone.Trim().Replace("-", "").Replace("(", "").Replace(")", "");


                // Commit changes
                db.SaveChanges();

                return View(accessUser);
            }

            return RedirectToAction("Error", "Error", new { error = 2 });
        }

        #endregion

        // Invite Actions
        #region Invite Actions

        /*
        * Function: Invites()
        * Purpose: Show the invite view 
        * Author: Jordan Pitner 10/3/2018
        */
        [Authorize]
        public ActionResult Invites()
        {
            // Only let admins access this view
            if(MembershipAuth.IsAdmin(HttpContext.Request, db))
            {
                // Get credentials to get proper invites
                string userName = MembershipAuth.GetCurrentUser(HttpContext.Request);
                int orgID = db.Users.SingleOrDefault(u => u.Email == userName).OrganizationID;

                // Return list of invites for viewing
                List<InviteCode> inviteCodes = db.InviteCodes.Where(i => i.OrganizationID == orgID).ToList();

                return View(inviteCodes);
            }

            return RedirectToAction("Error", "Error", new { error = 3 });
        }

        /*
        * Function: GenerateInvite()
        * Purpose: Generate a new invite and post it to the db
        * Author: Jordan Pitner 10/3/2018
        */
        [HttpPost]
        public ActionResult GenerateInvite(string email)
        {
            // Only allow admins to submit a new code
            if (MembershipAuth.IsAdmin(HttpContext.Request, db))
            {           
                // Generate the invite code     
                Random rand = new Random();
                string code = CreateInviteCode(rand.Next(58, 65));

                // Get information from database to populate the invite
                string userName = MembershipAuth.GetCurrentUser(HttpContext.Request);
                int orgID = db.Users.SingleOrDefault(u => u.Email == userName).OrganizationID;
                int createdBy = db.Users.SingleOrDefault(u => u.Email == userName).Id;

                // Create new invite code
                InviteCode invite = new InviteCode
                {
                    Code = code,
                    DateCreated = DateTime.Now,
                    OrganizationID = orgID,
                    IsExpired = false,
                    CreatedBy = createdBy,
                    SentTo = email
                };

                // Add and commit to database
                db.InviteCodes.Add(invite);
                db.SaveChanges();
            }

            // Return error screen
            return RedirectToAction("Error", "Error", new { error = 3 });
        }

        #endregion

        // Private Methods
        #region Private Methods

        /*
        * Function: CreateInviteCode(int length)
        * Purpose: Generate a random string of chars to act as the invite code
        * Author: Jordan Pitner 10/3/2018
        */
        private string CreateInviteCode(int length)
        {
            Random random = new Random();

            // Characters to choose from
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";

            // Generate string from random generator and length
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
    }
}