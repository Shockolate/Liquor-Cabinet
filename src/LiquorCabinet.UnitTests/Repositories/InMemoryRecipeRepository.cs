using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiquorCabinet.Repositories.Entities;
using LiquorCabinet.Repositories.Recipes;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.UnitTests.Repositories
{
    internal class InMemoryRecipeRepository : BaseInMemoryRepository<Recipe, int>, IRecipeRepository
    {
        private readonly Recipe _alexander = new Recipe
        {
            Id = 1,
            Name = "Alexander",
            Instructions = "Make an Alexander",
            Glassware = new Glass {Id = 4, Description = "A Cocktail Glass", Name = "Cocktail Glass", TypicalSize = "4 oz"},
            Components = new List<RecipeComponent>
            {
                new RecipeComponent
                {
                    Id = 292,
                    ComponentId = 20,
                    ComponentName = "Cognac",
                    RecipeId = 1,
                    QuantityPart = "1 Part",
                    QuantityMetric = 3.0,
                    QuantityImperial = 1.0
                },
                new RecipeComponent
                {
                    Id = 293,
                    ComponentId = 16,
                    ComponentName = "Chocolate Liqueur",
                    RecipeId = 1,
                    QuantityPart = "1 Part",
                    QuantityMetric = 3.0,
                    QuantityImperial = 1.0
                },
                new RecipeComponent
                {
                    Id = 294,
                    ComponentId = 23,
                    ComponentName = "Cream",
                    RecipeId = 1,
                    QuantityPart = "1 Part",
                    QuantityMetric = 3.0,
                    QuantityImperial = 1.0
                }
            }
        };

        private readonly Recipe _americano = new Recipe
        {
            Id = 2,
            Name = "Americano",
            Instructions = "Make an Americano",
            Glassware = new Glass {Id = 12, Name = "Old fashioned", Description = "A rocks glass", TypicalSize = "8-10oz"},
            Components = new List<RecipeComponent>
            {
                new RecipeComponent
                {
                    Id = 295,
                    ComponentId = 13,
                    ComponentName = "Campari",
                    RecipeId = 2,
                    QuantityPart = "1 Part",
                    QuantityMetric = 3.0,
                    QuantityImperial = 1.5
                },
                new RecipeComponent
                {
                    Id = 296,
                    ComponentId = 76,
                    ComponentName = "Sweet Vermouth",
                    QuantityPart = "1 Part",
                    RecipeId = 2,
                    QuantityMetric = 3.0,
                    QuantityImperial = 1.5
                },
                new RecipeComponent
                {
                    Id = 297,
                    ComponentId = 70,
                    ComponentName = "Soda",
                    RecipeId = 2,
                    QuantityPart = "Splash",
                    QuantityMetric = null,
                    QuantityImperial = null
                }
            }
        };

        public readonly List<Recipe> Recipes;

        public InMemoryRecipeRepository() : this(false) { }

        public InMemoryRecipeRepository(bool throws) : base(throws)
        {
            Recipes = new List<Recipe> {_alexander, _americano};
        }

        public IEnumerable<Recipe> GetRecipeListForUserAsync(int userId, ILogger logger) => throw new NotImplementedException();

        protected override Task DoInsertAsync(Recipe entityToCreate, ILogger logger)
        {
            Recipes.Add(entityToCreate);
            return Task.CompletedTask;
        }

        protected override Task<Recipe> DoGetAsync(int id, ILogger logger) => throw new NotImplementedException();

        protected override Task<IEnumerable<Recipe>> DoGetListAsync(ILogger logger) => Task.FromResult(Recipes.AsEnumerable());

        protected override Task DoUpdateAsync(Recipe entityToUpdate, ILogger logger) => throw new NotImplementedException();

        protected override Task DoDeleteAsync(int id, ILogger logger) => throw new NotImplementedException();
    }
}