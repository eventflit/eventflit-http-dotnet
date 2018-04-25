using EventflitClient;
using System.Web.Script.Serialization;

namespace EventflitServer.Tests.Helpers
{
    internal class InMemoryAuthorizer: IAuthorizer
    {
        EventflitServer.Eventflit _eventflit;
        PresenceChannelData _presenceData;

        public InMemoryAuthorizer(EventflitServer.Eventflit eventflit):
            this(eventflit, null)
        {
        }

        public InMemoryAuthorizer(EventflitServer.Eventflit eventflit, PresenceChannelData presenceData)
        {
            _eventflit = eventflit;
            _presenceData = presenceData;
        }

        public string Authorize(string channelName, string socketId)
        {
            IAuthenticationData auth = null;
            if (_presenceData != null)
            {
                auth = _eventflit.Authenticate(channelName, socketId, _presenceData);
            }
            else
            {
                auth = _eventflit.Authenticate(channelName, socketId);
            }
            return auth.ToJson();
        }
    }
}
