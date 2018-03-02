using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LiquorCabinet
{
    internal class JsonSerializerFactory
    {
        internal static JsonSerializerSettings DefaultSettings { get; } = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Error
        };
    }
}
