using System;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.users.userId.ingredients.ingredientId
{
    internal sealed class Handler : AbstractPathHandler
    {
        public Handler(RestResponseFactory restResponseFactory) : base(restResponseFactory)
        {
            VerbHandlers.Add(HttpVerb.Delete, DeleteAsync);
        }

        public Task<RestResponse> DeleteAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
    }
}