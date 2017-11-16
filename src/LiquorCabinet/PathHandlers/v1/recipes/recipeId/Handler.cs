using System;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.recipes.recipeId
{
    internal sealed class Handler : AbstractPathHandler
    {
        public Handler(RestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer) : base(restResponseFactory, payloadSerializer)
        {
            VerbHandlers.Add(HttpVerb.Get, GetAsync);
            VerbHandlers.Add(HttpVerb.Put, PutAsync);
        }

        public Task<RestResponse> GetAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
        public Task<RestResponse> PutAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
    }
}