using Business.Dtos;

namespace Business.Interfaces;

public interface IProductService
{
    Task<ProductDto> AddProductAsync(string productName, decimal price);
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto?> GetProductByNameAsync(string name);
    Task UpdateProductAsync(ProductDto productDto);
}