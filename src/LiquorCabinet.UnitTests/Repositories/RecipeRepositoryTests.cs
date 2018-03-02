using System.Collections.Generic;
using System.Linq;
using LiquorCabinet.Models;
using LiquorCabinet.Repositories.Recipes;
using NUnit.Framework;

namespace LiquorCabinet.UnitTests.Repositories
{
    [TestFixture]
    public class RecipeRepositoryTests
    {
        private readonly IList<RecipeRow> _recipeRows = new List<RecipeRow>
        {
            new RecipeRow
            {
                RecipeId = 1,
                RecipeName = "Alexander",
                RecipeInstructions = "Make an Alexander",
                GlasswareId = 4,
                GlasswareName = "Cocktail Glass",
                GlasswareDescription = "A Cocktail Glass",
                GlasswareTypicalSize = "4 oz",
                RecipeComponentQuantityId = 292,
                ComponentId = 20,
                ComponentName = "Cognac",
                ComponentQuantityPart = "1 Part",
                ComponentQuantityMetric = 3.0,
                ComponentQuantityImperial = 1.0
            },
            new RecipeRow
            {
                RecipeId = 1,
                RecipeName = "Alexander",
                RecipeInstructions = "Make an Alexander",
                GlasswareId = 4,
                GlasswareName = "Cocktail Glass",
                GlasswareDescription = "A Cocktail Glass",
                GlasswareTypicalSize = "4 oz",
                RecipeComponentQuantityId = 293,
                ComponentId = 16,
                ComponentName = "Chocolate Liqueur",
                ComponentQuantityPart = "1 Part",
                ComponentQuantityMetric = 3.0,
                ComponentQuantityImperial = 1.0
            },
            new RecipeRow
            {
                RecipeId = 1,
                RecipeName = "Alexander",
                RecipeInstructions = "Make an Alexander",
                GlasswareId = 4,
                GlasswareName = "Cocktail Glass",
                GlasswareDescription = "A Cocktail Glass",
                GlasswareTypicalSize = "4 oz",
                RecipeComponentQuantityId = 294,
                ComponentId = 23,
                ComponentName = "Cream",
                ComponentQuantityPart = "1 Part",
                ComponentQuantityMetric = 3.0,
                ComponentQuantityImperial = 1.0
            },
            new RecipeRow
            {
                RecipeId = 2,
                RecipeName = "Americano",
                RecipeInstructions = "Make an Americano",
                GlasswareId = 12,
                GlasswareName = "Old fashioned",
                GlasswareDescription = "A Tumbler",
                GlasswareTypicalSize = "8-10oz",
                RecipeComponentQuantityId = 295,
                ComponentId = 13,
                ComponentName = "Campari",
                ComponentQuantityPart = "1 Part",
                ComponentQuantityMetric = 3.0,
                ComponentQuantityImperial = 1.5
            },
            new RecipeRow
            {
                RecipeId = 2,
                RecipeName = "Americano",
                RecipeInstructions = "Make an Americano",
                GlasswareId = 12,
                GlasswareName = "Old fashioned",
                GlasswareDescription = "A Tumbler",
                GlasswareTypicalSize = "8-10oz",
                RecipeComponentQuantityId = 296,
                ComponentId = 76,
                ComponentName = "Sweet Vermouth",
                ComponentQuantityPart = "1 Part",
                ComponentQuantityMetric = 3.0,
                ComponentQuantityImperial = 1.5
            },
            new RecipeRow
            {
                RecipeId = 2,
                RecipeName = "Americano",
                RecipeInstructions = "Make an Americano",
                GlasswareId = 12,
                GlasswareName = "Old fashioned",
                GlasswareDescription = "A Tumbler",
                GlasswareTypicalSize = "8-10oz",
                RecipeComponentQuantityId = 297,
                ComponentId = 70,
                ComponentName = "Soda",
                ComponentQuantityPart = "Splash",
                ComponentQuantityMetric = null,
                ComponentQuantityImperial = null
            }
        };

        [Test]
        public void ConvertRowsToRecipesShouldSucceedWith2Recipes()
        {
            var recipes = RecipeRepository.ConvertRecipeRowsToRecipes(_recipeRows);
            var recipeArray = recipes as Recipe[] ?? recipes.ToArray();
            Assert.That(recipeArray.Length, Is.EqualTo(2));
            var alexander = recipeArray.Single(r => r.Id == 1);

            Assert.That(alexander.Name, Is.EqualTo("Alexander"));
            Assert.That(alexander.Instructions, Is.EqualTo("Make an Alexander"));
            Assert.That(alexander.Glassware.Id, Is.EqualTo(4));
            Assert.That(alexander.Glassware.Name, Is.EqualTo("Cocktail Glass"));
            var alexanderComponents = alexander.Components.ToArray();
            Assert.That(alexanderComponents.Length, Is.EqualTo(3));

            Assert.That(alexanderComponents[0].Id, Is.EqualTo(292));
            Assert.That(alexanderComponents[0].ComponentId, Is.EqualTo(20));
            Assert.That(alexanderComponents[0].ComponentName, Is.EqualTo("Cognac"));
            Assert.That(alexanderComponents[0].RecipeId, Is.EqualTo(1));
            Assert.That(alexanderComponents[0].QuantityPart, Is.EqualTo("1 Part"));
            Assert.That(alexanderComponents[0].QuantityMetric, Is.EqualTo(3.0));
            Assert.That(alexanderComponents[0].QuantityImperial, Is.EqualTo(1.0));

            Assert.That(alexanderComponents[1].Id, Is.EqualTo(293));
            Assert.That(alexanderComponents[1].ComponentId, Is.EqualTo(16));
            Assert.That(alexanderComponents[1].ComponentName, Is.EqualTo("Chocolate Liqueur"));
            Assert.That(alexanderComponents[1].RecipeId, Is.EqualTo(1));
            Assert.That(alexanderComponents[1].QuantityPart, Is.EqualTo("1 Part"));
            Assert.That(alexanderComponents[1].QuantityMetric, Is.EqualTo(3.0));
            Assert.That(alexanderComponents[1].QuantityImperial, Is.EqualTo(1.0));

            Assert.That(alexanderComponents[2].Id, Is.EqualTo(294));
            Assert.That(alexanderComponents[2].ComponentId, Is.EqualTo(23));
            Assert.That(alexanderComponents[2].ComponentName, Is.EqualTo("Cream"));
            Assert.That(alexanderComponents[2].RecipeId, Is.EqualTo(1));
            Assert.That(alexanderComponents[2].QuantityPart, Is.EqualTo("1 Part"));
            Assert.That(alexanderComponents[2].QuantityMetric, Is.EqualTo(3.0));
            Assert.That(alexanderComponents[2].QuantityImperial, Is.EqualTo(1.0));

            var americano = recipeArray.Single(r => r.Id == 2);
            Assert.That(americano.Name, Is.EqualTo("Americano"));
            Assert.That(americano.Instructions, Is.EqualTo("Make an Americano"));
            Assert.That(americano.Glassware.Id, Is.EqualTo(12));
            Assert.That(americano.Glassware.Name, Is.EqualTo("Old fashioned"));
            var americanoComponents = americano.Components.ToArray();
            Assert.That(americanoComponents.Length, Is.EqualTo(3));

            Assert.That(americanoComponents[0].Id, Is.EqualTo(295));
            Assert.That(americanoComponents[0].ComponentId, Is.EqualTo(13));
            Assert.That(americanoComponents[0].ComponentName, Is.EqualTo("Campari"));
            Assert.That(americanoComponents[0].RecipeId, Is.EqualTo(2));
            Assert.That(americanoComponents[0].QuantityPart, Is.EqualTo("1 Part"));
            Assert.That(americanoComponents[0].QuantityMetric, Is.EqualTo(3.0));
            Assert.That(americanoComponents[0].QuantityImperial, Is.EqualTo(1.5));

            Assert.That(americanoComponents[1].Id, Is.EqualTo(296));
            Assert.That(americanoComponents[1].ComponentId, Is.EqualTo(76));
            Assert.That(americanoComponents[1].ComponentName, Is.EqualTo("Sweet Vermouth"));
            Assert.That(americanoComponents[1].RecipeId, Is.EqualTo(2));
            Assert.That(americanoComponents[1].QuantityPart, Is.EqualTo("1 Part"));
            Assert.That(americanoComponents[1].QuantityMetric, Is.EqualTo(3.0));
            Assert.That(americanoComponents[1].QuantityImperial, Is.EqualTo(1.5));

            Assert.That(americanoComponents[2].Id, Is.EqualTo(297));
            Assert.That(americanoComponents[2].ComponentId, Is.EqualTo(70));
            Assert.That(americanoComponents[2].ComponentName, Is.EqualTo("Soda"));
            Assert.That(americanoComponents[2].RecipeId, Is.EqualTo(2));
            Assert.That(americanoComponents[2].QuantityPart, Is.EqualTo("Splash"));
            Assert.That(americanoComponents[2].QuantityMetric, Is.Null);
            Assert.That(americanoComponents[2].QuantityImperial, Is.Null);
        }
    }
}