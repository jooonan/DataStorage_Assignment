using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task AddProductAsync(ProductEntity product)
    {
        try
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while adding product: {ex.Message}");
            throw;
        }
    }

    public async Task<ProductEntity?> GetProductByIdAsync(int id)
    {
        try
        {
            return await _context.Products.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving product by ID ({id}): {ex.Message}");
            return null;
        }
    }

    public async Task<ProductEntity?> GetProductByNameAsync(string name)
    {
        try
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductName == name);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving product by name ({name}): {ex.Message}");
            return null;
        }
    }

    public async Task UpdateProductAsync(ProductEntity product)
    {
        try
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while updating product (ID: {product.Id}): {ex.Message}");
            throw;
        }
    }
}
