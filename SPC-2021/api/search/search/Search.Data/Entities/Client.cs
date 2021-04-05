using System;
using System.Collections.Generic;

#nullable disable

namespace Search.Data.Entities
{
    public partial class Client
    {
        public Client()
        {
            ChatMessageMessageReceiverClients = new HashSet<ChatMessage>();
            ChatMessageMessageSenderClients = new HashSet<ChatMessage>();
            ClientRelationXrefRecipientClients = new HashSet<ClientRelationXref>();
            ClientRelationXrefSenderClients = new HashSet<ClientRelationXref>();
            ClientTagXrefs = new HashSet<ClientTagXref>();
            Notes = new HashSet<Note>();
            SurveyResponses = new HashSet<SurveyResponse>();
        }

        public Guid ClientId { get; set; }
        public Guid ClientStatusId { get; set; }
        public string ClientName { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsAdmin { get; set; }
        public bool HasAccount { get; set; }

        public virtual ClientStatus ClientStatus { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageMessageReceiverClients { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageMessageSenderClients { get; set; }
        public virtual ICollection<ClientRelationXref> ClientRelationXrefRecipientClients { get; set; }
        public virtual ICollection<ClientRelationXref> ClientRelationXrefSenderClients { get; set; }
        public virtual ICollection<ClientTagXref> ClientTagXrefs { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<SurveyResponse> SurveyResponses { get; set; }
    }
}
