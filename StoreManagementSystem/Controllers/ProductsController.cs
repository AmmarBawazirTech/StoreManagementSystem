using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreManagementSystem.Models;
using StoreManagementSystem.Repostory;
using StoreManagementSystem.ViewModel;

namespace StoreManagementSystem.Controllers
{
    public class ProductsController : Controller
    {
        [Authorize]
        public class ProductsController : Controller
        {
            private readonly IRepository<Product> _productRepository;
            private readonly IRepository<Category> _categoryRepository;
            private readonly ILogger<ProductsController> _logger;

            public ProductsController(
                IRepository<Product> productRepository,
                IRepository<Category> categoryRepository,
                ILogger<ProductsController> logger)
            {
                _productRepository = productRepository;
                _categoryRepository = categoryRepository;
                _logger = logger;
            }

            // GET: Products
            public async Task<IActionResult> Index(string searchTerm = "", int? categoryId = null)
            {
                try
                {
                    ViewBag.SearchTerm = searchTerm;
                    ViewBag.CategoryId = categoryId;

                    // Get categories for dropdown
                    var categories = await _categoryRepository.GetAllAsync();
                    ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");

                    // Get filtered products
                    var products = await ((ProductRepository)_productRepository)
                        .GetProductListAsync(searchTerm, categoryId);

                    return View(products);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading product index");
                    TempData["ErrorMessage"] = "An error occurred while loading products.";
                    return RedirectToAction("Index", "Home");
                }
            }

            // GET: Products/Create
            public async Task<IActionResult> Create()
            {
                try
                {
                    var categories = await _categoryRepository.GetAllAsync();
                    var viewModel = new ProductCreateEditViewModel
                    {
                        CategorySelectList = new SelectList(categories, "CategoryId", "Name")
                    };
                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading product create form");
                    TempData["ErrorMessage"] = "An error occurred while loading the create form.";
                    return RedirectToAction(nameof(Index));
                }
            }

            // POST: Products/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(ProductCreateEditViewModel viewModel)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var product = new Product
                        {
                            Name = viewModel.Name,
                            Description = viewModel.Description,
                            Price = viewModel.Price,
                            StockQuantity = viewModel.StockQuantity,
                            CategoryId = viewModel.CategoryId,
                            IsActive = viewModel.IsActive
                        };

                        await _productRepository.AddAsync(product);

                        TempData["SuccessMessage"] = "Product created successfully!";
                        return RedirectToAction(nameof(Index));
                    }

                    // Reload categories if model is invalid
                    var categories = await _categoryRepository.GetAllAsync();
                    viewModel.CategorySelectList = new SelectList(categories, "CategoryId", "Name");

                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating new product");
                    TempData["ErrorMessage"] = "An error occurred while creating the product.";
                    return RedirectToAction(nameof(Index));
                }
            }

            // GET: Products/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                try
                {
                    var product = await _productRepository.GetByIdAsync(id.Value);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    var categories = await _categoryRepository.GetAllAsync();
                    var viewModel = new ProductCreateEditViewModel
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        StockQuantity = product.StockQuantity,
                        CategoryId = product.CategoryId,
                        IsActive = product.IsActive,
                        CategorySelectList = new SelectList(categories, "CategoryId", "Name", product.CategoryId)
                    };

                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error loading edit form for product ID {id}");
                    TempData["ErrorMessage"] = "An error occurred while loading the product for editing.";
                    return RedirectToAction(nameof(Index));
                }
            }

            // POST: Products/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, ProductCreateEditViewModel viewModel)
            {
                if (id != viewModel.ProductId)
                {
                    return NotFound();
                }

                try
                {
                    if (ModelState.IsValid)
                    {
                        var product = await _productRepository.GetByIdAsync(id);
                        if (product == null)
                        {
                            return NotFound();
                        }

                        product.Name = viewModel.Name;
                        product.Description = viewModel.Description;
                        product.Price = viewModel.Price;
                        product.StockQuantity = viewModel.StockQuantity;
                        product.CategoryId = viewModel.CategoryId;
                        product.IsActive = viewModel.IsActive;

                        await _productRepository.UpdateAsync(product);

                        TempData["SuccessMessage"] = "Product updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }

                    // Reload categories if model is invalid
                    var categories = await _categoryRepository.GetAllAsync();
                    viewModel.CategorySelectList = new SelectList(categories, "CategoryId", "Name", viewModel.CategoryId);

                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating product ID {id}");
                    TempData["ErrorMessage"] = "An error occurred while updating the product.";
                    return RedirectToAction(nameof(Index));
                }
            }

            // POST: Products/Delete/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Delete(int id)
            {
                try
                {
                    await _productRepository.DeleteAsync(id);
                    TempData["SuccessMessage"] = "Product deleted successfully!";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error deleting product ID {id}");
                    TempData["ErrorMessage"] = "An error occurred while deleting the product.";
                }

                return RedirectToAction(nameof(Index));
            }
        }
    }


}
