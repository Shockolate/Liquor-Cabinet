using System;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.users.userId.ingredients.ingredientId
{
    internal sealed class Handler : BaseHandler
    {
        public Handler(RestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer) : base(restResponseFactory, payloadSerializer)
        {
            VerbHandlers.Add(HttpVerb.Delete, DeleteAsync);
        }

        public Task<RestResponse> DeleteAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
    }
}