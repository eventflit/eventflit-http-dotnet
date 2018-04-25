using System;
using System.Threading.Tasks;

namespace EventflitServer.RestfulClient
{
    /// <summary>
    /// Contract for a client for the Eventflit REST requests
    /// </summary>
    public interface IEventflitRestClient
    {
        /// <summary>
        /// Execute a REST GET request to the Eventflit API asynchronously
        /// </summary>
        /// <param name="eventflitRestRequest">The request to execute</param>
        /// <returns>The response received from Eventflit</returns>
        Task<GetResult<T>> ExecuteGetAsync<T>(IEventflitRestRequest eventflitRestRequest);

        /// <summary>
        /// Execute a REST POST request to the Eventflit API asynchronously
        /// </summary>
        /// <param name="eventflitRestRequest">The request to execute</param>
        /// <returns>The response received from Eventflit</returns>
        Task<TriggerResult> ExecutePostAsync(IEventflitRestRequest eventflitRestRequest);

        /// <summary>
        /// Gets the Base Url this client is using
        /// </summary>
        Uri BaseUrl { get; }
    }
}