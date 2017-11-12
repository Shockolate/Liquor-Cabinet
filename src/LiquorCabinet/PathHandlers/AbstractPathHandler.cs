using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers
{
    internal abstract class AbstractPathHandler
    {
        protected readonly RestResponseFactory RestResponseFactory;
        protected AbstractPathHandler(RestResponseFactory restResponseFactory)
        {
            RestResponseFactory = restResponseFactory;
        }

        internal virtual IDictionary<HttpVerb, Func<RestRequest, ILogger, Task<RestResponse>>> VerbHandlers { get; } =
            new Dictionary<HttpVerb, Func<RestRequest, ILogger, Task<RestResponse>>>(Enum.GetNames(typeof(HttpVerb)).Length);
    }
}