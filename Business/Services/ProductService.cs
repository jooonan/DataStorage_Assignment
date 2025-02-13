using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Data.Repositories;

namespace Business.Services;

public class ProductService(ProductRepository productRepository) : IProductService
{
    private readonly ProductRepository _productRepository = productRepository;

    public async Task<ProductDto> AddProductAsync(string productName, decimal price)
    {
        var existingProduct = await _productRepository.GetProductByNameAsync(productName);
        if (existingProduct != null)
        {
            return new ProductDto
            {
                Id = existingProduct.Id,
                ProductName = existingProduct.ProductName,
                Price = existingProduct.Price
            };
        }

        var newProduct = new ProductEntity
        {
            ProductName = productName,
            Price = price
        };

        await _productRepository.AddProductAsync(newProduct);

        return new ProductDto
        {
            Id = newProduct.Id,
            ProductName = newProduct.ProductName,
            Price = newProduct.Price
        };
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null) return null;

        return new ProductDto
        {
            Id = product.Id,
            ProductName = product.ProductName,
            Price = product.Price
        };
    }

    public async Task<ProductDto?> GetProductByNameAsync(string name)
    {
        var product = await _productRepository.GetProductByNameAsync(name);
        if (product == null) return null;

        return new ProductDto
        {
            Id = product.Id,
            ProductName = product.ProductName,
            Price = product.Price
        };
    }

    public async Task UpdateProductAsync(ProductDto productDto)
    {
        var existingProduct = await _productRepository.GetProductByIdAsync(productDto.Id);
        if (existingProduct != null)
        {
            existingProduct.ProductName = productDto.ProductName;
            existingProduct.Price = productDto.Price;
            await _productRepository.UpdateProductAsync(existingProduct);
        }
    }
}
