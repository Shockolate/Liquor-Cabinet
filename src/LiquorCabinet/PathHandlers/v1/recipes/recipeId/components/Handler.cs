using System;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.recipes.recipeId.components
{
    internal sealed class Handler : BaseHandler
    {
        public Handler(RestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer) : base(restResponseFactory, payloadSerializer)
        {
            VerbHandlers.Add(HttpVerb.Post, PostAsync);
        }

        public Task<RestResponse> PostAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
    }
}