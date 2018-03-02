using LiquorCabinet.Controllers.v1;
using LiquorCabinet.Models;
using LiquorCabinet.UnitTests.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LiquorCabinet.UnitTests.Controllers.v1
{
    [TestFixture]
    public class IngredientsControllerTests
    {
        private ILogger<IngredientsController> _logger;
        private InMemoryIngredientRepository _repository;
        private IngredientsController _controller;

        private IActionResult result = null;

        private Ingredient _validNewIngredient = new Ingredient
        {
            Name = "Tequila",
            Description = "Agave liquer"
        };

        private Ingredient _validUpdateIngredient = new Ingredient
        {
            Id = 2,
            Name = "Whiskey",
            Description = "BrownWater"
        };

        [OneTimeSetUp]
        public void Once()
        {
            _logger = UnitTestLoggerFactory.CreateLogger<IngredientsController>();
        }

        [SetUp]
        public void BeforeEach()
        {
            _repository = new InMemoryIngredientRepository();
            _controller = new IngredientsController(_logger, _repository);
            result = null;
        }

        [Test]
        public void CreateIngredientShouldReturn201Successfully()
        {
            Assert.DoesNotThrowAsync(async () => result = await _controller.CreateIngredient(_validNewIngredient).ConfigureAwait(false));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CreatedResult>(result);
            Assert.NotNull(((CreatedResult)result).Value);
            Assert.That(((Ingredient)((CreatedResult)result).Value).Name, Is.EqualTo("Tequila"));
            Assert.That(_repository.Ingredients.Count, Is.EqualTo(3));
        }

        [Test]
        public void CreateIngredientShouldReturn400OnNull()
        {
            Assert.DoesNotThrowAsync(async () => result = await _controller.CreateIngredient(null).ConfigureAwait(false));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void GetAllIngredientsReturns200Successfully()
        {
            Assert.DoesNotThrowAsync(async () => result = await _controller.GetAllIngredients().ConfigureAwait(false));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var list = okResult.Value as IEnumerable<Ingredient>;
            Assert.IsNotNull(list);
            Assert.That(list.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetIngredientReturns200Successfully()
        {
            const int vodkaId = 1;

            Assert.DoesNotThrowAsync(async () => result = await _controller.GetIngredient(vodkaId).ConfigureAwait(false));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var ingredient = okResult.Value as Ingredient;
            Assert.IsNotNull(ingredient);
            Assert.That(ingredient.Name, Is.EqualTo("Vodka"));
        }

        [Test]
        public void GetIngredientReturns404OnBadId()
        {
            const int badId = 42;

            Assert.DoesNotThrowAsync(async () => result = await _controller.GetIngredient(badId).ConfigureAwait(false));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void UpdateIngredientReturns204Successfully()
        {
            const int whiskeyId = 2;

            Assert.DoesNotThrowAsync(async () => result = await _controller.UpdateIngredient(whiskeyId, _validUpdateIngredient).ConfigureAwait(false));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);

            Assert.That(_repository.Ingredients[whiskeyId].Description, Is.EqualTo("BrownWater"));
        }
    }
}
