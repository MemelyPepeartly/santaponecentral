using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Santa.Api.Models.Auth0_Response_Models;
using Santa.Api.Models.Mailbag_Models;
using Santa.Logic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.SendGrid
{
    public interface IMailbag
    {
        MailbagKeyModel getKey();
        Task sendSignedUpAndAwaitingEmail(Client emailRecipientClient);
        Task sendPasswordResetEmail(string recipientEmail, string recipientNickname, Auth0TicketResponse ticket, bool isNewUser);
        Task sendApprovedForEventWithNoAccountEmail(Client emailRecipientClient);
        Task sendChatNotificationEmail(Client recipient, Event eventType);
        Task sendDeniedEmail(Client recipient);
        Task sendAssignedRecipientEmail(Client recipient, Event eventType);
        Task sendCompletedEmail(Client recipient);
        Task sendReelistedEmail(Client recipient);
        Task sendUndeniedEmail(Client recipient);
    }
}
