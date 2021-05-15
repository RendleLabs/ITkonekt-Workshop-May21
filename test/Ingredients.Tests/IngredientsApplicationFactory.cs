using System.Collections.Generic;
using System.Threading;
using Ingredients.Protos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Pizza.Data;
using TestHelpers;

namespace Ingredients.Tests
{
    public class IngredientsApplicationFactory : WebApplicationFactory<Startup>
    {
        public IngredientsService.IngredientsServiceClient CreateGrpcClient()
        {
            var channel = this.CreateGrpcChannel();
            return new IngredientsService.IngredientsServiceClient(channel);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IToppingData>();

                var toppingEntities = new List<ToppingEntity>
                {
                    new ToppingEntity("cheese", "Cheese", 0.5d, 10),
                    new ToppingEntity("tomato", "Tomato", 1d, 10),
                };

                var toppingSub = Substitute.For<IToppingData>();
                toppingSub.GetAsync(Arg.Any<CancellationToken>())
                    .Returns(toppingEntities);

                services.AddSingleton(toppingSub);

                services.RemoveAll<ICrustData>();

                var crustEntities = new List<CrustEntity>
                {
                    new CrustEntity("thin9", "Thin", 9, 5d, 10),
                    new CrustEntity("deep9", "Deep", 9, 6d, 10),
                };

                var crustSub = Substitute.For<ICrustData>();
                crustSub.GetAsync(Arg.Any<CancellationToken>())
                    .Returns(crustEntities);

                services.AddSingleton(crustSub);
            });
            base.ConfigureWebHost(builder);
        }
    }
}