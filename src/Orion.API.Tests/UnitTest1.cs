using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Orion.DAL.Repository.Interfaces;
using System;
using Xunit;

namespace Orion.API.Tests
{
    public class UnitTest1
    {
        //TestServer server;
        //HttpClient client;

        //[Fact]
        //public ApplicationUserTest()
        //{
        //    var userRepo = new Mock<IUserRepository>();
        //    userRepo.Setup(a => a.GetByEmail)
        //        .Returns(new[] {
        //    new Model.Reseller
        //    {
        //        Id = Guid.NewGuid(),
        //        Code = "R1",
        //        Name = "Reseller 1"
        //    }
        //        }.AsQueryable());

        //    // How to inject mock properly in the lines below?

        //    server = new TestServer(new WebHostBuilder()
        //        .ConfigureServices(a => a.AddAutofac())
        //        .UseStartup<Startup>());

        //    client = server.CreateClient();
        //}
    }
}
