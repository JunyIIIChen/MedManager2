using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MedManager.Utils
{

    public class EmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "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";

        //public void Send(String toEmail, String subject, String contents, HttpPostedFileBase postedFile)
        //{
        //    var client = new SendGridClient(API_KEY);
        //    var from = new EmailAddress("cjy799255537@gmail.com", "FIT5032 Example Email User");
        //    var to = new EmailAddress(toEmail, "");
        //    var plainTextContent = contents;
        //    var htmlContent = "<p>" + contents + "</p>";
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    if (postedFile != null)
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            // 1. convert attach file to inputStream
        //            postedFile.InputStream.CopyTo(memoryStream);
        //            // 2. convert inputstream to byte array
        //            byte[] bytes = memoryStream.ToArray();
        //            Attachment attachment = new Attachment();
        //            // 3. convert to base64 type(String)
        //            attachment.Content = Convert.ToBase64String(bytes);
        //            attachment.Filename = postedFile.FileName;
        //            // 4. add this attachment to email message
        //            msg.AddAttachment(attachment.Filename, attachment.Content);
        //        }
        //    }

        public void Send(String toEmail, String subject, String contents)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("cjy799255537@gmail.com", "FIT5032 Example Email User");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        public void SendWithAttachment(string toEmail, string subject, string contents, string attachmentFileName, byte[] attachmentData)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("cjy799255537@gmail.com", "FIT5032 Example Email User");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var attachment = new Attachment
            {
                Content = Convert.ToBase64String(attachmentData),
                Filename = attachmentFileName
            };
            msg.AddAttachment(attachment);

            var response = client.SendEmailAsync(msg);
        }


    }
}
