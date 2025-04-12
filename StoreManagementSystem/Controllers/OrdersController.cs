using Microsoft.AspNetCore.Mvc;
using StoreManagementSystem.Repostory;

namespace StoreManagementSystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;

        public OrdersController(OrderRepository orderRepository, ProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _productRepository.GetAllActiveProductsAsync();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var orderId = await _orderRepository.ProcessOrderAsync(
                        model.CustomerId,
                        model.EmployeeId,
                        model.Items);

                    return RedirectToAction("Details", new { id = orderId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error processing order: " + ex.Message);
                }
            }

            ViewBag.Products = await _productRepository.GetAllActiveProductsAsync();
            return View(model);
        }
    }

}
