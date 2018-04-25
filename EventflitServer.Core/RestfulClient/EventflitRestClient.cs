using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EventflitServer.RestfulClient
{
    /// <summary>
    /// A client for the Eventflit REST requests
    /// </summary>
    public class EventflitRestClient : IEventflitRestClient
    {
        private readonly string _libraryName;
        private readonly string _version;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructs a new instance of the EventflitRestClient
        /// </summary>
        /// <param name="baseAddress">The base address of the Eventflit API as a URI formatted string</param>
        /// <param name="libraryName"></param>
        /// <param name="version"></param>
        public EventflitRestClient(string baseAddress, string libraryName, Version version) : this(new Uri(baseAddress), libraryName, version)
        {}

        /// <summary>
        /// Constructs a new instance of the EventflitRestClient
        /// </summary>
        /// <param name="baseAddress">The base address of the Eventflit API</param>
        /// <param name="libraryName">The name of the Eventflit Library</param>
        /// <param name="version">The version of the Eventflit library</param>
        public EventflitRestClient(Uri baseAddress, string libraryName, Version version)
        {
            _httpClient = new HttpClient { BaseAddress = baseAddress };
            _libraryName = libraryName;
            _version = version.ToString(3);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Eventflit-Library-Name", _libraryName);
            _httpClient.DefaultRequestHeaders.Add("Eventflit-Library-Version", _version);
        }

        ///<inheritDoc/>
        public Uri BaseUrl {
            get { return _httpClient.BaseAddress; } 
        }

        ///<inheritDoc/>
        public async Task<GetResult<T>> ExecuteGetAsync<T>(IEventflitRestRequest eventflitRestRequest)
        {
            if (eventflitRestRequest.Method == EventflitMethod.GET)
            {
                var response = await _httpClient.GetAsync(eventflitRestRequest.ResourceUri);
                var responseContent = await response.Content.ReadAsStringAsync();

                return new GetResult<T>(response, responseContent);
            }

            return null;
        }

        ///<inheritDoc/>
        public async Task<TriggerResult> ExecutePostAsync(IEventflitRestRequest eventflitRestRequest)
        {
            if (eventflitRestRequest.Method == EventflitMethod.POST)
            {
                var content = new StringContent(eventflitRestRequest.GetContentAsJsonString(), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(eventflitRestRequest.ResourceUri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                return new TriggerResult(response, responseContent);
            }

            return null;
        }
    }
}
