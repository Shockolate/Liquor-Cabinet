using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using NUnit.Framework;

namespace LiquorCabinet.APIGatewayAdapter.UnitTests
{
    [TestFixture]
    public class LambdaExecutorTests
    {
        [Test]
        public void TestLambdaExecutorDefaultConstructor()
        {
            Assert.DoesNotThrow(() =>
            {
                var executor = new LambdaExecutor();
                Assert.That(executor, Is.Not.Null);
            });
        }

        private APIGatewayProxyRequest getGlasswareEvent = new APIGatewayProxyRequest
        {
            Body = "",
            Resource = "/{proxy+}",
            RequestContext = new APIGatewayProxyRequest.ProxyRequestContext
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
                    AccountId = null,
                },
                Stage = "production"
            },
            QueryStringParameters = new Dictionary<string, string>(),
            Headers = new Dictionary<string, string>(),
            PathParameters = new Dictionary<string, string>(),
            HttpMethod = "GET",
            StageVariables = new Dictionary<string, string> { { "Verbosity", "Debug"} },
            Path = "/v1/glassware"
        };
    }
}