using Xunit;
using Santa.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Santa.Test;
using System.Threading.Tasks;
using System.Linq;
using Moq;

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
        public void GetTest()
        {
            throw new NotImplementedException();
        }

        [Fact()]
        public async System.Threading.Tasks.Task GetClientByIDAsyncTestAsync()
        {
            SantasLittleHelper helper = new SantasLittleHelper();
            Guid clientId = helper.Clients[0].clientID;

            helper.Repository.Setup(x => x.GetClientByID(clientId))
                .ReturnsAsync(helper.Clients.FirstOrDefault(c => c.clientID == clientId));
            // Act
            var testClient = await helper.ClientController.GetClientByIDAsync(clientId);

            // Assert
            
            //This is the assert. I need to get the guid to equal the one in test client
            //Assert.Equal(clientId.ToString(), testClient.Result.ToString());

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