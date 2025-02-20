using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Data.Repositories;

namespace Business.Services;

public class CustomerService(CustomerRepository customerRepository) : ICustomerService
{
    private readonly CustomerRepository _customerRepository = customerRepository;

    public async Task<CustomerDto> AddCustomerAsync(string customerName)
    {
        try
        {
            var existingCustomer = await _customerRepository.GetCustomerByNameAsync(customerName);
            if (existingCustomer != null)
            {
                return new CustomerDto
                {
                    Id = existingCustomer.Id,
                    CustomerName = existingCustomer.CustomerName
                };
            }

            var newCustomer = new CustomerEntity
            {
                CustomerName = customerName
            };

            await _customerRepository.AddCustomerAsync(newCustomer);

            return new CustomerDto
            {
                Id = newCustomer.Id,
                CustomerName = newCustomer.CustomerName
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while adding customer: {ex.Message}");
            throw;
        }
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        try
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            return customer == null ? null : new CustomerDto
            {
                Id = customer.Id,
                CustomerName = customer.CustomerName
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving customer: {ex.Message}");
            return null;
        }
    }
}