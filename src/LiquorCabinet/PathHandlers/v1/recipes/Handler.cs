using System;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.recipes
{
    internal sealed class Handler : AbstractPathHandler
    {
        public Handler(RestResponseFactory restResponseFactory) : base(restResponseFactory)
        {
            VerbHandlers.Add(HttpVerb.Get, GetAsync);
            VerbHandlers.Add(HttpVerb.Post, PostAsync);
        }

        public Task<RestResponse> GetAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();

        public Task<RestResponse> PostAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
    }
}