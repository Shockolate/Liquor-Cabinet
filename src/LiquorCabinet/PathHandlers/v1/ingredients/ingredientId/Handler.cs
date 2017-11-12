using System;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.ingredients.ingredientId
{
    internal sealed class Handler : AbstractPathHandler
    {
        public Handler(RestResponseFactory restResponseFactory) : base(restResponseFactory)
        {
            VerbHandlers.Add(HttpVerb.Get, GetAsync);
            VerbHandlers.Add(HttpVerb.Put, PutAsync);
            VerbHandlers.Add(HttpVerb.Delete, DeleteAsync);
        }


        public Task<RestResponse> GetAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();

        public Task<RestResponse> PutAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();

        public Task<RestResponse> DeleteAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
    }
}