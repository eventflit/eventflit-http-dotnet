namespace EventflitServer.RestfulClient
{
    /// <summary>
    /// The contract for the factory that creates authenticated requests to send to the Eventflit API
    /// </summary>
    public interface IAuthenticatedRequestFactory
    {
        /// <summary>
        /// Builds a new authenticated request to send to Eventflit
        /// </summary>
        /// <param name="requestType">What type of REST call is to be made</param>
        /// <param name="resource">The resource path for the REST call</param>
        /// <param name="requestParameters">(Optional) Any parameters that need to be included in the call</param>
        /// <param name="requestBody">(Optional) The body to be sent with the request</param>
        /// <returns>A constructed REST request</returns>
        IEventflitRestRequest Build(EventflitMethod requestType, string resource, object requestParameters = null, object requestBody = null);
    }
}