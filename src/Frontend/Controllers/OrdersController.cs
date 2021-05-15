using System.Linq;
using System.Threading.Tasks;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.Protos;

namespace Frontend.Controllers
{
    [Route("orders")]
    public class OrdersController : Controller
    {
        private readonly OrderService.OrderServiceClient _orders;
        private readonly ILogger<OrdersController> _log;

        public OrdersController(OrderService.OrderServiceClient orders, ILogger<OrdersController> log)
        {
            _orders = orders;
            _log = log;
        }

        [HttpPost]
        public async Task<IActionResult> Order([FromForm]HomeViewModel viewModel)
        {
            var request = new PlaceOrderRequest
            {
                CrustId = viewModel.SelectedCrust,
                ToppingIds =
                {
                    viewModel.Toppings
                        .Where(t => t.Selected)
                        .Select(t => t.Id)
                }
            };
            await _orders.PlaceOrderAsync(request);
            return View();
        }
    }
}