using System.Collections.Generic;
using LiquorCabinet.PathHandlers.v1.recipes;
using LiquorCabinet.Repositories.Entities;
using LiquorCabinet.UnitTests.Repositories;
using NUnit.Framework;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Handlers.v1.recipes
{
    [TestFixture]
    public class HandlerTests
    {
        [SetUp]
        public void SetUp()
        {
            _recipeRepository = new InMemoryRecipeRepository(false);
            _handlerUnderTest = new Handler(_restResponseFactory, _payloadSerializer, _recipeRepository);
        }

        private Handler _handlerUnderTest;
        private ILogger _logger;
        private IRestResponseFactory _restResponseFactory;
        private IPayloadSerializer _payloadSerializer;
        private InMemoryRecipeRepository _recipeRepository;

        private readonly RestRequest _validGetRestRequest = new RestRequest {Method = HttpVerb.Get, InvokedPath = "/v1/recipes"};

        private readonly RestRequest _validPostRestRequest = new RestRequest
        {
            Method = HttpVerb.Post,
            InvokedPath = "/v1/recipes",
            Body =
                "{\"name\":\"Clover Club\",\"instructions\":\"Pour all ingredients into cocktail shaker filled with ice. Shake well. Strain into cocktail glass.\",\"glasswareId\":4,\"components\":[{\"componentId\":33,\"quantityPart\":\"3 Parts\",\"quantityMetric\":4.5,\"quantityImperial\":1.5},{\"componentId\":66,\"quantityPart\":\"1 Part\",\"quantityMetric\":1.5,\"quantityImperial\":1},{\"componentId\":42,\"quantityPart\":\"1 Part\",\"quantityMetric\":1.5,\"quantityImperial\":1},{\"componentId\":28,\"quantityPart\":\"Few Drops\",\"quantityMetric\":null,\"quantityImperial\":null}]}"
        };

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _logger = new UnitTestLogger();
            _restResponseFactory = new RestResponseFactory();
            _payloadSerializer = new UnitTestPayloadSerializer();
        }

        [TestCase(
            "{\"name\":\"\",\"instructions\":\"Pour all ingredients into cocktail shaker filled with ice. Shake well. Strain into cocktail glass.\",\"glasswareId\":4,\"components\":[{\"componentId\":33,\"quantityPart\":\"3 Parts\",\"quantityMetric\":4.5,\"quantityImperial\":1.5},{\"componentId\":66,\"quantityPart\":\"1 Part\",\"quantityMetric\":1.5,\"quantityImperial\":1},{\"componentId\":42,\"quantityPart\":\"1 Part\",\"quantityMetric\":1.5,\"quantityImperial\":1},{\"componentId\":28,\"quantityPart\":\"Few Drops\",\"quantityMetric\":null,\"quantityImperial\":null}]}")]
        [TestCase("{\"foo\":\"bar\"")]
        [TestCase("{}")]
        [TestCase(
            "{\"name\":\"\",\"instructions\":\"Pour all ingredients into cocktail shaker filled with ice. Shake well. Strain into cocktail glass.\",\"glasswareId\":4,\"components\":[{\"componentId\":33,\"quantityPart\":\"3 Parts\",\"quantityMetric\":4.5,\"quantityImperial\":1.5},{\"componentId\":66,\"quantityPart\":\"1 Part\",\"quantityMetric\":1.5,\"quantityImperial\":1},{\"componentId\":42,\"quantityPart\":\"1 Part\",\"quantityMetric\":1.5,\"quantityImperial\":1},{\"componentId\":28,\"quantityPart\":\"\"]}")]
        [TestCase(
            "{\"name\":\"Clover Club\",\"instructions\":\"Pour all ingredients into cocktail shaker filled with ice. Shake well. Strain into cocktail glass.\",\"glasswareId\":4,\"components\":[{\"componentId\":33,\"quantityPart\":\"3 Parts\",\"quantityMetric\":4.5,\"quantityImperial\":1.5},{\"componentId\":66,\"quantityPart\":\"1 Part\",\"quantityMetric\":1.5,\"quantityImperial\":1},{\"componentId\":42,\"quantityPart\":\"1 Part\",\"quantityMetric\":1.5,\"quantityImperial\":1},{\"quantityPart\":\"Few Drops\",\"quantityMetric\":null,\"quantityImperial\":null}]}")]
        public void PostShouldReturn400BadRequest(string jsonSlob)
        {
            var request = new RestRequest {Method = HttpVerb.Post, InvokedPath = "/v1/recipes", Body = jsonSlob};
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.PostAsync(request, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(400));
            });
        }

        [Test]
        public void GetShouldReturn200Successfully()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.GetAsync(_validGetRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(200));
                Assert.That(((List<Recipe>) response.Body).Count, Is.EqualTo(2));
            });
        }

        [Test]
        public void GetShouldReturn500OnRepositoryError()
        {
            var throwingRepository = new InMemoryRecipeRepository(true);
            var handler = new Handler(_restResponseFactory, _payloadSerializer, throwingRepository);
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await handler.GetAsync(_validGetRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(500));
            });
        }

        [Test]
        public void PostShouldReturn201Successfully()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await _handlerUnderTest.PostAsync(_validPostRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(201));
                Assert.That(_recipeRepository.Recipes.Count, Is.EqualTo(3));
            });
        }

        [Test]
        public void PostShouldReturn500OnRepositoryError()
        {
            var throwingRecipeRepository = new InMemoryRecipeRepository(true);
            var handler = new Handler(_restResponseFactory, _payloadSerializer, throwingRecipeRepository);
            Assert.DoesNotThrowAsync(async () =>
            {
                var response = await handler.PostAsync(_validPostRestRequest, _logger);
                Assert.That(response.StatusCode, Is.EqualTo(500));
            });
        }
    }
}