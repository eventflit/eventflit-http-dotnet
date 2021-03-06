﻿using Newtonsoft.Json;

namespace EventflitServer
{
    /// <summary>
    /// Default implmentation for deserializing an object
    /// </summary>
    public class DefaultDeserializer : IDeserializeJsonStrings
    {
        /// <inheritDoc/>
        public T Deserialize<T>(string stringToDeserialize)
        {
            return JsonConvert.DeserializeObject<T>(stringToDeserialize);
        }
    }
}