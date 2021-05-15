using System;
using System.Threading.Tasks;
using Ingredients.Protos;
using Xunit;

namespace Ingredients.Tests
{
    public class IngredientsClientTests : IClassFixture<IngredientsApplicationFactory>
    {
        private readonly IngredientsApplicationFactory _factory;

        public IngredientsClientTests(IngredientsApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetsToppings()
        {
            var client = _factory.CreateGrpcClient();
            var response = await client.GetToppingsAsync(new GetToppingsRequest());
            
            Assert.Collection(response.Toppings,
                t => Assert.Equal("cheese", t.Id),
                t => Assert.Equal("tomato", t.Id));
        }

        [Fact]
        public async Task GetsCrusts()
        {
            var client = _factory.CreateGrpcClient();
            var response = await client.GetCrustsAsync(new GetCrustsRequest());
            
            Assert.Collection(response.Crusts,
                c => Assert.Equal("thin9", c.Id),
                c => Assert.Equal("deep9", c.Id));
        }
    }
}