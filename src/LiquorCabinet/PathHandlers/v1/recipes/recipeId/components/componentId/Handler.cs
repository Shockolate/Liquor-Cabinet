using System;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.recipes.recipeId.components.componentId
{
    internal sealed class RestfulMicroserverlessHandler1 : AbstractPathHandler
    {
        public RestfulMicroserverlessHandler1(RestResponseFactory restResponseFactory) : base(restResponseFactory)
        {
            VerbHandlers.Add(HttpVerb.Put, PutAsync);
        }

        public Task<RestResponse> PutAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
    }
}