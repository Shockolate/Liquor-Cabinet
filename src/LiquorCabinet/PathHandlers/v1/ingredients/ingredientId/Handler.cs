using System;
using System.Threading.Tasks;
using LiquorCabinet.Repositories;
using LiquorCabinet.Repositories.Entities;
using Newtonsoft.Json;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.ingredients.ingredientId
{
    internal sealed class Handler : BaseHandler
    {
        private readonly ICrudRepository<Ingredient, int> _ingredientRepository;

        public Handler(IRestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer, ICrudRepository<Ingredient, int> ingredientRepository) :
            base(restResponseFactory, payloadSerializer)
        {
            _ingredientRepository = ingredientRepository;
            VerbHandlers.Add(HttpVerb.Get, GetAsync);
            VerbHandlers.Add(HttpVerb.Put, PutAsync);
            VerbHandlers.Add(HttpVerb.Delete, DeleteAsync);
        }


        public async Task<RestResponse> GetAsync(RestRequest request, ILogger logger)
        {
            try
            {
                if (!int.TryParse(request.PathParameters["ingredientId"], out var ingredientId))
                {
                    return RestResponseFactory.CreateErrorMessageRestResponse("Invalid IngredientId", 400);
                }

                var ingredient = await _ingredientRepository.GetAsync(ingredientId, logger);
                if (ingredient == default(Ingredient))
                {
                    return RestResponseFactory.CreateErrorMessageRestResponse($"Ingredient {ingredientId} Not Found.", 404);
                }
                var response = RestResponseFactory.CreateCorsRestResponse(200);
                response.Body = ingredient;
                return response;
            }
            catch (EntityNotFoundException e)
            {
                logger.LogError(() => e.Message);
                return RestResponseFactory.CreateErrorMessageRestResponse(e.Message, 404);
            }
            catch (Exception e)
            {
                logger.LogError(() => $"Repository Error: ${e.Message}");
                var response = RestResponseFactory.CreateErrorMessageRestResponse("Internal Server Error", 500);
                return response;
            }
        }

        public async Task<RestResponse> PutAsync(RestRequest request, ILogger logger)
        {
            try
            {
                if (!int.TryParse(request.PathParameters["ingredientId"], out var ingredientId))
                {
                    return RestResponseFactory.CreateErrorMessageRestResponse("Invalid IngredientId", 400);
                }

                var ingredient = PayloadSerializer.DeserializePayload<Ingredient>(request.Body);
                if (ingredient.Id != ingredientId)
                {
                    var errorMessage = $"Path IngredientId: {ingredientId} does not match Body Ingredient Id: {ingredient.Id}";
                    logger.LogDebug(() => errorMessage);
                    return RestResponseFactory.CreateErrorMessageRestResponse(errorMessage, 400);
                }
                await _ingredientRepository.UpdateAsync(ingredient, logger);
                var response = RestResponseFactory.CreateCorsRestResponse(204);
                return response;
            }
            catch (JsonException e)
            {
                logger.LogError(() => $"Malformed JSON Body: {e.Message}");
                return RestResponseFactory.CreateErrorMessageRestResponse("Malformed JSON Body.", 400);
            }
            catch (EntityNotFoundException e)
            {
                logger.LogError(() => e.Message);
                return RestResponseFactory.CreateErrorMessageRestResponse(e.Message, 404);
            }
            catch (Exception e)
            {
                logger.LogError(() => $"Repository Error: {e.Message}");
                return RestResponseFactory.CreateErrorMessageRestResponse("Internal Server Error.", 500);
            }
        }

        public async Task<RestResponse> DeleteAsync(RestRequest request, ILogger logger)
        {
            if (!int.TryParse(request.PathParameters["ingredientId"], out var ingredientId))
            {
                return RestResponseFactory.CreateErrorMessageRestResponse("Invalid IngredientId", 400);
            }
            try
            {
                logger.LogInfo((() => $"Deleting Ingredient: {ingredientId}."));
                await _ingredientRepository.DeleteAsync(ingredientId, logger);
                return RestResponseFactory.CreateCorsRestResponse(204);
            }
            catch (EntityNotFoundException e)
            {
                logger.LogError((() => e.Message));
                return RestResponseFactory.CreateErrorMessageRestResponse(e.Message, 404);
            }
            catch (Exception e)
            {
                logger.LogError((() => $"Repository Error: {e.Message}"));
                return RestResponseFactory.CreateErrorMessageRestResponse("Internal Server Error.", 500);
            }
        }
    }
}