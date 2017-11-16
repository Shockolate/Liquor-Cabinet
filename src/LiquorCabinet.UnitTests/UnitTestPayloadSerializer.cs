using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests
{
    internal class UnitTestPayloadSerializer : IPayloadSerializer
    {
        private readonly JsonSerializer _jsonSerializer;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public UnitTestPayloadSerializer()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Include,
                MissingMemberHandling = MissingMemberHandling.Error
            };

            _jsonSerializer = JsonSerializer.Create(_jsonSerializerSettings);
        }

        public string SerializePayload(object objectToConvert) => JsonConvert.SerializeObject(objectToConvert, _jsonSerializerSettings);

        public void SerializePayload(object objectToConvert, Stream targetStream)
        {
            using (var streamWriter = new StreamWriter(targetStream))
            using (var textWriter = new JsonTextWriter(streamWriter))
            {
                _jsonSerializer.Serialize(textWriter, objectToConvert);
            }
        }

        public T DeserializePayload<T>(string payload)
        {
            using (var stringReader = new StringReader(payload))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                return _jsonSerializer.Deserialize<T>(jsonReader);
            }
        }

        public T DeserializePayload<T>(Stream payloadStream)
        {
            using (var streamReader = new StreamReader(payloadStream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return _jsonSerializer.Deserialize<T>(jsonReader);
            }
        }
    }
}