using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.EmailTemplater;
using Business.MailMandrill;
using Shared;

namespace Favours4Favours.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        [HttpPost]
        public ActionResult Index(ContactModel model)
        {
            var message = new EmailFactory
            {
                FromEmail = "contactus@favours4favours.com",
                ToEmail = "dave@drpcreative.co.uk",
                Template = "Customer-Contact-Us",
                Subject = "Contact Me",
                Data = new Dictionary<string, string>
                {
                    {"name", model.Name},
                    {"email",  model.Email},
                    {"message", model.Message}
                }
            };

            message.LoadTemplate();

            try
            {
                var result = new MailManagerMandrill().SendAsync(message);

                TempData["Message"] = "Your message has been successfully sent and we will be in touch soon...";

                // Clear all the form fields
                ModelState.Clear();
                model.Name = string.Empty;
                model.Email = string.Empty;
                model.Message = string.Empty;

                //redirect to current page to clear the form
                return Redirect("ThankYou.html");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message + ex.StackTrace;
            }


            return Redirect("ThankYou.html");
        }
    }
}