using Business.Dtos;

namespace Business.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> AddCustomerAsync(string customerName);
    Task<CustomerDto?> GetCustomerByIdAsync(int id);
    Task<CustomerDto?> GetCustomerByNameAsync(string customerName);
}