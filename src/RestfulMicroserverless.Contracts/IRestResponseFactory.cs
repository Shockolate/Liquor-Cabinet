using System;
using System.Collections.Generic;
using System.Text;

namespace RestfulMicroserverless.Contracts
{
    public interface IRestResponseFactory
    {
        RestResponse CreateCorsRestResponse();

        RestResponse CreateCorsRestResponse(int statusCode);

        RestResponse CreateMethodNotAllowedWithCorsRestResponse(HttpVerb invokedVerb, string invokedPath);

        RestResponse CreateErrorMessageRestResponse(string errorMessage);

        RestResponse CreateErrorMessageRestResponse(string errorMessage, int statusCode);
    }
}
