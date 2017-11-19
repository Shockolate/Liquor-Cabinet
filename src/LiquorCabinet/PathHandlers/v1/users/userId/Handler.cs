using System;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.users.userId
{
    internal sealed class Handler : BaseHandler
    {
        public Handler(IRestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer) : base(restResponseFactory, payloadSerializer)
        {
            VerbHandlers.Add(HttpVerb.Get, GetAsync);
            VerbHandlers.Add(HttpVerb.Put, PutAsync);
        }

        public Task<RestResponse> GetAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
        public Task<RestResponse> PutAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
    }
}