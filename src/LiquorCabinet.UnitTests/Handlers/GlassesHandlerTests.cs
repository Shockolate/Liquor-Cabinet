using System.Collections.Generic;
using LiquorCabinet.PathHandlers.v1.glassware;
using LiquorCabinet.Repositories.Entities;
using LiquorCabinet.UnitTests.Repositories.Glassware;
using NUnit.Framework;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Handlers
{
    [TestFixture]
    public class GlassesHandlerTests
    {
        private Handler _handlerUnderTest;
        private ILogger _logger;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _handlerUnderTest = new Handler(new RestResponseFactory(), new UnitTestPayloadSerializer(), new InMemoryGlasswareRepository());
            _logger = new UnitTestLogger();
        }

        [Test]
        public void GetSuccessfullyRetrieves()
        {
            var request = new RestRequest
            {
                Body = string.Empty,
                Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}},
                InvokedPath = "/v1/glassware",
                Method = HttpVerb.Get
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.GetAsync(request, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(200));
                Assert.That(((List<Glass>) response.Body).Count, Is.EqualTo(2));
            });
        }
    }
}