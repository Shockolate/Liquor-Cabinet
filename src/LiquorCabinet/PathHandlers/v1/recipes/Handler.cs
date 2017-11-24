using System;
using System.Linq;
using System.Threading.Tasks;
using LiquorCabinet.Repositories.Entities;
using LiquorCabinet.Repositories.Recipes;
using Newtonsoft.Json;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.recipes
{
    internal sealed class Handler : BaseHandler
    {
        private readonly IRecipeRepository _recipeRepository;

        public Handler(IRestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer, IRecipeRepository recipeRepository) : base(
            restResponseFactory, payloadSerializer)
        {
            _recipeRepository = recipeRepository;
            VerbHandlers.Add(HttpVerb.Get, GetAsync);
            VerbHandlers.Add(HttpVerb.Post, PostAsync);
        }

        public async Task<RestResponse> GetAsync(RestRequest request, ILogger logger)
        {
            if (request.QueryStringParameters.ContainsKey("userId"))
            {
                throw new NotImplementedException();
            }

            try
            {
                logger.LogDebug(() => "Getting All Recipes.");
                var recipes = await _recipeRepository.GetListAsync(logger);
                var response = RestResponseFactory.CreateCorsRestResponse(200);
                response.Body = recipes;
                return response;
            }
            catch (Exception e)
            {
                logger.LogError(() => $"Repository Error {e.Message}");
                return RestResponseFactory.CreateErrorMessageRestResponse("Internal Server Error.", 500);
            }
        }

        public async Task<RestResponse> PostAsync(RestRequest request, ILogger logger)
        {
            Recipe recipe;
            try
            {
                var newRecipe = PayloadSerializer.DeserializePayload<NewRecipe>(request.Body);
                ValidateNewRecipe(newRecipe);
                recipe = ConvertNewRecipeToRecipe(newRecipe);
            }
            catch (ArgumentException e)
            {
                var errorMessage = $"Error with Recipe Body: {e.Message}";
                return RestResponseFactory.CreateErrorMessageRestResponse(errorMessage, 400);
            }
            catch (JsonException e)
            {
                logger.LogError(() => $"Error deserializing body: {e.Message}");
                return RestResponseFactory.CreateErrorMessageRestResponse("Malformed Body.", 400);
            }

            try
            {
                logger.LogInfo(() => $"Persisting new Recipe: {recipe.Name}");
                await _recipeRepository.InsertAsync(recipe, logger);
                return RestResponseFactory.CreateCorsRestResponse(201);
            }
            catch (Exception e)
            {
                logger.LogError(() => $"Persistence Error: {e.Message}");
                return RestResponseFactory.CreateErrorMessageRestResponse("Internal Server Error.", 500);
            }
        }

        internal static void ValidateNewRecipe(NewRecipe newRecipe)
        {
            if (string.IsNullOrEmpty(newRecipe.Name))
            {
                throw new ArgumentException("Recipe Name must not be empty");
            }

            if (newRecipe.Components.Any(c => c.ComponentId == default(int) || string.IsNullOrEmpty(c.QuantityPart)))
            {
                throw new ArgumentException("Invalid Recipe Component");
            }
        }

        internal static Recipe ConvertNewRecipeToRecipe(NewRecipe newRecipe)
        {
            return new Recipe
            {
                Name = newRecipe.Name,
                Instructions = newRecipe.Instructions,
                Glassware = new Glass {Id = newRecipe.GlasswareId},
                Components = newRecipe.Components.Select(c => new RecipeComponent
                {
                    ComponentId = c.ComponentId,
                    QuantityPart = c.QuantityPart,
                    QuantityImperial = c.QuantityImperial,
                    QuantityMetric = c.QuantityMetric
                }).ToList()
            };
        }
    }
}