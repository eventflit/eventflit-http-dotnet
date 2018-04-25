using System.Diagnostics;
using System.Net;
using NUnit.Framework;

namespace EventflitServer.Tests.AcceptanceTests
{
    [TestFixture]
    public class When_application_channels_are_queried
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            EventflitClient.Eventflit.Trace.Listeners.Add(new ConsoleTraceListener(true));
        }

        [Test]
        public async void It_should_return_a_200_response()
        {
            IEventflit eventflit = new Eventflit(Config.AppId, Config.AppKey, Config.AppSecret, new EventflitOptions()
            {
                HostName = Config.HttpHost
            });

            IGetResult<object> result = await eventflit.GetAsync<object>("/channels");

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public async void It_should_be_possible_to_deserialize_the_request_result_body_as_an_object()
        {
            IEventflit eventflit = new Eventflit(Config.AppId, Config.AppKey, Config.AppSecret, new EventflitOptions()
            {
                HostName = Config.HttpHost
            });

            IGetResult<object> result = await eventflit.GetAsync<object>("/channels");

            Assert.NotNull(result.Data);
        }

        [Test]
        public async void It_should_be_possible_to_deserialize_the_a_channels_result_body_as_an_ChannelsList()
        {
            IEventflit eventflit = new Eventflit(Config.AppId, Config.AppKey, Config.AppSecret, new EventflitOptions()
            {
                HostName = Config.HttpHost
            });

            IGetResult<ChannelsList> result = await eventflit.GetAsync<ChannelsList>("/channels");

            Assert.IsTrue(result.Data.Channels.Count >= 0);
        }
    }
}
