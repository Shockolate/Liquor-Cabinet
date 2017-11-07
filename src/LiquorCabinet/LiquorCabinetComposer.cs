using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet
{
    public static class LiquorCabinetComposer
    {
        public static IEnumerable<IHttpPathHandler> CreatePathHandlers(IHttpPathHandlerFactory pathHandlerFactory, IPayloadSerializer payloadSerializer,
            IConfiguration configuration)
        {
            // Use Configuration.
            var restResponseFactory = new RestResponseFactory(payloadSerializer);
            
            return new List<IHttpPathHandler>();
        }
    }
}