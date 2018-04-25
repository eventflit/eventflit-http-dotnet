using System;
using EventflitServer.RestfulClient;

namespace EventflitServer
{
    /// <summary>
    /// Interface for Eventflit Options
    /// </summary>
    public interface IEventflitOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether calls to the Eventflit REST API are over HTTP or HTTPS.
        /// </summary>
        /// <value>
        ///   <c>true</c> if encrypted; otherwise, <c>false</c>.
        /// </value>
        bool Encrypted { get; set; }

        /// <summary>
        /// Gets or sets the REST API port that the HTTP calls will be made to.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        int Port { get; set; }

        /// <summary>
        /// Gets or sets the Json Serializer
        /// </summary>
        ISerializeObjectsToJson JsonSerializer { get; set; }

        /// <summary>
        /// Gets or sets the Json Deserializer
        /// </summary>
        IDeserializeJsonStrings JsonDeserializer { get; set; }

        /// <summary>
        /// Gets or sets the eventflit rest client. Generally only expected to be used for testing.
        /// </summary>
        /// <value>
        /// The eventflit rest client.
        /// </value>
        IEventflitRestClient RestClient { get; set; }

        /// <summary>
        /// The host of the HTTP API endpoint excluding the scheme e.g. service.eventflit.com
        /// </summary>
        /// <exception cref="FormatException">If a scheme is found at the start of the host value</exception>
        string HostName { get; set; }

        /// <summary>
        /// The cluster where the application was created, e.g. eu
        /// </summary>
        string Cluster { get; set; }

        /// <summary>
        /// Gets the base Url based on the set Options
        /// </summary>
        /// <returns>The constructed URL</returns>
        Uri GetBaseUrl();
    }
}
