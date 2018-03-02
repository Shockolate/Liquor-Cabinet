using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;

namespace LiquorCabinet.APIGatewayAdapter.UnitTests
{
    public class TestEvents
    {
        public APIGatewayProxyRequest ValidGetGlassware { get; } = new APIGatewayProxyRequest
        {
            Body = "",
            Resource = "/{proxy+}",
            RequestContext =
                new APIGatewayProxyRequest.ProxyRequestContext
                {
                    ResourceId = "123456",
                    ApiId = "1234567890",
                    ResourcePath = "/{proxy+}",
                    HttpMethod = "GET",
                    RequestId = "c6af9ac6-7b61-11e6-9a41-93e8deadbeef",
                    AccountId = "123456789012",
                    Identity = new APIGatewayProxyRequest.RequestIdentity
                    {
                        ApiKey = null,
                        UserArn = null,
                        CognitoAuthenticationType = null,
                        Caller = null,
                        UserAgent = "Custom User Agent String",
                        User = null,
                        CognitoIdentityPoolId = null,
                        CognitoIdentityId = null,
                        CognitoAuthenticationProvider = null,
                        SourceIp = "127.0.0.1",
                        AccountId = null
                    },
                    Stage = "production"
                },
            QueryStringParameters = new Dictionary<string, string>(),
            Headers = new Dictionary<string, string>(),
            PathParameters = new Dictionary<string, string>(),
            HttpMethod = "GET",
            StageVariables = new Dictionary<string, string> {{"Verbosity", "Debug"}},
            Path = "/v1/glassware"
        };

        public APIGatewayProxyRequest ValidPostGlassware { get; } = new APIGatewayProxyRequest
        {
            Body = @"{""name"":""Shot Glass"",""description"":""enough for a shot"",""typicalSize"":""2 oz""}",
            Resource = "/{proxy+}",
            RequestContext =
                new APIGatewayProxyRequest.ProxyRequestContext
                {
                    ResourceId = "123456",
                    ApiId = "1234567890",
                    ResourcePath = "/{proxy+}",
                    HttpMethod = "POST",
                    RequestId = "c6af9ac6-7b61-11e6-9a41-93e8deadbeef",
                    AccountId = "123456789012",
                    Identity = new APIGatewayProxyRequest.RequestIdentity
                    {
                        ApiKey = null,
                        UserArn = null,
                        CognitoAuthenticationType = null,
                        Caller = null,
                        UserAgent = "Custom User Agent String",
                        User = null,
                        CognitoIdentityPoolId = null,
                        CognitoIdentityId = null,
                        CognitoAuthenticationProvider = null,
                        SourceIp = "127.0.0.1",
                        AccountId = null
                    },
                    Stage = "production"
                },
            QueryStringParameters = new Dictionary<string, string>(),
            Headers = new Dictionary<string, string>(),
            PathParameters = new Dictionary<string, string>(),
            HttpMethod = "POST",
            StageVariables = new Dictionary<string, string> { { "Verbosity", "Debug" } },
            Path = "/v1/glassware"
        };
    }
}