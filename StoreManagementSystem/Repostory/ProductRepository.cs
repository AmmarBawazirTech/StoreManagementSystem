using Microsoft.AspNetCore.Mvc.Rendering;
using StoreManagementSystem.Datas;
using StoreManagementSystem.ViewModel;
using System.Linq.Expressions;

namespace StoreManagementSystem.Repostory
{
    
    
        public class ProductRepository : IRepository<Product>
        {
            private readonly StoreDbContext _context;
            private readonly ILogger<ProductRepository> _logger;

            public ProductRepository(StoreDbContext context, ILogger<ProductRepository> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Product?> GetByIdAsync(int id)
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == id);
            }

            public async Task<IEnumerable<Product>> GetAllAsync()
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.IsActive)
                    .ToListAsync();
            }

            public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate)
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .Where(predicate)
                    .Where(p => p.IsActive)
                    .ToListAsync();
            }

            public async Task<bool> ExistsAsync(Expression<Func<Product, bool>> predicate)
            {
                return await _context.Products
                    .Where(p => p.IsActive)
                    .AnyAsync(predicate);
            }

            public async Task AddAsync(Product entity)
            {
                await _context.Products.AddAsync(entity);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(Product entity)
            {
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var product = await GetByIdAsync(id);
                if (product != null)
                {
                    product.IsActive = false;
                    await UpdateAsync(product);
                }
            }

            public async Task<int> CountAsync(Expression<Func<Product, bool>>? predicate = null)
            {
                return predicate == null
                    ? await _context.Products.CountAsync(p => p.IsActive)
                    : await _context.Products.Where(predicate).CountAsync(p => p.IsActive);
            }

            // Additional product-specific methods
            public async Task<IEnumerable<ProductListViewModel>> GetProductListAsync(string searchTerm = "", int? categoryId = null)
            {
                var query = _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.IsActive);

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(p =>
                        p.Name.Contains(searchTerm) ||
                        (p.Description != null && p.Description.Contains(searchTerm)));
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(p => p.CategoryId == categoryId.Value);
                }

                return await query
                    .OrderBy(p => p.Name)
                    .Select(p => new ProductListViewModel
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        CategoryName = p.Category.Name,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        StockStatus = p.StockQuantity > 10 ? "In Stock" :
                                    p.StockQuantity > 0 ? "Low Stock" : "Out of Stock",
                        IsActive = p.IsActive
                    })
                    .ToListAsync();
            }

            public async Task<IEnumerable<SelectListItem>> GetProductSelectListAsync()
            {
                return await _context.Products
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.Name)
                    .Select(p => new SelectListItem
                    {
                        Value = p.ProductId.ToString(),
                        Text = $"{p.Name} (${p.Price}) - {p.StockQuantity} in stock"
                    })
                    .ToListAsync();
            }
        }
    }
}
