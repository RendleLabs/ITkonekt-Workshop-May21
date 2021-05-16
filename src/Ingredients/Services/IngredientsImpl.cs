using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Ingredients.Protos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Pizza.Data;

namespace Ingredients
{
    public class IngredientsImpl : IngredientsService.IngredientsServiceBase
    {
        private readonly IToppingData _toppingData;
        private readonly ICrustData _crustData;
        private readonly ILogger<IngredientsImpl> _logger;

        public IngredientsImpl(IToppingData toppingData, ICrustData crustData, ILogger<IngredientsImpl> logger)
        {
            _toppingData = toppingData;
            _crustData = crustData;
            _logger = logger;
        }

        public override async Task<GetToppingsResponse> GetToppings(GetToppingsRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetToppings");
            try
            {
                var toppings = await _toppingData.GetAsync(context.CancellationToken);
                var response = new GetToppingsResponse
                {
                    Toppings =
                    {
                        toppings.Select(t => new Topping
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Price = t.Price
                        })
                    }
                };
                return response;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operation was cancelled.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: {ex.Message}");
                throw;
            }
        }

        public override async Task<DecrementToppingsResponse> DecrementToppings(DecrementToppingsRequest request, ServerCallContext context)
        {
            foreach (var toppingId in request.ToppingIds)
            {
                await _toppingData.DecrementStockAsync(toppingId, context.CancellationToken);
            }

            return new DecrementToppingsResponse();
        }

        public override async Task<GetCrustsResponse> GetCrusts(GetCrustsRequest request, ServerCallContext context)
        {
            try
            {
                var crusts = await _crustData.GetAsync(context.CancellationToken);
                var response = new GetCrustsResponse
                {
                    Crusts =
                    {
                        crusts.Select(c => new Crust
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Size = c.Size,
                            Price = c.Price
                        })
                    }
                };
                return response;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operation was cancelled.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: {ex.Message}");
                throw;
            }
        }

        public override async Task<DecrementCrustsResponse> DecrementCrusts(DecrementCrustsRequest request, ServerCallContext context)
        {
            await _crustData.DecrementStockAsync(request.CrustId);
            return new DecrementCrustsResponse();
        }
    }
}