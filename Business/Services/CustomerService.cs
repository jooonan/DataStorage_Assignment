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


    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(id);
        return customer == null ? null : new CustomerDto
        {
            Id = customer.Id,
            CustomerName = customer.CustomerName
        };
    }

    public async Task<CustomerDto?> GetCustomerByNameAsync(string customerName)
    {
        var existingCustomer = await _customerRepository.GetCustomerByNameAsync(customerName);
        if (existingCustomer != null)
        {
            return new CustomerDto { Id = existingCustomer.Id, CustomerName = existingCustomer.CustomerName };
        }
        return null;
    }
}
