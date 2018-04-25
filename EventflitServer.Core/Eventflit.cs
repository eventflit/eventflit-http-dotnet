using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EventflitServer.RestfulClient;

namespace EventflitServer
{
    /// <summary>
    /// Provides access to functionality within the Eventflit service such as Trigger to trigger events
    /// and authenticating subscription requests to private and presence channels.
    /// </summary>
    public class Eventflit : IEventflit
    {
        private const string ChannelUsersResource = "/channels/{0}/users";
        private const string ChannelResource = "/channels/{0}";
        private const string MultipleChannelsResource = "/channels";

        private readonly string _appKey;
        private readonly string _appSecret;
        private readonly IEventflitOptions _options;

        private readonly IAuthenticatedRequestFactory _factory;

        /// <summary>
        /// Eventflit library version information.
        /// </summary>
        public static Version VERSION
        {
            get
            {
                return typeof(Eventflit).GetTypeInfo().Assembly.GetName().Version;
            }
        }

        /// <summary>
        /// The Eventflit library name.
        /// </summary>
        public static String LIBRARY_NAME
        {
            get
            {
                Attribute attr = typeof(Eventflit).GetTypeInfo().Assembly.GetCustomAttribute(typeof(AssemblyProductAttribute));

                AssemblyProductAttribute adAttr = (AssemblyProductAttribute)attr;
                
                return adAttr.Product;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Eventflit" /> class.
        /// </summary>
        /// <param name="appId">The app id.</param>
        /// <param name="appKey">The app key.</param>
        /// <param name="appSecret">The app secret.</param>
        /// <param name="options">(Optional)Additional options to be used with the instance e.g. setting the call to the REST API to be made over HTTPS.</param>
        public Eventflit(string appId, string appKey, string appSecret, IEventflitOptions options = null)
        {
            ThrowArgumentExceptionIfNullOrEmpty(appId, "appId");
            ThrowArgumentExceptionIfNullOrEmpty(appKey, "appKey");
            ThrowArgumentExceptionIfNullOrEmpty(appSecret, "appSecret");

            if (options == null)
            {
                options = new EventflitOptions();
            }

            _appKey = appKey;
            _appSecret = appSecret;
            _options = options;

            _factory = new AuthenticatedRequestFactory(appKey, appId, appSecret);
        }

        /// <inheritdoc/>
        public async Task<ITriggerResult> TriggerAsync(string channelName, string eventName, object data, ITriggerOptions options = null)
        {
            return await TriggerAsync(new[] { channelName }, eventName, data, options);
        }

        /// <inheritdoc/>
        public async Task<ITriggerResult> TriggerAsync(string[] channelNames, string eventName, object data, ITriggerOptions options = null)
        {
            if (options == null)
                options = new TriggerOptions();

            var bodyData = CreateTriggerBody(channelNames, eventName, data, options);

            var request = _factory.Build(EventflitMethod.POST, "/events", requestBody: bodyData);

            DebugTriggerRequest(request);

            var result = await _options.RestClient.ExecutePostAsync(request);

            DebugTriggerResponse(result);

            return result;
        }

        ///<inheritDoc/>
        public async Task<ITriggerResult> TriggerAsync(Event[] events)
        {
            var bodyData = CreateBatchTriggerBody(events);

            var request = _factory.Build(EventflitMethod.POST, "/batch_events", requestBody: bodyData);

            DebugTriggerRequest(request);

            var result = await _options.RestClient.ExecutePostAsync(request);

            DebugTriggerResponse(result);

            return result;
        }

        private TriggerBody CreateTriggerBody(string[] channelNames, string eventName, object data, ITriggerOptions options)
        {
            ValidationHelper.ValidateChannelNames(channelNames);
            ValidationHelper.ValidateSocketId(options.SocketId);

            TriggerBody bodyData = new TriggerBody()
            {
                name = eventName,
                data = _options.JsonSerializer.Serialize(data),
                channels = channelNames
            };

            if (string.IsNullOrEmpty(options.SocketId) == false)
            {
                bodyData.socket_id = options.SocketId;
            }

            return bodyData;
        }

        private BatchTriggerBody CreateBatchTriggerBody(Event[] events)
        {
            ValidationHelper.ValidateBatchEvents(events);
            
            var batchEvents = events.Select(e => new BatchEvent
            {
                name = e.EventName,
                channel = e.Channel,
                socket_id = e.SocketId,
                data = _options.JsonSerializer.Serialize(e.Data)
            }).ToArray();

            return new BatchTriggerBody()
            {
                batch = batchEvents
            };
        }

        ///<inheritDoc/>
        public IAuthenticationData Authenticate(string channelName, string socketId)
        {
            return new AuthenticationData(_appKey, _appSecret, channelName, socketId);
        }

        ///<inheritDoc/>
        public IAuthenticationData Authenticate(string channelName, string socketId, PresenceChannelData presenceData)
        {
            if(presenceData == null)
            {
                throw new ArgumentNullException(nameof(presenceData));
            }

            return new AuthenticationData(_appKey, _appSecret, channelName, socketId, presenceData);
        }

        ///<inheritDoc/>
        public async Task<IGetResult<T>> GetAsync<T>(string resource, object parameters = null)
        {
            var request = _factory.Build(EventflitMethod.GET, resource, parameters);

            var response = await _options.RestClient.ExecuteGetAsync<T>(request);

            return response;
        }

        ///<inheritDoc/>
        public IWebHook ProcessWebHook(string signature, string body)
        {
            return new WebHook(_appSecret, signature, body);
        }

        /// <inheritDoc/>
        public async Task<IGetResult<T>> FetchUsersFromPresenceChannelAsync<T>(string channelName)
        {
            ThrowArgumentExceptionIfNullOrEmpty(channelName, "channelName");

            var request = _factory.Build(EventflitMethod.GET, string.Format(ChannelUsersResource, channelName));

            var response = await _options.RestClient.ExecuteGetAsync<T>(request);

            return response;
        }

        /// <inheritDoc/>
        public async Task<IGetResult<T>> FetchStateForChannelAsync<T>(string channelName, object info = null)
        {
            ThrowArgumentExceptionIfNullOrEmpty(channelName, "channelName");

            var request = _factory.Build(EventflitMethod.GET, string.Format(ChannelResource, channelName), info);

            var response = await _options.RestClient.ExecuteGetAsync<T>(request);

            return response;
        }

        /// <inheritDoc/>
        public async Task<IGetResult<T>> FetchStateForChannelsAsync<T>(object info = null)
        {
            var request = _factory.Build(EventflitMethod.GET, MultipleChannelsResource, info);

            var response = await _options.RestClient.ExecuteGetAsync<T>(request);

            return response;
        }

        private void DebugTriggerRequest(IEventflitRestRequest request)
        {
            Debug.WriteLine($"Method: {request.Method}{Environment.NewLine}Host: {_options.RestClient.BaseUrl}{Environment.NewLine}Resource: {request.ResourceUri}{Environment.NewLine}Body:{request.Body}");
        }

        private void DebugTriggerResponse(TriggerResult response)
        {
            Debug.WriteLine($"Response{Environment.NewLine}StatusCode: {response.StatusCode}{Environment.NewLine}Body: {response.OriginalContent}");
        }

        private static void ThrowArgumentExceptionIfNullOrEmpty(string value, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{argumentName} cannot be null or empty");
            }
        }
    }
}