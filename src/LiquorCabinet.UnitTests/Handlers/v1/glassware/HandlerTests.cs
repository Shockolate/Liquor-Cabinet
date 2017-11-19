using System.Collections.Generic;
using LiquorCabinet.PathHandlers.v1.glassware;
using LiquorCabinet.Repositories.Entities;
using LiquorCabinet.UnitTests.Repositories;
using NUnit.Framework;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Handlers.v1.glassware
{
    [TestFixture]
    public class HandlerTests
    {

        #region SetUp and Fields

        private Handler _handlerUnderTest;
        private InMemoryGlasswareRepository _glasswareRepository;
        private RestResponseFactory _restResponseFactory;
        private IPayloadSerializer _payloadSerializer;
        private ILogger _logger;

        private static readonly RestRequest _validGetRestRequest = new RestRequest
        {
            Body = string.Empty,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
            InvokedPath = "/v1/glassware",
            Method = HttpVerb.Get
        };

        private static readonly RestRequest _validPostRestRequest = new RestRequest
        {
            Body = @"{""name"":""Shot Glass"",""description"":""enough for a shot"",""typicalSize"":""2 oz""}",
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } },
            InvokedPath = "/v1/glassware",
            Method = HttpVerb.Post
        };

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _logger = new UnitTestLogger();
            _restResponseFactory = new RestResponseFactory();
            _payloadSerializer = new UnitTestPayloadSerializer();
        }

        [SetUp]
        public void SetUp()
        {
            _glasswareRepository = new InMemoryGlasswareRepository();
            _handlerUnderTest = new Handler(_restResponseFactory, _payloadSerializer, _glasswareRepository);
        }

        #endregion

        #region POST

        [TestCase(@"{""name"":""Shot Glass"",""foo"":""bar"",""typicalSize"":""2 oz""}")]
        [TestCase(@"{""name"":""Shot Glass"",""description"":""bar"",""typicalSize"":""2 oz""")]
        [TestCase(@"{""name"":""Shot Glass"",""description"":""bar"",""typicalSize"":""2 oz"", ""extra"":""property""}")]
        public void PostReturns400OnBadJson(string body)
        {
            var invalidRestRequest = new RestRequest
            {
                Body = body,
                Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}},
                InvokedPath = "/v1/glassware",
                Method = HttpVerb.Post
            };
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.PostAsync(invalidRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(400));
            });
        }

        [Test]
        public void PostReturns500OnSqlError()
        {
            var throwingGlasswareRepository = new InMemoryGlasswareRepository(true);
            _handlerUnderTest = new Handler(_restResponseFactory, _payloadSerializer, throwingGlasswareRepository);
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.PostAsync(_validPostRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(500));
            });
        }

        [Test]
        public void PostSuccessfullyAdds()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.PostAsync(_validPostRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(201));
                Assert.That(_glasswareRepository.Glasses.Count, Is.EqualTo(3));
            });
        }

        #endregion POST

        #region GET

        [Test]
        public void GetReturns500OnSqlError()
        {
            var throwingGlasswareRepository = new InMemoryGlasswareRepository(true);
            _handlerUnderTest = new Handler(_restResponseFactory, _payloadSerializer, throwingGlasswareRepository);
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.GetAsync(_validGetRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(500));
            });
        }

        [Test]
        public void GetSuccessfullyRetrieves()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.GetAsync(_validGetRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(200));
                Assert.That(((List<Glass>) response.Body).Count, Is.EqualTo(2));
            });
        }

        #endregion

    }
}