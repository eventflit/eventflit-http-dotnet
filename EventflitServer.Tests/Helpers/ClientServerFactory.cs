using System;
using System.Threading;

namespace EventflitServer.Tests.Helpers
{
    internal sealed class ClientServerFactory
    {
        /// <summary>
        /// Create a Eventflit Client, and subscribes a user
        /// </summary>
        /// <param name="eventflitServer">Server to connect to</param>
        /// <param name="reset">The AutoReset to control the subscription by the client</param>
        /// <param name="channelName">The name of the channel to subscribe to</param>
        /// <returns>A subscribed client</returns>
        public static EventflitClient.Eventflit CreateClient(Eventflit eventflitServer, AutoResetEvent reset, string channelName)
        {
            EventflitClient.Eventflit eventflitClient =
                new EventflitClient.Eventflit(Config.AppKey, new EventflitClient.EventflitOptions()
                {
                    Authorizer = new InMemoryAuthorizer(
                        eventflitServer,
                        new PresenceChannelData()
                        {
                            user_id = "Mr Eventflit",
                            user_info = new { twitter_id = "@eventflit" }
                        })
                });

            eventflitClient.Connected += delegate { reset.Set(); };

            eventflitClient.Connect();

            reset.WaitOne(TimeSpan.FromSeconds(5));

            var channel = eventflitClient.Subscribe(channelName);

            channel.Subscribed += delegate { reset.Set(); };

            reset.WaitOne(TimeSpan.FromSeconds(5));

            return eventflitClient;
        }

        /// <summary>
        /// Create a Eventflit Server instance
        /// </summary>
        /// <returns></returns>
        public static EventflitServer.Eventflit CreateServer()
        {
            return new Eventflit(Config.AppId, Config.AppKey, Config.AppSecret);
        }
    }
}
