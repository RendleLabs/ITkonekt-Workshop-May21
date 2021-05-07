using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Pizza.Data;

namespace CreateData
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateCrusts();
            await CreateToppings();
        }

        private static async Task CreateCrusts()
        {
            var logger = new NullLogger<CrustData>();
            var data = new CrustData(logger);

            Console.Write("Adding crusts...");

            await data.AddAsync("thin9", "Thin", 9, 5d, 1000);
            await data.AddAsync("thin12", "Thin", 12, 7.50d, 1000);
            await data.AddAsync("thin15", "Thin", 15, 10d, 1000);
            await data.AddAsync("deep9", "Deep", 9, 6d, 1000);
            await data.AddAsync("deep12", "Deep", 12, 9d, 1000);
            await data.AddAsync("deep15", "Deep", 15, 12d, 1000);
            await data.AddAsync("stuffed12", "Stuffed", 12, 10d, 1000);
            await data.AddAsync("stuffed15", "Stuffed", 15, 14d, 1000);

            Console.WriteLine(" Done.");
        }

        private static async Task CreateToppings()
        {
            var logger = new NullLogger<ToppingData>();
            var data = new ToppingData(logger);

            Console.Write("Adding toppings...");

            await data.AddAsync("cheese", "Cheese", 0.5d, 10000);
            await data.AddAsync("sauce", "Tomato Sauce", 0.5d, 10000);
            await data.AddAsync("pepperoni", "Pepperoni", 1d, 1000);
            await data.AddAsync("ham", "Ham", 1d, 1000);
            await data.AddAsync("mushroom", "Mushrooms", 0.75d, 1000);
            await data.AddAsync("pineapple", "Pineapple", 2d, 1000);
            await data.AddAsync("anchovies", "Anchovies", 1d, 1000);
            await data.AddAsync("peppers", "Peppers", 0.75d, 1000);
            await data.AddAsync("onion", "Onion", 0.75d, 1000);
            await data.AddAsync("olives", "Olives", 1d, 1000);
            await data.AddAsync("beef", "Beef", 1d, 1000);

            Console.WriteLine(" Done.");
        }
    }
}
