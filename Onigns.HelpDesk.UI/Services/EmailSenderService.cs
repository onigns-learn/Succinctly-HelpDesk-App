﻿using SendGrid.Helpers.Mail;
using SendGrid;

namespace Onigns.HelpDesk.UI.Services
{
    public class EmailSenderService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        public EmailSenderService(
        IConfiguration Configuration,
        IHttpContextAccessor HttpContextAccessor)
        {
            configuration = Configuration;
            httpContextAccessor = HttpContextAccessor;
        }
        public async Task SendEmail(string EmailType, string EmailAddress, string TicketGuid)
        {
            try
            {
                // Email settings.
                SendGridMessage msg = new SendGridMessage();
                var apiKey = configuration["SENDGRID_APIKEY"];
                var senderEmail = configuration["SenderEmail"];
                var client = new SendGridClient(apiKey);
                var FromEmail = new EmailAddress(senderEmail, senderEmail);
                // Format email contents.
                string strPlainTextContent = $"{EmailType}: {GetHelpDeskTicketUrl(TicketGuid)}";
                string strHtmlContent = $"<b>{EmailType}:</b> ";
                strHtmlContent += $"<a href='{GetHelpDeskTicketUrl(TicketGuid)}'>";
                strHtmlContent += $"{GetHelpDeskTicketUrl(TicketGuid)}</a>";
                if (EmailType == "Help Desk Ticket Created")
                {
                    msg = new SendGridMessage()
                    {
                        From = FromEmail,
                        Subject = EmailType,
                        PlainTextContent = strPlainTextContent,
                        HtmlContent = strHtmlContent
                    };
                    // Created email always goes to administrator.
                    // Send to senderEmail configured in appsettings.json
                    msg.AddTo(new EmailAddress(senderEmail, EmailType));
                }
                if (EmailType == "Help Desk Ticket Updated")
                {
                    msg = new SendGridMessage()
                    {
                        From = FromEmail,
                        Subject = EmailType,
                        PlainTextContent = strPlainTextContent,
                        HtmlContent = strHtmlContent
                    };
                    // Updated emails go to administrator or ticket creator.
                    // Send to EmailAddress passed to method.
                    msg.AddTo(new EmailAddress(EmailAddress, EmailType));
                }
                var response = await client.SendEmailAsync(msg);
            }
            catch
            {
                // Could not send email.
                // Perhaps SENDGRID_APIKEY not set in
                // appsettings.json
            }
        }
        // Utility
        #region public string GetHelpDeskTicketUrl(string TicketGuid)
        public string GetHelpDeskTicketUrl(string TicketGuid)
        {
            var request = httpContextAccessor.HttpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            return $@"{request.Scheme}://{host}{pathBase}/emailticketedit/{TicketGuid}";
        }
        #endregion
    }
}
