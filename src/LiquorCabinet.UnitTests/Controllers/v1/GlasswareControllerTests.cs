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
    public class GlasswareControllerTests
    {
        private ILogger<GlasswareController> _logger;
        private GlasswareController _controller;
        private InMemoryGlasswareRepository _glasswareRepository;

        private IActionResult result = null;

        private Glass _validNewGlass = new Glass
        {
            Name = "Shot Glass",
            Description = "A Glass to shoot with",
            TypicalSize = "Enough for a shot"
        };

        [OneTimeSetUp]
        public void Once()
        {
            _logger = UnitTestLoggerFactory.CreateLogger<GlasswareController>();
        }

        [SetUp]
        public void BeforeEach()
        {
            _glasswareRepository = new InMemoryGlasswareRepository();
            _controller = new GlasswareController(_logger, _glasswareRepository);
            result = null;
        }

        [Test]
        public void GetAllGlasswareReturns200Successfully()
        {
            Assert.DoesNotThrowAsync(async () => result = await _controller.GetAllGlassware().ConfigureAwait(false));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOf<IEnumerable<Glass>>(okResult.Value);
            var glasses = okResult.Value as IEnumerable<Glass>;
            Assert.IsNotNull(glasses);
            Assert.AreEqual(2, glasses.ToList().Count);
        }

        [Test]
        public void CreateNewGlassReturns201Successfully()
        {
            var startingGlasswareCount = _glasswareRepository.Glasses.Count;

            Assert.DoesNotThrowAsync(async () => result = await _controller.CreateNewGlass(_validNewGlass).ConfigureAwait(false));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CreatedResult>(result);
            Assert.That(_glasswareRepository.Glasses.Count, Is.EqualTo(startingGlasswareCount + 1));
        }

        [Test]
        public void CreateNewGlassReturns400OnNull()
        {
            Assert.DoesNotThrowAsync(async () => result = await _controller.CreateNewGlass(null).ConfigureAwait(false));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestResult>(result);
         }

        [Test]
        public void GetGlassReturns200Successfully()
        {
            const int cocktailglassId = 1;

            Assert.DoesNotThrowAsync(async () => result = await _controller.GetGlass(cocktailglassId).ConfigureAwait(false));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var value = okObjectResult.Value;
            Assert.IsNotNull(value);
            Assert.IsInstanceOf<Glass>(value);
            var glass = value as Glass;
            Assert.IsNotNull(glass);
            Assert.That(glass.Name, Is.EqualTo("Cocktail Glass"));
        }

        [Test]
        public void GetGlassReturns404OnMissingId()
        {
            const int badGlassId = 42;

            Assert.DoesNotThrowAsync(async () => result = await _controller.GetGlass(badGlassId).ConfigureAwait(false));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}
