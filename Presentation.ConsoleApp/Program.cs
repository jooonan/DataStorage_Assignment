using Business.Interfaces;
using Business.Services;
using Data.Context;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.ConsoleApp.Dialogs;

class Program
{
    static async Task Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<DataContext>(options =>
                options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Projects\\DataStorage_Assignment\\Data\\Databases\\local_database.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True"))

            .AddScoped<ProjectRepository>()
            .AddScoped<CustomerRepository>()
            .AddScoped<UserRepository>()
            .AddScoped<ProductRepository>()
            .AddScoped<StatusTypeRepository>()

            .AddScoped<IUserService, UserService>()
            .AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IProjectService, ProjectService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IStatusTypeService, StatusTypeService>()

            .AddScoped<MenuDialogs>()

            .BuildServiceProvider();

        var menuDialogs = serviceProvider.GetRequiredService<MenuDialogs>();
        await menuDialogs.ShowMainMenuAsync();
    }
}
