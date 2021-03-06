﻿namespace RestfulMicroserverless.Contracts
{
    public class RestResponseFactory : IRestResponseFactory
    {
        public RestResponse CreateCorsRestResponse()
        {
            var response = new RestResponse();
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        public RestResponse CreateCorsRestResponse(int statusCode)
        {
            var response = CreateCorsRestResponse();
            response.StatusCode = statusCode;
            return response;
        }

        public RestResponse CreateMethodNotAllowedWithCorsRestResponse(HttpVerb invokedVerb, string invokedPath)
        {
            var errorMessage = $"{invokedVerb:G} Not Implemented on {invokedPath}";
            var response = CreateErrorMessageRestResponse(errorMessage);
            // 405 - Method Not Allowed
            response.StatusCode = 405;
            return response;
        }

        public RestResponse CreateErrorMessageRestResponse(string errorMessage)
        {
            var response = CreateCorsRestResponse();
            response.Body = new {errorMessage};
            return response;
        }


        public RestResponse CreateErrorMessageRestResponse(string errorMessage, int statusCode)
        {
            var response = CreateErrorMessageRestResponse(errorMessage);
            response.StatusCode = statusCode;
            return response;
        }
    }
}