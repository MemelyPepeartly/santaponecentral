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
            List<Guid> clientGUIDs = helper.Clients.Select(c => c.clientID).ToList();
            helper.Repository.Setup(x => x.GetAllClients()).Returns(helper.Clients);

            // Act
            var clientsList = helper.ClientController.GetAllClients();
            var okObjectResult = Assert.IsAssignableFrom<OkObjectResult>(clientsList.Result);
            List<Client> testClients = Assert.IsAssignableFrom<List<Client>>(okObjectResult.Value);

            // Assert
            Assert.Equal(testClients.ToArray().Length, clientGUIDs.ToArray().Length);
            Assert.NotNull(testClients);

            foreach(Client tc in testClients)
            {
                if(testClients.Contains(helper.Clients.First(c => c.clientID == tc.clientID)))
                {
                    Client comparisonClient = helper.Clients.First(c => c.clientID == tc.clientID);

                    Assert.Equal(comparisonClient.clientID, tc.clientID);
                    Assert.Equal(comparisonClient.address, tc.address);
                    Assert.Equal(comparisonClient.clientName, tc.clientName);
                    Assert.Equal(comparisonClient.email, tc.email);
                    Assert.Equal(comparisonClient.nickname, tc.nickname);
                    Assert.Equal(comparisonClient.recipients, tc.recipients);
                    Assert.Equal(comparisonClient.senders, tc.senders);
                }
                else
                {
                    throw new Exception();
                }
            }
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