using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task AddCustomerAsync(CustomerEntity customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<CustomerEntity?> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<CustomerEntity?> GetCustomerByNameAsync(string customerName)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerName == customerName);
    }
}
