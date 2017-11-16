﻿using System;
using System.Threading.Tasks;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.users
{
    internal sealed class Handler : AbstractPathHandler
    {
        public Handler(RestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer) : base(restResponseFactory, payloadSerializer)
        {
            VerbHandlers.Add(HttpVerb.Post, PostAsync);
        }

        public Task<RestResponse> PostAsync(RestRequest request, ILogger logger) => throw new NotImplementedException();
    }
}