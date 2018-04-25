using Newtonsoft.Json;

namespace EventflitServer
{
    /// <summary>
    /// Default implmentation for serializing an object
    /// </summary>
    public class DefaultSerializer : ISerializeObjectsToJson
    {
        /// <inheritDoc/>
        public string Serialize(object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }
    }
}