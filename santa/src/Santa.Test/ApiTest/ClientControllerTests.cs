using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Santa.Test.ApiTest
{
    public class ClientControllerTests
    {
        [Fact]
        public async Task GetClientByIDAsync()
        {
            SantasLittleHelper helper = new SantasLittleHelper();
            Guid clientId = helper.Clients[0].clientID;

            helper.Repository
                .Setup(x => x.GetClientByID(It.IsAny<Guid>()))
                .Returns(Task.Run(() => helper.Clients.Where(c => c.clientID == clientId).FirstOrDefault()));

            Assert.NotNull(await helper.ClientController.GetClientByIDAsync(clientId));
        }
    }
}
