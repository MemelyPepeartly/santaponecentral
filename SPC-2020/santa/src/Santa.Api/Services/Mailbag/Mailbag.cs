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
        private string url = string.Empty;
        private const string appEmail = "mailbag@santaponecentral.com";

        private const string emailStart = @"
                <html>
                <head>
                <style type='text / css'>
                </style>
                </head>

                <body>
                    <div style='width: 100%; text-align: center;'>
                        <img src='https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/47316544-c98a-4954-adaf-0f3847d88a4b/de4x7vz-4c57d19e-fc3a-41cb-8f6e-1263a57f1b87.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOiIsImlzcyI6InVybjphcHA6Iiwib2JqIjpbW3sicGF0aCI6IlwvZlwvNDczMTY1NDQtYzk4YS00OTU0LWFkYWYtMGYzODQ3ZDg4YTRiXC9kZTR4N3Z6LTRjNTdkMTllLWZjM2EtNDFjYi04ZjZlLTEyNjNhNTdmMWI4Ny5wbmcifV1dLCJhdWQiOlsidXJuOnNlcnZpY2U6ZmlsZS5kb3dubG9hZCJdfQ.lop-9l47EbixLniXlXc2snyoLxOqIXmZy4eqYi-0Ka4' alt='SecretSantaBadgeOfAuthenticity' style='margin-left: auto; margin-right: auto; width: 300px;'>";
        private const string emailEnd = @"
                    </div>
                </body>

                </html>";

        public Mailbag(IConfiguration configRoot)
        {
            ConfigRoot = (IConfigurationRoot)configRoot;
            url = ConfigRoot["SendgridAPI:emailURL"];
        }
        public MailbagKeyModel getKey()
        {
            return new MailbagKeyModel()
            {
                key = ConfigRoot["SendgridAPI:SendgridAPIKey"]
            };
        }
        public async Task sendSignedUpAndAwaitingEmail(Client recipient)
        {
            SendGridClient client = new SendGridClient(getKey().key);
            EmailAddress from = new EmailAddress(appEmail, "SantaPone Central");
            string subject = "SantaPone Central Signup In Progress";
            EmailAddress to = new EmailAddress(recipient.email, recipient.nickname);
            string plainTextContent = "Your application submission is in! All there is to do now is wait, and the organizers will go through your application from here. " +
                "Keep an eye on your email for your confirmation, and be sure to check your spam folder in case any get caught up in there once it comes through!";
            string htmlContent = emailStart +
                @$"
                    <p>Your application submission is in! All there is to do now is wait, and the organizers will go through your application from here.</p>
                    <br>
                    <p>Keep an eye on your email for your confirmation, and be sure to check your spam folder in case any get caught up in there once it comes through! If you have any urgent questions prior to being approved, as always, be sure to 
                    reach out to Santapone at mlpsantapone@gmail.com, or Cardslut at thecardslut@gmail.com, or just ask on the thread.</p>
                    <br>
                    <p>Over and Out</p>
                    <p><strong>Pretty Online Notification Equines</strong></p>"
                + emailEnd;

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        public async Task sendPasswordResetEmail(string recipientEmail, string recipientNickname, Auth0TicketResponse ticket, bool isNewUser)
        {
            SendGridClient client = new SendGridClient(getKey().key);
            EmailAddress from = new EmailAddress(appEmail, "SantaPone Central");
            string subject = "SantaPone Central Login Information";
            EmailAddress to = new EmailAddress(recipientEmail, recipientNickname);
            string plainTextContent = string.Empty;
            string htmlContent = string.Empty;
            if (isNewUser)
            {
                plainTextContent = "Agent, it's time to bring the cheer, and you've been approved for the cause! Follow the link here to set your password: " + ticket.ticket + "\nOnce you have it set, login at " + url;
                htmlContent = emailStart +
                    @$"
                    <p>Agent, it's time to bring the cheer, and you've been approved for the cause! Follow the link here to set your password: <a href='{ticket.ticket}'>Set your password</a></p>
                    <br>
                    <p>Once you've done that, log into your account at <a href='{url}'>SantaPone Central</a></p>
                    <br>
                    <p><strong>Santa Authorization and Networking Telecommunication Administration</strong></p>"

                    + emailEnd;
            }
            else
            {
                plainTextContent = "Agent, A change to your username, or a request to change your password has been made. Use this link to reset your password: " + ticket.ticket + "\nOnce you have it set, login at " + url + 
                    "If you did not make this request, reach out to the admins in your General Correspondence tab on your profile, or mlpsantapone@gmail.com!";
                htmlContent = emailStart +
                    @$"
                    <p>Agent, A change to your username, or a request to change your password has been made. Use this link to reset your password: <a href='{ticket.ticket}'>Password Reset</a></p>
                    <br>
                    <p>Once you've done that, log into your account at <a href='{url}'>SantaPone Central</a></p>
                    <p>If you did not make this request, reach out to the admins in your General Correspondence tab on your profile, or mlpsantapone@gmail.com!</p>
                    <br>
                    <p><strong>Santa Authorization and Networking Telecommunication Administration</strong></p>"
                    + emailEnd;
            }
            

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        public async Task sendApprovedForEventWithNoAccountEmail(Client emailRecipientClient)
        {
            SendGridClient client = new SendGridClient(getKey().key);
            EmailAddress from = new EmailAddress(appEmail, "SantaPone Central");
            string subject = "SantaPone Central Approval Status";
            EmailAddress to = new EmailAddress(emailRecipientClient.email, emailRecipientClient.nickname);
            string plainTextContent = "HEY ANON! You were approved to join the Secret Santa Event! " +
                $"Your holiday nickname given to you is {emailRecipientClient.nickname} and you will receive an email as soon as you are given your assignments! " +
                "A login for you has not been created per request, so be sure to continue to email Santapone at mlpsantapone@gmail.com, or Cardslut at thecardslut@gmail.com about any updates to your assignment(s). If you decide later on you would like a login to help see and organize your assignments, " +
                "just let the shark, or any other event organizers know!";
            string htmlContent = emailStart +
                @$"
                    <p>HEY ANON! You were approved to join the Secret Santa Event! 
                       Your holiday nickname given to you is {emailRecipientClient.nickname}, and you will receive an email as soon as you are given your assignments!
                    </p>
                    <br>
                    <p>A login for you has not been created per request, so be sure to continue to email Santapone at mlpsantapone@gmail.com, or Cardslut at thecardslut@gmail.com about any updates to your assignment(s). 
                       If you decide later on you would like a login to help see and organize your assignments, just let the shark, or any other event organizers know!
                    </p>
                    <br>
                    <p>Over and Out</p>
                    <p><strong>Pretty Online Notification Equines</strong></p>"
                + emailEnd;

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task sendChatNotificationEmail(Logic.Objects.Client recipient, Logic.Objects.Event eventType)
        {
            SendGridClient client = new SendGridClient(getKey().key);
            EmailAddress from = new EmailAddress(appEmail, "SantaPone Central");
            string subject = "SantaPone Central Notification";
            EmailAddress to = new EmailAddress(recipient.email, recipient.nickname);
            string plainTextContent = "";
            string htmlContent = "";
            if (!string.IsNullOrWhiteSpace(eventType.eventDescription))
            {
                plainTextContent = $"You have recieved message for an assignment in the {eventType.eventDescription}, agent! Log into {url} to view it!";
                htmlContent = emailStart +
                    @$"
                    <p>You have recieved message for an assignment in the {eventType.eventDescription}, agent! Log into <a href='{url}'>SantaPone Central</a> to view it!</p>
                    <br>
                    <p>Over and Out,</p>
                    <p><strong>Pretty Online Notification Equines</strong></p>"
                    + emailEnd;
            }
            else
            {
                plainTextContent = $"You have recieved a message in your general correspondence, agent! Log into {url} to view it!";
                htmlContent = emailStart +
                    @$"
                    <p>You have recieved a message in your general correspondence, agent! Log into <a href='{url}'>SantaPone Central</a> to view it!</p>
                    <br>
                    <p>Over and Out</p>
                    <p><strong>Pretty Online Notification Equines</strong></p>"
                    + emailEnd;
            }

            if(!string.IsNullOrWhiteSpace(plainTextContent) && !string.IsNullOrWhiteSpace(htmlContent))
            {
                SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
            else
            {
                throw new Exception("Email content is invalid for sending chat notification");
            }
        }
        public async Task sendAssignedRecipientEmail(Client recipient, Event eventType)
        {
            SendGridClient client = new SendGridClient(getKey().key);
            EmailAddress from = new EmailAddress(appEmail, "SantaPone Central");
            string subject = "SantaPone Central New Assignment";
            EmailAddress to = new EmailAddress(recipient.email, recipient.nickname);

            string accountText = recipient.hasAccount ? "<p>If you have any questions, feel free to reach out to the admins under your profile's General Correspondence section!</p>" : "<p>If you have any questions or updates, be sure to email Santapone at mlpsantapone@gmail.com for gift assignments, or Cardslut at thecardslut@gmail.com for card assignments!</p>";
            string plainTextContent = $"You have been given your assignment(s) for the {eventType.eventDescription} event! If you have any questions, feel free to reach out to the admins under your profile's General Correspondence section!";
            string htmlContent = emailStart +
                @$"
                    <p>You have been given your assignment(s) for the {eventType.eventDescription} event!</p>
                    <br>
                    {accountText}
                    <br>
                    <p>Over and Out</p>
                    <p><strong>Pretty Online Notification Equines</strong></p>"
                + emailEnd;

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        public async Task sendDeniedEmail(Client recipient)
        {
            SendGridClient client = new SendGridClient(getKey().key);
            EmailAddress from = new EmailAddress(appEmail, "SantaPone Central");
            string subject = "SantaPone Central Status";
            EmailAddress to = new EmailAddress(recipient.email, "Anon");
            string plainTextContent = "Unfortunately, you were not approved for the Secret Santa event. If you feel this is a mistake, or wish to make an appeal, feel free to reach out to mlpsantapone@gmail.com.";
            string htmlContent = emailStart +
                @$"
                    <p>Unfortunately, you were not approved for the Secret Santa event.</p>
                    <br>
                    <p>If you feel this is a mistake, or wish to make an appeal, feel free to reach out to mlpsantapone@gmail.com</p>
                    <br>
                    <p>Over and Out</p>
                    <p><strong>Pretty Online Notification Equines</strong></p>"
                + emailEnd;

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        public async Task sendUndeniedEmail(Client recipient)
        {
            SendGridClient client = new SendGridClient(getKey().key);
            EmailAddress from = new EmailAddress(appEmail, "SantaPone Central");
            string subject = "SantaPone Central Status";
            EmailAddress to = new EmailAddress(recipient.email, "Anon");

            string plainTextContent = "";
            string htmlContent =  "";

            if(recipient.hasAccount)
            {
                plainTextContent = "After consideration, you were approved to join the Secret Santa Event! Check your email, as you should have recieved a second email with instructions to log in, and be sure to double check your spam folder if any are missed. " +
                "If you have any questions, feel free to reach out to the admins under your profile's General Correspondence section!";
                htmlContent = emailStart +
                    @$"
                    <p>After consideration, you were approved to join the Secret Santa Event! Check your email, as you should have recieved a second email with instructions to log in, and be sure to double check your spam folder if any are missed.</p>
                    <br>
                    <p>If you have any questions, feel free to reach out to the admins under your profile's General Correspondence section!</p>
                    <br>
                    <p>Over and Out</p>
                    <p><strong>Pretty Online Notification Equines</strong></p>"
                    + emailEnd;
            }
            else
            {
                plainTextContent = "After consideration, you were approved to join the Secret Santa Event! Check your email for any additional details, and be sure to double check your spam folder if any are missed! " +
                "If you have any questions, feel free to email Santapone at mlpsantapone@gmail.com, or Cardslut at thecardslut@gmail.com.";
                htmlContent = emailStart +
                    @$"
                    <p>After consideration, you were approved to join the Secret Santa Event! Check your email for any additional details, and be sure to double check your spam folder if any are missed! </p>
                    <br>
                    <p>If you have any questions, feel free to email Santapone at mlpsantapone@gmail.com, or Cardslut at thecardslut@gmail.com.</p>
                    <br>
                    <p>Over and Out</p>
                    <p><strong>Pretty Online Notification Equines</strong></p>"
                    + emailEnd;
            }
            

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        public async Task sendReelistedEmail(Client recipient)
        {
            SendGridClient client = new SendGridClient(getKey().key);
            EmailAddress from = new EmailAddress(appEmail, "SantaPone Central");
            string subject = "SantaPone Central Re-Enlistment";
            EmailAddress to = new EmailAddress(recipient.email, "Anon");
            string plainTextContent = "Your tenacity and dedication to the cause is incredible, agent! You have been successfully re-enlisted to the cause! The world could do with more santa's like you. " +
                "Intelligence is working on assigning you new ponies to bring cheer to, so sit tight and await that email! If you have any other questions, feel free to reach out to the admins under your profile's General Correspondence section! Keep on being awesome, agent!";
            string htmlContent = emailStart +
                @$"
                    <p>Your tenacity and dedication to the cause is incredible, agent! You have been successfully re-enlisted to the cause! The world could do with more santa's like you.</p>
                    <br>
                    <p>Intelligence is working on assigning you new ponies to bring cheer to, so sit tight and await that email! If you have any other questions, feel free to reach out to the admins under your profile's General Correspondence section! Keep on being awesome, agent!</p>
                    <br>
                    <p>Over and Out</p>
                    <p><strong>Pretty Online Notification Equines</strong></p>"
                + emailEnd;

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        public async Task sendCompletedEmail(Client recipient)
        {
            SendGridClient client = new SendGridClient(getKey().key);
            EmailAddress from = new EmailAddress(appEmail, "SantaPone Central");
            string subject = "SantaPone Central Completed Confirmation";
            EmailAddress to = new EmailAddress(recipient.email, "Anon");
            string plainTextContent = "Well done, agent! You were called to answer a cry for cheer and answered with due diligence! With your assignments sent, and the world a better place, you are free to take a rest! " +
                "Now of course, if you feel there can be more to do  or have any other questions, feel free to reach out to the admins under your profile's General Correspondence section!";
            string htmlContent = emailStart +
                @$"
                    <p>Well done, agent! You were called to answer the call for cheer, and answered with due diligence! With your assignments sent and the world a better place, you are free to take a rest!</p>
                    <br>
                    <p>Now of course, if you feel there can be more to do or have any other questions, feel free to reach out to the admins under your profile's General Correspondence section!</p>
                    <br>
                    <p>Thanks again for joining the cause, Agent, Over and Out</p>
                    <p><strong>Pretty Online Notification Equines</strong></p>"
                + emailEnd;

            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
