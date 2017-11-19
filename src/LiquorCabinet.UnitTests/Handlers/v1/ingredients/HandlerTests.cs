using System.Collections.Generic;
using LiquorCabinet.PathHandlers.v1.ingredients;
using LiquorCabinet.Repositories;
using LiquorCabinet.Repositories.Entities;
using LiquorCabinet.UnitTests.Repositories;
using NUnit.Framework;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Handlers.v1.ingredients
{
    [TestFixture]
    public class HandlerTests
    {
        #region Fields and SetUp

        private ILogger _logger;
        private IRestResponseFactory _restResponseFactory;
        private IPayloadSerializer _payloadSerializer;
        private Handler _handlerUnderTest;
        private ICrudRepository<Ingredient, int> _ingredientRepository;
        private RestRequest _validGetRequest;
        private RestRequest _validPostRequest;

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
            _ingredientRepository = new InMemoryIngredientRepository(false);
            _handlerUnderTest = new Handler(_restResponseFactory, _payloadSerializer, _ingredientRepository);
            _validGetRequest = new RestRequest { Body = string.Empty, InvokedPath = "/v1/ingredients", Method = HttpVerb.Get };
            _validPostRequest = new RestRequest
            {
                Body = "{\"name\":\"wine\",\"description\":\"fermented grape juice\"}",
                InvokedPath = "/v1/ingredients",
                Method = HttpVerb.Post
            };
            _validPostRequest.Headers.Add("Content-Type", "application/json");
        }

        #endregion

        #region POST

        [Test]
        public void PostShouldReturnSuccessfully()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.PostAsync(_validPostRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(201));
            });
        }

        [TestCase("{\"name\":\"wine\",\"description\":\"fermented grape juice\"")]
        [TestCase("{\"name\":\"wine\",\"desc\":\"fermented grape juice\"}")]
        [TestCase("{\"name\":\"wine\",\"description\":\"fermented grape juice\",\"foo\":\"bar\"}")]
        public void PostShouldReturn400WithBadBody(string body)
        {
            var request = _validPostRequest;
            request.Body = body;
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.PostAsync(request, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(400));
            });
        }

        #endregion

        #region GET

        [Test]
        public void GetReturns500OnRepositoryError()
        {
            var throwingIngredientRepository = new InMemoryIngredientRepository(true);
            _handlerUnderTest = new Handler(_restResponseFactory, _payloadSerializer, throwingIngredientRepository);

            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.GetAsync(_validGetRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(500));
            });
        }

        [Test]
        public void GetShouldReturnSuccessfully()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.GetAsync(_validGetRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(200));
                Assert.That(((List<Ingredient>) response.Body).Count, Is.EqualTo(2));
            });
        }

        #endregion
    }
}