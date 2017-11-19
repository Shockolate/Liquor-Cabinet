using LiquorCabinet.PathHandlers.v1.ingredients.ingredientId;
using LiquorCabinet.Repositories.Entities;
using LiquorCabinet.UnitTests.Repositories;
using NUnit.Framework;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Handlers.v1.ingredients.ingredientId
{
    [TestFixture]
    public class HandlerTests
    {
        [SetUp]
        public void SetUp()
        {
            _ingredientRepository = new InMemoryIngredientRepository(false);
            _handlerUnderTest = new Handler(_restResponseFactory, _payloadSerializer, _ingredientRepository);
        }

        private Handler _handlerUnderTest;
        private ILogger _logger;
        private IRestResponseFactory _restResponseFactory;
        private IPayloadSerializer _payloadSerializer;
        private InMemoryIngredientRepository _ingredientRepository;

        private readonly RestRequest _validGetRestRequest1 = new RestRequest {Method = HttpVerb.Get, InvokedPath = "/v1/ingredients/1"};
        private readonly RestRequest _validGetRestRequest2 = new RestRequest {Method = HttpVerb.Get, InvokedPath = "/v1/ingredients/2"};
        private readonly RestRequest _notfoundGetRestRequest = new RestRequest {Method = HttpVerb.Get, InvokedPath = "/v1/ingredients/3"};

        private readonly RestRequest _validPutRestRequest = new RestRequest
        {
            Method = HttpVerb.Put,
            InvokedPath = "/v1/ingredients/1",
            Body = @"{""id"":1,""name"":""Vodka"",""description"":""New Description.""}"
        };

        private readonly RestRequest _invalidPutRestRequestMismatch400 = new RestRequest
        {
            Method = HttpVerb.Put,
            InvokedPath = "/v1/ingredients/2",
            Body = @"{""id"":1,""name"":""Vodka"",""description"":""New Description.""}"
        };

        private readonly RestRequest _validDeleteRequest = new RestRequest {Method = HttpVerb.Delete, InvokedPath = "/v1/ingredients/1"};

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _logger = new UnitTestLogger();
            _restResponseFactory = new RestResponseFactory();
            _payloadSerializer = new UnitTestPayloadSerializer();
            _validGetRestRequest1.PathParameters.Add("ingredientId", "1");
            _validGetRestRequest2.PathParameters.Add("ingredientId", "2");
            _notfoundGetRestRequest.PathParameters.Add("ingredientId", "3");
            _validPutRestRequest.PathParameters.Add("ingredientId", "1");
            _invalidPutRestRequestMismatch400.PathParameters.Add("ingredientId", "2");
            _validDeleteRequest.PathParameters.Add("ingredientId", "1");
        }

        [TestCase(@"{""foo"":""bar""}")]
        [TestCase(@"{""name"":1,""description"":false}")]
        [TestCase(@"{""id"":1,""name"":""Vodka"",""description"":""vodka""")]
        public void PutShouldReturn400OnMalformedBody(string body)
        {
            var request = new RestRequest {Body = body, Method = HttpVerb.Put, InvokedPath = "/v1/ingredients/1"};
            request.PathParameters.Add("ingredientId", "1");
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.PutAsync(request, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(400));
            });
        }

        [Test]
        public void DeleteShouldReturn204Successfully()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.DeleteAsync(_validDeleteRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(204));
            });
        }

        [Test]
        public void GetShouldReturn200Successfully1()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.GetAsync(_validGetRestRequest1, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(200));
                Assert.That((Ingredient) response.Body, Is.EqualTo(_ingredientRepository.Vodka));
            });
        }

        [Test]
        public void GetShouldReturn200Successfully2()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.GetAsync(_validGetRestRequest2, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(200));
                Assert.That((Ingredient) response.Body, Is.EqualTo(_ingredientRepository.Whiskey));
            });
        }

        [Test]
        public void GetShouldReturn404()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.GetAsync(_notfoundGetRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(404));
            });
        }

        [Test]
        public void GetShouldReturn500OnRepositoryError()
        {
            var throwingIngredientRepository = new InMemoryIngredientRepository(true);
            var handler = new Handler(_restResponseFactory, _payloadSerializer, throwingIngredientRepository);
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await handler.GetAsync(_validGetRestRequest1, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(500));
            });
        }

        [Test]
        public void PutShouldReturn204Successfully1()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.PutAsync(_validPutRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(204));
            });
        }

        [Test]
        public void PutShouldReturn400OnBodyAndPathMismatch()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.PutAsync(_invalidPutRestRequestMismatch400, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(400));
            });
        }

        [Test]
        public void PutShouldReturn500OnRepositoryError()
        {
            var throwingIngredientRepository = new InMemoryIngredientRepository(true);
            var handler = new Handler(_restResponseFactory, _payloadSerializer, throwingIngredientRepository);
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await handler.PutAsync(_validPutRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(500));
            });
        }
    }
}