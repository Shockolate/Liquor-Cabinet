using System;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.recipes.recipeId.components.componentId
{
    internal sealed class Handler : AbstractPathHandler
    {
        public Handler(RestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer) : base(restResponseFactory, payloadSerializer)
        {
            VerbHandlers.Add(HttpVerb.Put, PutAsync);
        }

        public Task<RestResponse> PutAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
    }
}