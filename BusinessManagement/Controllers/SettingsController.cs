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
        MembershipAuth membership = new MembershipAuth();

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
            string userName = membership.GetCurrentUser(HttpContext.Request);
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
                string userName = membership.GetCurrentUser(HttpContext.Request);
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
    }
}