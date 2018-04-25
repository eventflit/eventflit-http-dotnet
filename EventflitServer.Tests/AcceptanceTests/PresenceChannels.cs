using System;
using System.Net;
using System.Threading;
using NUnit.Framework;
using EventflitServer.Tests.Helpers;

namespace EventflitServer.Tests.AcceptanceTests
{
    [TestFixture]
    public class When_querying_the_Presence_Channel
    {
        [Test]
        public async void Should_get_a_list_of_subscribed_users_asynchronously_when_using_the_correct_channel_name_and_users_are_subscribed()
        {
            var reset = new AutoResetEvent(false);

            string channelName = "presence-test-channel-async-1";

            var eventflitServer = ClientServerFactory.CreateServer();
            var eventflitClient = ClientServerFactory.CreateClient(eventflitServer, reset, channelName);

            IGetResult<PresenceChannelMessage> result = await eventflitServer.FetchUsersFromPresenceChannelAsync<PresenceChannelMessage>(channelName);

            reset.Set();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(1, result.Data.Users.Length);
            Assert.AreEqual("Mr Eventflit", result.Data.Users[0].Id);
        }

        [Test]
        public async void Should_get_an_empty_list_of_subscribed_users_asynchronously_when_using_the_correct_channel_name_and_no_users_are_subscribed()
        {
            var reset = new AutoResetEvent(false);

            string channelName = "presence-test-channel-async-2";

            var eventflitServer = ClientServerFactory.CreateServer();

            IGetResult<PresenceChannelMessage> result = await eventflitServer.FetchUsersFromPresenceChannelAsync<PresenceChannelMessage>(channelName);

            reset.Set();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(0, result.Data.Users.Length);
        }

        [Test]
        public async void should_return_bad_request_asynchronously_using_an_incorrect_channel_name_and_users_are_subscribed()
        {
            var reset = new AutoResetEvent(false);

            string channelName = "presence-test-channel-async-3";

            var eventflitServer = ClientServerFactory.CreateServer();
            var eventflitClient = ClientServerFactory.CreateClient(eventflitServer, reset, channelName);

            IGetResult<PresenceChannelMessage> result = await eventflitServer.FetchUsersFromPresenceChannelAsync<PresenceChannelMessage>("test-channel-async");

            reset.Set();

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async void should_throw_an_exception_when_given_a_null_for_a_channel_name_async()
        {
            var reset = new AutoResetEvent(false);

            var eventflitServer = ClientServerFactory.CreateServer();

            ArgumentException caughtException = null;

            try
            {
                var response = await eventflitServer.FetchUsersFromPresenceChannelAsync<PresenceChannelMessage>(null);
                reset.Set();
            }
            catch (ArgumentException ex)
            {
                caughtException = ex;
            }

            Assert.IsNotNull(caughtException);
            StringAssert.IsMatch("channelName cannot be null or empty", caughtException.Message);
        }

        [Test]
        public async void should_throw_an_exception_when_given_an_empty_string_for_a_channel_name_async()
        {
            var reset = new AutoResetEvent(false);

            var eventflitServer = ClientServerFactory.CreateServer();

            ArgumentException caughtException = null;

            try
            {
                var response = await eventflitServer.FetchUsersFromPresenceChannelAsync<PresenceChannelMessage>(string.Empty);
                reset.Set();
            }
            catch (ArgumentException ex)
            {
                caughtException = ex;
            }

            Assert.IsNotNull(caughtException);
            StringAssert.IsMatch("channelName cannot be null or empty", caughtException.Message);
        }

        private class PresenceChannelMessage
        {
            public PresenceChannelUser[] Users { get; set; }
        }

        private class PresenceChannelUser
        {
            public string Id { get; set; }
        }
    }
}
