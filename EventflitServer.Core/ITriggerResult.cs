using System.Collections.Generic;

namespace EventflitServer
{
    /// <summary>
    /// Interface for Trigger Request Results
    /// </summary>
    public interface ITriggerResult: IRequestResult
    {
        /// <summary>
        /// Gets the Event IDs related to this Trigger Event
        /// </summary>
        IDictionary<string, string> EventIds { get; }
    }
}
