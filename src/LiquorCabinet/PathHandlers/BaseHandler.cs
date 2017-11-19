using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers
{
    internal abstract class BaseHandler
    {
        protected readonly IPayloadSerializer PayloadSerializer;
        protected readonly IRestResponseFactory RestResponseFactory;

        protected BaseHandler(IRestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer)
        {
            RestResponseFactory = restResponseFactory;
            PayloadSerializer = payloadSerializer;
        }

        internal virtual IDictionary<HttpVerb, Func<RestRequest, ILogger, Task<RestResponse>>> VerbHandlers { get; } =
            new Dictionary<HttpVerb, Func<RestRequest, ILogger, Task<RestResponse>>>(Enum.GetNames(typeof(HttpVerb)).Length);
    }
}