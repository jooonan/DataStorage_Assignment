using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task AddCustomerAsync(CustomerEntity customer)
    {
        try
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while adding customer: {ex.Message}");
            throw;
        }
    }

    public async Task<CustomerEntity?> GetCustomerByIdAsync(int id)
    {
        try
        {
            return await _context.Customers.FindAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving customer by ID ({id}): {ex.Message}");
            return null;
        }
    }

    public async Task<CustomerEntity?> GetCustomerByNameAsync(string customerName)
    {
        try
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerName == customerName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving customer by name ({customerName}): {ex.Message}");
            return null;
        }
    }
}
