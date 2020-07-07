using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Santa.Api.Models.Auth0_Response_Models;
using Santa.Api.Models.Mailbag_Models;
using Santa.Logic.Objects;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
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
            SendGridClient client = new SendGridClient(getKey().key);
            EmailAddress from = new EmailAddress(appEmail, "SantaPone Central");
            string subject = "SantaPone Central Login Information";
            EmailAddress to = new EmailAddress(recipient.email, recipient.nickname);
            string plainTextContent = "Agent, it's time to bring the cheer, and you've been approved for the cause! Follow the link here to set your password: " + ticket.ticket +"\nOnce you have it set, login at " + url;
            string htmlContent = @$"
                <html>
                <head>
                <style type='text / css'>
                </style>
                </head>

                <body>
                    <div style='width: 100%; text-align: center;'>
                        <img src='https://derpicdn.net/img/2020/6/10/2370933/large.png' alt='TotallyNotAShark' style='margin-left: auto; margin-right: auto; width: 200px;'>
                        <p>Agent, it's time to bring the cheer, and you've been approved for the cause! Follow the link here to set your password: <a href='{ticket.ticket}'>Password Reset</a></p>
                        <br>
                        <p>Once you've done that, log into your accout at <a href='{url}'>SantaPone Central</a></p>
                    </div>
                </body>

                </html>";

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task sendChatNotificationEmail(Logic.Objects.Client recipient, Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging.Message message)
        {
            throw new NotImplementedException();
        }

        public Task sendDeniedEmail(Client recipient)
        {
            throw new NotImplementedException();
        }
    }
}
