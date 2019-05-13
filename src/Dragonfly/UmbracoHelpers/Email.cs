// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Email.cs">
//   2015
// </copyright>
// <summary>
//   Email Helpers for website generated emails. Used on multiple pages
//   "Inspired" by Cultiv Contact Form =)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dragonfly.UmbracoHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using HtmlAgilityPack;
    using Umbraco.Core;
    using Umbraco.Core.Logging;

    /// <summary>
    /// EmailMessage Helpers
    /// </summary>
    public static class Email
    {
        private const string ThisClassName = "Dragonfly.Umbraco7Helpers.Email";

        /// <summary>
        /// Data structure for storing mail related items
        /// </summary>
        public class MailVariables
        {
            public MailVariables() { this.IsReady = false; }

            public string BodyContent { get; set; }
            public string Subject { get; set; }
            public string To { get; set; }
            public string ToName { get; set; }
            public string From { get; set; }
            public string FromName { get; set; }
            public string ReplyTo { get; set; }
            public bool EnableSsl { get; set; }
            public bool IsReady { get; set; }
            public bool IsHtml { get; set; }

        }

        /// <summary>
        /// Gets mail variables from EmailMessage document type
        /// </summary>
        /// <param name="nodeId">
        /// The node id of the EmailMessage
        /// </param>
        /// <returns>
        /// The <see cref="MailVariables"/>.
        /// </returns>
        public static MailVariables GetMailVariables(int nodeId)
        {
            var umbContentService = ApplicationContext.Current.Services.ContentService;
            var emf = umbContentService.GetById(nodeId);

            MailVariables mailvars = new MailVariables();
            try
            {
                mailvars.From = emf.GetValue<string>("from");
                mailvars.FromName = emf.GetValue<string>("fromName");
                mailvars.To = emf.GetValue<string>("to");
                mailvars.ToName = emf.GetValue<string>("toName");
                mailvars.Subject = emf.GetValue<string>("subject");
                mailvars.BodyContent = emf.GetValue<string>("content");
                mailvars.IsReady = true;
            }
            catch (Exception ex)
            {
                var Msg = string.Format("Error creating or MailVariables. Exception: {0}", ex.Message);
                LogHelper.Error(typeof(Email), Msg, new Exception(Msg));
            }
            return mailvars;
        }

        /// <summary>
        /// Attempts to send an email with mail variable package passed-in
        /// </summary>
        /// <param name="package">
        /// The The <see cref="MailVariables"/> package.
        /// </param>
        /// <returns>
        /// <see cref="bool"/> indicating successful send.
        /// </returns>
        public static bool TrySendMail(MailVariables package)
        {
            try
            {
                var msg = new System.Net.Mail.MailMessage();
                msg.From = new System.Net.Mail.MailAddress(package.From, HttpUtility.HtmlEncode(package.FromName));
                msg.Subject = package.Subject;
                msg.Body = package.BodyContent;
                msg.IsBodyHtml = package.IsHtml;

                msg.To.Add(new System.Net.Mail.MailAddress(HttpUtility.HtmlEncode(package.To), HttpUtility.HtmlEncode(package.ToName)));

                var smtp = new System.Net.Mail.SmtpClient { EnableSsl = package.EnableSsl };
                smtp.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                var Msg = string.Concat("TrySendMail: ", string.Format("Error creating or sending email, exception: {0}", ex.Message));
                LogHelper.Error(typeof(Email), Msg, new Exception(Msg));
            }

            return false;
        }

        ///// <summary>
        ///// Given the set of replacement values and a list of email fields, construct and send the required emails.
        ///// </summary>
        ///// <param name="emailValues">The replacement values</param>
        ///// <param name="formAliases">The node property aliases, relevant to the current node.</param>
        //public static void ProcessForms(this UmbracoHelper umbraco, Dictionary<string, string> emailValues, IEnumerable<EmailFields> emailFieldsList, EmailType? emailType, bool addFiles = false)
        //{
        //    var streams = new Dictionary<string, MemoryStream>();

        //    if (addFiles)
        //    {
        //        var files = HttpContext.Current.Request.Files;
        //        foreach (string fileKey in files)
        //        {
        //            var file = files[fileKey];

        //            //Only add the file if one has been selected.
        //            if (file.ContentLength > 0)
        //            {
        //                file.InputStream.Position = 0;
        //                var memoryStream = new MemoryStream();
        //                file.InputStream.CopyTo(memoryStream);
        //                streams.Add(file.FileName, memoryStream);
        //            }
        //        }
        //    }

        //    foreach (var emailFields in emailFieldsList)
        //    {
        //        if (emailFields.Send
        //            && !string.IsNullOrWhiteSpace(emailFields.SenderName)
        //            && !string.IsNullOrWhiteSpace(emailFields.SenderEmail)
        //            && !string.IsNullOrWhiteSpace(emailFields.ReceiverEmail)
        //            && !string.IsNullOrWhiteSpace(emailFields.Subject)
        //            )
        //        {
        //            var attachments = new Dictionary<string, MemoryStream>();
        //            foreach (var stream in streams)
        //            {
        //                var memoryStream = new MemoryStream();
        //                stream.Value.Position = 0;
        //                stream.Value.CopyTo(memoryStream);
        //                memoryStream.Position = 0;
        //                attachments.Add(stream.Key, memoryStream);
        //            }

        //            ReplacePlaceholders(emailFields, emailValues);
        //            emailFields.Body = AddImgAbsolutePath(emailFields.Body);
        //            Umbraco.SendEmail(
        //                emailFields.SenderEmail,
        //                emailFields.SenderName,
        //                emailFields.ReceiverEmail,
        //                emailFields.Subject,
        //                emailFields.Body,
        //                emailFields.CcEmail,
        //                emailFields.BccEmail,
        //                emailType: emailType,
        //                addFiles: addFiles,
        //                attachments: attachments
        //                );
        //        }
        //    }
        //}

        /// <summary>
        /// Using a dictionary of replacement keys with their corresponding values,
        /// replace the placeholders in the Template content. 
        /// </summary>
        /// <param name="TemplateContent">The email template content to process.</param>
        /// <param name="PlaceholdersData">The placeholder data Dictionary</param>
        /// <param name="TemplatePattern">The format pattern to indicate placeholders in the template content</param>
        public static string ReplacePlaceholders(string TemplateContent, Dictionary<string, string> PlaceholdersData, string TemplatePattern = "[{0}]", bool EscapeHtml = false)
        {
            StringBuilder templ = new StringBuilder(TemplateContent);

            foreach (var kv in PlaceholdersData)
            {
                var placeholder = string.Format(TemplatePattern, kv.Key);
                var val = kv.Value;

                if (EscapeHtml)
                {
                    val = HttpContext.Current.Server.HtmlEncode(val);
                }

                templ.Replace(placeholder, val);
            }

            return templ.ToString();
        }

        /// <summary>
        /// Add an absolute path to all the img tags in the html of an e-mail.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string AddImgAbsolutePath(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
            var domainUrl = string.Format("{0}://{1}", uri.Scheme, uri.Authority);

            if (doc.DocumentNode.SelectNodes("//img[@src]") != null)
            {
                foreach (HtmlNode img in doc.DocumentNode.SelectNodes("//img[@src]"))
                {
                    HtmlAttribute att = img.Attributes["src"];
                    if (att.Value.StartsWith("/"))
                    {
                        att.Value = domainUrl + att.Value;
                    }
                }
            }

            return doc.DocumentNode.InnerHtml;
        }
    }
}
