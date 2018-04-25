using System;
using NUnit.Framework;
using EventflitServer.RestfulClient;
using EventflitServer.Tests.RestfulClient.Fakes;

namespace EventflitServer.Tests.RestfulClient
{
    [TestFixture]
    public class When_making_a_request
    {
        [Test]
        public async void then_the_get_request_should_be_made_with_a_valid_resource()
        {
            var factory = new AuthenticatedRequestFactory(Config.AppKey, Config.AppId, Config.AppSecret);
            var request = factory.Build(EventflitMethod.GET, "/channels/newRestClient");

            var client = new EventflitRestClient("http://service.eventflit.com", "eventflit-http-dotnet", Version.Parse("4.0.0"));
            var response = await client.ExecuteGetAsync<TestOccupied>(request);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Data.Occupied);
        }

        //[Test]
        //public async void then_the_post_request_should_be_made_with_a_valid_resource()
        //{
        //    var factory = new AuthenticatedRequestFactory(Config.AppKey, Config.AppId, Config.AppSecret);

        //    var testObject = new { hello = "world" };

        //    var request = factory.Build(EventflitMethod.POST, "/trigger", requestBody: testObject);

        //    var client = new EventflitRestClient("http://service.eventflit.com");
        //    var response = await client.ExecuteAsync<object>(request, "eventflit-http-dotnet", Version.Parse("3.0.0"));

        //    Assert.IsNotNull(response);
        //}
    }
}