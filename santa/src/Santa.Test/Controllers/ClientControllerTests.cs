using Xunit;
using Santa.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Santa.Test;
using System.Threading.Tasks;
using System.Linq;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Santa.Logic.Objects;

namespace Santa.Api.Controllers.Tests
{
    public class ClientControllerTests
    {
        [Fact()]
        public void ClientControllerTest()
        {
            throw new NotImplementedException();
        }

        [Fact()]
        public void GetAllClientsTest()
        {
            // Arrange
            SantasLittleHelper helper = new SantasLittleHelper();

            List<Guid> clientGUIDs = new List<Guid>();
            foreach(Client c in helper.Clients)
            {
                clientGUIDs.Add(c.clientID);
            }

            
            List<Guid> clientIDs = helper.Clients.Select(c => c.clientID).ToList();

            helper.Repository.Setup(x => x.GetAllClients()).Returns(helper.Clients);

            var clientsList = helper.ClientController.GetAllClients();

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(clientsList.Result);
            var testClients = Assert.IsAssignableFrom<List<Client>>(okObjectResult.Value);

            Assert.Equal(testClients.ToArray().Length, clientGUIDs.ToArray().Length);

            Assert.NotNull(testClients.Select(c => c.clientStatusID));
            Assert.NotNull(testClients.Select(c => c.clientName));
            Assert.NotNull(testClients.Select(c => c.address));
            Assert.NotNull(testClients.Select(c => c.clientStatusID));
            Assert.NotNull(testClients.Select(c => c.email));
            Assert.NotNull(testClients.Select(c => c.nickname));
            Assert.NotNull(testClients.Select(c => c.recipients));
            Assert.NotNull(testClients.Select(c => c.senders));


            /*
            foreach (Client c in testClients)
            {
                Assert.Equal(c.clientID.ToString(), clientGUIDs.Where(g => g.Equals(c.clientID)).ToString());
            }
            */
        }

        [Fact()]
        public async System.Threading.Tasks.Task GetClientByIDAsyncTestAsync()
        {
            // Arrange
            SantasLittleHelper helper = new SantasLittleHelper();
            Guid clientId = helper.Clients[0].clientID;

            helper.Repository.Setup(x => x.GetClientByID(clientId))
                .ReturnsAsync(helper.Clients.First(c => c.clientID == clientId));
            // Act
            var testClient = await helper.ClientController.GetClientByIDAsync(clientId);

            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(testClient.Result);
            var client = Assert.IsAssignableFrom<Client>(okObjectResult.Value);

            // Assert

            //This is the assert. I need to get the guid to equal the one in test client
            Assert.Equal(clientId.ToString(), client.clientID.ToString());

        }

        [Fact()]
        public void PostAsyncTest()
        {
            throw new NotImplementedException();
        }

        [Fact()]
        public void PutTest()
        {
            throw new NotImplementedException();
        }

        [Fact()]
        public void DeleteTest()
        {
            throw new NotImplementedException();
        }
    }
}