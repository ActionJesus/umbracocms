using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using umbracocms.Models;
using Umbraco.Web.Mvc;
using Umbraco.Core.Models;

namespace umbracocms.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        public ActionResult Index()
        {
            return PartialView("ContactFormPartial", new ContactModel());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactModel model)
        {
            if (!ModelState.IsValid) { return CurrentUmbracoPage(); }
            using (var smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("Confidential", "Confidential");
                smtp.EnableSsl = true;
                var message = new MailMessage();
                message.To.Add("larskroejmand@gmail.com");
                message.Subject = model.Subject;
                message.From = new MailAddress(model.Email, model.Name);
                message.Body = model.Message;
                //smtp.Send(message);
            }

            var comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Comment");
            comment.SetValue("cName", model.Name);
            comment.SetValue("email", model.Email);
            comment.SetValue("subject", model.Subject);
            comment.SetValue("message", model.Message);

            Services.ContentService.Save(comment);

            TempData["success"] = true;
            return RedirectToCurrentUmbracoPage();
        }
    }
}