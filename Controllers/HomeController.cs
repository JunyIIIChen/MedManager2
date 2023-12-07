using MedManager.Models;
using MedManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        //public ActionResult Send_Email()
        //{
        //    SendEmailViewModel sendEmailViewModel = new SendEmailViewModel();
        //    sendEmailViewModel.ToEmail = "123@163.com";
        //    sendEmailViewModel.Contents = "This is content";
        //    return View(sendEmailViewModel);
        //}

        //[HttpPost]
        //public ActionResult Send_Email(SendEmailViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            String toEmail = model.ToEmail;
        //            String subject = model.Subject;
        //            String contents = model.Contents;
        //            HttpPostedFileBase attachment = model.Attachment;

        //            EmailSender es = new EmailSender();
        //            es.Send(toEmail, subject, contents, attachment);

        //            ViewBag.Result = "Email has been send.";

        //            ModelState.Clear();

        //            return View(new SendEmailViewModel());
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }

        //    return View();
        //}

        public ActionResult Send_Email()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        // Usually it is enabled by default
        //Preventing Cross-Site Scripting (XSS):
        //Preventing user input from being interpreted as a malicious script, usually by encoding the data entered by the user.
        public ActionResult Send_Email(SendEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string toEmails = model.ToEmail;
                    string[] toEmailList = toEmails.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    ViewBag.ToEmailList = toEmailList;

                    string subject = model.Subject;
                    string contents = model.Contents;

                    EmailSender es = new EmailSender();

                    foreach (var toEmail in toEmailList)
                    {
                        if (model.Attachment != null && model.Attachment.ContentLength > 0)
                        {
                            // check the attachment
                            byte[] fileData = new byte[model.Attachment.InputStream.Length];
                            model.Attachment.InputStream.Read(fileData, 0, model.Attachment.ContentLength);

                            // send email with attachment
                            es.SendWithAttachment(toEmail.Trim(), subject, contents, model.Attachment.FileName, fileData);
                        }
                        else
                        {
                            // send email
                            es.Send(toEmail.Trim(), subject, contents);
                        }
                    }

                    ViewBag.Result = "Email(s) have been sent.";

                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch (Exception ex)
                {
                    ViewBag.Result = "Error: " + ex.Message;
                }
            }

            return View();
        }
    }
}