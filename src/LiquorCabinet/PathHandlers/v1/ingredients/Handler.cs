using System;
using System.Threading.Tasks;
using LiquorCabinet.Repositories;
using LiquorCabinet.Repositories.Entities;
using RestfulMicroserverless.Contracts;

namespace LiquorCabinet.PathHandlers.v1.ingredients
{
    internal sealed class Handler : BaseHandler
    {
        private readonly ICrudRepository<Ingredient, int> _ingredientRepository;

        public Handler(IRestResponseFactory restResponseFactory, IPayloadSerializer payloadSerializer, ICrudRepository<Ingredient, int> ingredientRepository) :
            base(restResponseFactory, payloadSerializer)
        {
            _ingredientRepository = ingredientRepository;
            VerbHandlers.Add(HttpVerb.Post, PostAsync);
            VerbHandlers.Add(HttpVerb.Get, GetAsync);
        }

        public async Task<RestResponse> PostAsync(RestRequest request, ILogger logger)
        {
            Ingredient ingredient;
            try
            {
                ingredient = PayloadSerializer.DeserializePayload<Ingredient>(request.Body);
            }
            catch (Exception e)
            {
                logger.LogError(() => $"Error deserializing Post Event. {e.Message}");
                return RestResponseFactory.CreateErrorMessageRestResponse("Malformed Body.", 400);
            }

            try
            {
                await _ingredientRepository.InsertAsync(ingredient, logger).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                logger.LogError(() => $"Repository Error: {e.Message}");
                return RestResponseFactory.CreateErrorMessageRestResponse("Internal Server Error.", 500);
            }
            return RestResponseFactory.CreateCorsRestResponse(201);
        }

        public async Task<RestResponse> GetAsync(RestRequest request, ILogger logger)
        {
            try
            {
                var response = RestResponseFactory.CreateCorsRestResponse(200);
                response.Body = await _ingredientRepository.GetListAsync(logger);
                return response;
            }
            catch (Exception e)
            {
                logger.LogError(() => $"Repository Error: ${e.Message}");
                var response = RestResponseFactory.CreateErrorMessageRestResponse("Internal Server Error", 500);
                return response;
            }
        }
    }
}