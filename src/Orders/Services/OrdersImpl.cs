using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ingredients.Protos;
using Orders.Protos;

namespace Orders.Services
{
    public class OrdersImpl : OrderService.OrderServiceBase
    {
        private readonly IngredientsService.IngredientsServiceClient _ingredients;

        public OrdersImpl(IngredientsService.IngredientsServiceClient ingredients)
        {
            _ingredients = ingredients;
        }

        public override async Task<PlaceOrderResponse> PlaceOrder(PlaceOrderRequest request, ServerCallContext context)
        {
            var decrementToppingsRequest = new DecrementToppingsRequest
            {
                ToppingIds = {request.ToppingIds}
            };
            await _ingredients.DecrementToppingsAsync(decrementToppingsRequest);

            var decrementCrustsRequest = new DecrementCrustsRequest
            {
                CrustId = request.CrustId
            };
            await _ingredients.DecrementCrustsAsync(decrementCrustsRequest);

            var time = DateTimeOffset.UtcNow.AddHours(0.5d);

            return new PlaceOrderResponse
            {
                Time = time.ToTimestamp()
            };
        }
    }
}