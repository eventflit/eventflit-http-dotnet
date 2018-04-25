using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EventflitServer.RestfulClient
{
    /// <summary>
    /// A REST request to be made to the Eventflit API
    /// </summary>
    public class EventflitRestRequest : IEventflitRestRequest
    {
        /// <summary>
        /// Creates a new REST request to make back to Eventflit HQ
        /// </summary>
        /// <param name="resourceUri">The URI to call</param>
        public EventflitRestRequest(string resourceUri)
        {
            if (string.IsNullOrWhiteSpace(resourceUri))
                throw new ArgumentNullException(nameof(resourceUri), "The resource URI must be a populated string");

            ResourceUri = resourceUri;
        }

        /// <inheritdoc/>
        public EventflitMethod Method { get; set; }

        /// <inheritdoc/>
        public string ResourceUri { get; }

        /// <inheritdoc/>
        public object Body { get; set; }

        /// <inheritdoc/>
        public string GetContentAsJsonString()
        {
            return Body != null ? JsonConvert.SerializeObject(Body) : null;
        }
    }
}