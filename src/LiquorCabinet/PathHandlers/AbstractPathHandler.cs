using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers
{
    internal abstract class AbstractPathHandler
    {
        protected readonly RestResponseFactory RestResponseFactory;
        protected readonly IPayloadSerializer PayloadSerializer;
        protected AbstractPathHandler(RestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer)
        {
            RestResponseFactory = restResponseFactory;
            PayloadSerializer = payloadSerializer;
        }

        internal virtual IDictionary<HttpVerb, Func<RestRequest, ILogger, Task<RestResponse>>> VerbHandlers { get; } =
            new Dictionary<HttpVerb, Func<RestRequest, ILogger, Task<RestResponse>>>(Enum.GetNames(typeof(HttpVerb)).Length);
    }
}