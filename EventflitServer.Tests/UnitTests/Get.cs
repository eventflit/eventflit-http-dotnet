using NSubstitute;
using NUnit.Framework;
using EventflitServer.RestfulClient;

namespace EventflitServer.Tests.UnitTests
{
    [TestFixture]
    public class When_using_async_Get_to_retrieve_a_list_of_application_channels
    {
        IEventflit _eventflit;
        IEventflitRestClient _subEventflitClient;

        [SetUp]
        public void Setup()
        {
            _subEventflitClient = Substitute.For<IEventflitRestClient>();

            IEventflitOptions options = new EventflitOptions()
            {
                RestClient = _subEventflitClient
            };

            Config.AppId = "test-app-id";
            Config.AppKey = "test-app-key";
            Config.AppSecret = "test-app-secret";

            _eventflit = new Eventflit(Config.AppId, Config.AppKey, Config.AppSecret, options);
        }

        [Test]
        public async void url_is_in_expected_format()
        {
            await _eventflit.GetAsync<object>("/channels");

#pragma warning disable 4014
            _subEventflitClient.Received().ExecuteGetAsync<object>(
#pragma warning restore 4014
                Arg.Is<IEventflitRestRequest>(
                    x => x.ResourceUri.StartsWith("/apps/" + Config.AppId + "/channels")
                )
            );
        }

        [Test]
        public async void GET_request_is_made()
        {
            await _eventflit.GetAsync<object>("/channels");

#pragma warning disable 4014
            _subEventflitClient.Received().ExecuteGetAsync<object>(
#pragma warning restore 4014
                Arg.Is<IEventflitRestRequest>(
                    x => x.Method == EventflitMethod.GET
                )
            );
        }

        [Test]
        public async void additional_parameters_should_be_added_to_query_string()
        {
            await _eventflit.GetAsync<object>("/channels", new { filter_by_prefix = "presence-", info = "user_count" });

#pragma warning disable 4014
            _subEventflitClient.Received().ExecuteGetAsync<object>(
#pragma warning restore 4014
                Arg.Is<IEventflitRestRequest>(
                    x => x.ResourceUri.Contains("&filter_by_prefix=presence-") &&
                         x.ResourceUri.Contains("&info=user_count")
                )
            );
        }
    }
}
