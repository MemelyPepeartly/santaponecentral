using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Santa.Api.Models.Auth0_Response_Models;
using Santa.Api.Models.Mailbag_Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.SendGrid
{
    public class Mailbag : IMailbag
    {
        private IConfigurationRoot ConfigRoot;
#warning Change address at prod
        private const string url = "http://dev-santaponecentral.azurewebsites.net/";
        private const string appEmail = "mailbag@santaponecentral.com";

        public Mailbag(IConfiguration configRoot)
        {
            ConfigRoot = (IConfigurationRoot)configRoot;
        }
        public MailbagKeyModel getKey()
        {
            return new MailbagKeyModel()
            {
                key = ConfigRoot["SendgridAPI:SendgridAPIKey"]
            };
        }

        public async Task sendPasswordResetEmail(Logic.Objects.Client recipient, Auth0TicketResponse ticket)
        {
            var client = new SendGridClient(getKey().key);
            var from = new EmailAddress(appEmail, "SantaPone Central");
            var subject = "SantaPone Central Login Information";
            var to = new EmailAddress(recipient.email, recipient.nickname);
            var plainTextContent = "Agent, it's time to bring the cheer, and you've been approved for the cause! Follow the link here to set your password: " + ticket.ticket +"\nOnce you have it set, login at " + url;
            var htmlContent = "<span>Agent, it's time to bring the cheer, and you've been approved for the cause! Follow the link here to set your password: <a href='" + ticket.ticket + "'>Password Reset</a></span><br>" +
                "<p>Once you've done that, log into your accout at <a href='"+ url +"'>SantaPone Central</a></p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task sendChatNotificationEmail(Logic.Objects.Client recipient, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
