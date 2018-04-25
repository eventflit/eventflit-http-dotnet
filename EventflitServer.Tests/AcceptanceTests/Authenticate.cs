using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using EventflitServer.Tests.Helpers;

namespace EventflitServer.Tests.AcceptanceTests
{
    [TestFixture]
    public class When_authenticating_a_private_subscription
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            EventflitClient.Eventflit.Trace.Listeners.Add(new ConsoleTraceListener(true));
        }

        [Test]
        public void the_authentication_token_for_a_private_channel_should_be_accepted_by_Eventflit()
        {
            EventflitServer.Eventflit eventflitServer = new Eventflit(Config.AppId, Config.AppKey, Config.AppSecret, new EventflitOptions()
            {
                HostName = Config.HttpHost
            });
            EventflitClient.Eventflit eventflitClient =
                new EventflitClient.Eventflit(Config.AppKey, new EventflitClient.EventflitOptions()
                    {
                        Authorizer = new InMemoryAuthorizer(eventflitServer)
                    });
            eventflitClient.Host = Config.WebSocketHost;

            string channelName = "private-channel";

            bool subscribed = false;
            AutoResetEvent reset = new AutoResetEvent(false);

            eventflitClient.Connected += new EventflitClient.ConnectedEventHandler(delegate(object sender)
            {
                Debug.WriteLine("connected");
                reset.Set();
            });

            Debug.WriteLine("connecting");
            eventflitClient.Connect();

            Debug.WriteLine("waiting to connect");
            reset.WaitOne(TimeSpan.FromSeconds(5));

            Debug.WriteLine("subscribing");
            var channel = eventflitClient.Subscribe(channelName);
            channel.Subscribed += new EventflitClient.SubscriptionEventHandler(delegate(object s)
            {
                Debug.WriteLine("subscribed");
                subscribed = true;
                reset.Set();
            });

            Debug.WriteLine("waiting to subscribe");
            reset.WaitOne(TimeSpan.FromSeconds(5));

            Assert.IsTrue(subscribed);
        }

        [Test]
        public void the_authentication_token_for_a_presence_channel_should_be_accepted_by_Eventflit()
        {
            EventflitServer.Eventflit eventflitServer = new Eventflit(Config.AppId, Config.AppKey, Config.AppSecret, new EventflitOptions()
            {
                HostName = Config.HttpHost
            });
            EventflitClient.Eventflit eventflitClient =
                new EventflitClient.Eventflit(Config.AppKey, new EventflitClient.EventflitOptions()
                {
                    Authorizer = new InMemoryAuthorizer(
                        eventflitServer,
                        new PresenceChannelData()
                        {
                            user_id = "leggetter",
                            user_info = new { twitter_id = "@leggetter" }
                        })
                });
            eventflitClient.Host = Config.WebSocketHost;

            string channelName = "presence-channel";

            bool subscribed = false;
            AutoResetEvent reset = new AutoResetEvent(false);

            eventflitClient.Connected += new EventflitClient.ConnectedEventHandler(delegate(object sender)
            {
                Debug.WriteLine("connected");
                reset.Set();
            });

            Debug.WriteLine("connecting");
            eventflitClient.Connect();

            Debug.WriteLine("waiting to connect");
            reset.WaitOne(TimeSpan.FromSeconds(10));

            Debug.WriteLine("subscribing");
            var channel = eventflitClient.Subscribe(channelName);
            channel.Subscribed += new EventflitClient.SubscriptionEventHandler(delegate(object s)
            {
                Debug.WriteLine("subscribed");
                subscribed = true;
                reset.Set();
            });

            Debug.WriteLine("waiting to subscribe");
            reset.WaitOne(TimeSpan.FromSeconds(5));

            Assert.IsTrue(subscribed);
        }
    }
}
