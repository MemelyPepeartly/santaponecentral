using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Santa.Api.Models.Auth0_Response_Models;
using Santa.Api.Models.Mailbag_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Santa.Api.SendGrid
{
    public interface IMailbag
    {
        MailbagKeyModel getKey();
        Task sendPasswordResetEmail(Logic.Objects.Client recipient, Auth0TicketResponse ticket);
        Task sendChatNotificationEmail(Logic.Objects.Client recipient, Message message);
    }
}
