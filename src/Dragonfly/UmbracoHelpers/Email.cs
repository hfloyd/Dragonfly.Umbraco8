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
    using System.Net.Mail;
    using System.Text;
    using System.Web;
    using HtmlAgilityPack;
    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Composing;

    /// <summary>
    /// EmailMessage Helpers
    /// </summary>
    public static class Email
    {
        //TODO: Need to figure out Dependency Inject here.... 
        private const string ThisClassName = "Dragonfly.UmbracoHelpers.Email";

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
        /// <param name="NodeId">
        /// The node id of the EmailMessage
        /// </param>
        /// <returns>
        /// The <see cref="MailVariables"/>.
        /// </returns>
        public static MailVariables GetMailVariables(int NodeId)
        {
            var umbContentService = Current.Services.ContentService;
            var emf = umbContentService.GetById(NodeId);

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
                var msg = string.Format("Error creating MailVariables.", ex.Message);
                Current.Logger.Error<MailVariables>(ex, msg);
            }
            return mailvars;
        }

        /// <summary>
        /// Attempts to send an email with mail variable package passed-in
        /// </summary>
        /// <param name="Package">
        /// The The <see cref="MailVariables"/> package.
        /// </param>
        /// <returns>
        /// <see cref="bool"/> indicating successful send.
        /// </returns>
        public static bool TrySendMail(MailVariables Package)
        {
            try
            {
                var msg = new System.Net.Mail.MailMessage();
                msg.From = new System.Net.Mail.MailAddress(Package.From, HttpUtility.HtmlEncode(Package.FromName));
                msg.Subject = Package.Subject;
                msg.Body = Package.BodyContent;
                msg.IsBodyHtml = Package.IsHtml;

                msg.To.Add(new System.Net.Mail.MailAddress(HttpUtility.HtmlEncode(Package.To), HttpUtility.HtmlEncode(Package.ToName)));

                var smtp = new System.Net.Mail.SmtpClient { EnableSsl = Package.EnableSsl };
                smtp.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                var msg = string.Concat("TrySendMail: ", string.Format("Error creating or sending email"));
                Current.Logger.Error<MailMessage>(ex, msg);
            }

            return false;
        }

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
        /// <param name="Html"></param>
        /// <returns></returns>
        public static string AddImgAbsolutePath(string Html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Html);

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
