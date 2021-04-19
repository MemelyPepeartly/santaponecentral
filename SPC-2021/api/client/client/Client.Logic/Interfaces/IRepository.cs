using Client.Logic.Objects;
using Client.Logic.Objects.Information_Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic.Interfaces
{
    public interface IRepository
    {
        #region Client
        /// <summary>
        /// Creates a new client by a logic client object
        /// </summary>
        /// <param name="newClient"></param>
        /// <returns></returns>
        Task CreateClient(Objects.Client newClient);
        /// <summary>
        /// Creates a new client relationship for assignments by a senderID, recipientID, and the eventTypeID, and its assignmentStatusID that the assignment relates to
        /// </summary>
        /// <param name="senderClientID"></param>
        /// <param name="recipientClientID"></param>
        /// <param name="eventTypeID"></param>
        /// <returns></returns>
        Task CreateClientRelationByID(Guid senderClientID, Guid recipientClientID, Guid eventTypeID, Guid assignmentStatusID);
        /// <summary>
        /// Gets a client by their ID
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<Logic.Objects.Client> GetClientByIDAsync(Guid clientId);
        /// <summary>
        /// Gets a client by their email
        /// </summary>
        /// <param name="clientEmail"></param>
        /// <returns></returns>
        Task<Logic.Objects.Client> GetClientByEmailAsync(string clientEmail);
        /// <summary>
        /// Gets a very minimized version of the logic client object by ID. This is for quickly getting info such as the address or email.
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        Task<Objects.Client> GetStaticClientObjectByID(Guid clientID);
        /// <summary>
        /// Gets a very minimized version of the logic client object by email. This is for quickly getting info such as the clientID or just address from a client.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Objects.Client> GetStaticClientObjectByEmail(string email);

        #region Minimized Client Data Getters
        /// <summary>
        /// Gets a headquarter object for clients and faster parsing
        /// </summary>
        /// <returns></returns>
        Task<List<HQClient>> GetAllHeadquarterClients();
        /// <summary>
        /// Gets a headquarter client object by ID
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        Task<HQClient> GetHeadquarterClientByID(Guid clientID);
        /// <summary>
        /// Gets a list of minimized client data for speed and parsing and data purposes
        /// </summary>
        /// <returns></returns>
        Task<List<StrippedClient>> GetAllStrippedClientData();
        /// <summary>
        /// Gets a minimized stripped client data object by client ID
        /// </summary>
        /// <returns></returns>
        Task<StrippedClient> GetStrippedClientDataByID(Guid clientID);
        /// <summary>
        /// Gets a list of all client ID's quickly
        /// </summary>
        /// <returns></returns>
        Task<List<BaseClient>> GetAllBasicClientInformation();
        /// <summary>
        /// Gets a basic client type by ID
        /// </summary>
        /// <returns></returns>
        Task<BaseClient> GetBasicClientInformationByID(Guid clientID);
        /// <summary>
        /// Gets a basic client type by email
        /// </summary>
        /// <returns></returns>
        Task<BaseClient> GetBasicClientInformationByEmail(string clientEmail);
        #endregion

        /// <summary>
        /// Updates a client with a logic client object of the target object that reflects what the client should be updated to
        /// </summary>
        /// <param name="targetClient"></param>
        /// <returns></returns>
        Task UpdateClientByIDAsync(Objects.Client targetClient);
        /// <summary>
        /// Updates an assignment's progress to a chosen assignment status ID
        /// </summary>
        /// <param name="assignmentID"></param>
        /// <param name="newAssignmentStatusID"></param>
        /// <returns></returns>
        Task UpdateAssignmentProgressStatusByID(Guid assignmentID, Guid newAssignmentStatusID);
        /// <summary>
        /// Deletes a client by their ID along with any data about them. This includes chat histories, relationships, and answers
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        Task DeleteClientByIDAsync(Guid clientID);
        /// <summary>
        /// Deletes a reciever from the client by the client's ID, and the IDs of the recipient and event in question
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="recipientID"></param>
        /// <param name="eventID"></param>
        /// <returns></returns>
        Task DeleteRecieverXref(Guid clientID, Guid recipientID, Guid eventID);
        #endregion

        #region Informational
        /// <summary>
        /// Gets a client informational container that contains notes, responses, senders, and recievers
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        Task<InfoContainer> getClientInfoContainerByIDAsync(Guid clientID);
        /// <summary>
        /// Gets a list of assignments for a client by their ID with additional information that a RelationshipMeta provides
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        Task<List<RelationshipMeta>> getClientAssignmentsInfoByIDAsync(Guid clientID);
        /// <summary>
        /// Gets a specific relationship for a client by xrefID
        /// </summary>
        /// <param name="xrefID"></param>
        /// <returns></returns>
        Task<RelationshipMeta> getAssignmentRelationshipMetaByIDAsync(Guid xrefID);
        /// <summary>
        /// Gets a list of assignment xref ID's for a client by ID. This is a minimized data call for speed purposes
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        Task<List<Guid>> getClientAssignmentXrefIDsByIDAsync(Guid clientID);
        #endregion

        #region Utility
        /// <summary>
        /// Saves changes of any CRUD operations in the queue
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
        #endregion

    }
}