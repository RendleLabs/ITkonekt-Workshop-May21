using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Frontend.Models;
using Ingredients.Protos;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ActivitySource Activities = new(nameof(HomeController));
        
        private readonly IngredientsService.IngredientsServiceClient _ingredients;
        private readonly ILogger<HomeController> _log;

        public HomeController(IngredientsService.IngredientsServiceClient ingredients, ILogger<HomeController> log)
        {
            _ingredients = ingredients;
            _log = log;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<ToppingViewModel> toppings;
            List<CrustViewModel> crusts;
            
            using (var activity = Activities.StartActivity("GetIngredients"))
            {
                activity?.AddTag("User", User.Identity.Name ?? "Anonymous");
                var toppingsResponse = await _ingredients.GetToppingsAsync(new GetToppingsRequest());

                toppings = toppingsResponse.Toppings
                    .Select(t => new ToppingViewModel(t.Id, t.Name, Convert.ToDecimal(t.Price)))
                    .ToList();

                var crustsResponse = await _ingredients.GetCrustsAsync(new GetCrustsRequest());

                crusts = crustsResponse.Crusts
                    .Select(c => new CrustViewModel(c.Id, c.Name, c.Size, Convert.ToDecimal(c.Price)))
                    .ToList();
            }

            var viewModel = new HomeViewModel(toppings, crusts);
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
