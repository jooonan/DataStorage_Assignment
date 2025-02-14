﻿using System;
using Business.Dtos;
using Business.Interfaces;

namespace Presentation.ConsoleApp.Dialogs;

public class MenuDialogs(IProjectService projectService, ICustomerService customerService,
                   IUserService userService, IProductService productService, IStatusTypeService statusTypeService)
{
    private readonly IProjectService _projectService = projectService;
    private readonly ICustomerService _customerService = customerService;
    private readonly IUserService _userService = userService;
    private readonly IProductService _productService = productService;
    private readonly IStatusTypeService _statusTypeService = statusTypeService;

    public async Task ShowMainMenuAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("##### PROJECT MANAGEMENT SYSTEM #####");
            Console.WriteLine("");
            Console.WriteLine("1. Create New Project");
            Console.WriteLine("2. View All Projects");
            Console.WriteLine("3. Edit Existing Project");
            Console.WriteLine("4. Delete Project");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    await CreateProjectAsync();
                    break;
                case "2":
                    await ShowAllProjectsAsync();
                    break;
                case "3":
                    await UpdateProjectAsync();
                    break;
                case "4":
                    await DeleteProjectAsync();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Press any key to try again...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task CreateProjectAsync()
    {
        Console.Clear();
        Console.WriteLine("##### CREATE NEW PROJECT #####");
        Console.WriteLine("");

        string? title;
        do
        {
            Console.Write("Enter Project Title: ");
            title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title cannot be empty.");
            }
        }
        while (string.IsNullOrWhiteSpace(title));
        Console.WriteLine("");

        Console.Write("Enter Project Description: ");
        var description = Console.ReadLine();
        Console.WriteLine("");

        DateTime startDate = GetDateInput("Enter Start Date (YYYY-MM-DD)", DateTime.Now);
        DateTime endDate = GetDateInput("Enter End Date (YYYY-MM-DD)", startDate.AddDays(31));
        Console.WriteLine("");

        int customerId = await GetValidCustomerName();
        Console.WriteLine("");
        int userId = await GetValidUserName();
        Console.WriteLine("");
        int statusId = await GetValidStatusId();
        Console.WriteLine("");
        int productId = await GetValidProductName();
        Console.WriteLine("");

        var projectDto = new ProjectDto
        {
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            CustomerId = customerId,
            UserId = userId,
            StatusId = statusId,
            ProductId = productId
        };

        await _projectService.AddProjectAsync(projectDto);
        Console.WriteLine("Project successfully created!");
        Console.ReadKey();
    }


    private async Task ShowAllProjectsAsync()
    {
        Console.Clear();
        Console.WriteLine("##### ALL PROJECTS #####");
        Console.WriteLine("");

        var projects = await _projectService.GetAllProjectsAsync();
        if (!projects.Any())
        {
            Console.WriteLine("No projects found.");
            Console.ReadKey();
            return;
        }

        foreach (var project in projects)
        {
            var customer = await _customerService.GetCustomerByIdAsync(project.CustomerId);
            string customerName = customer?.CustomerName ?? "Unknown";

            Console.WriteLine($"{project.ProjectId}: {project.Title} (Customer: {customerName})");
            Console.WriteLine("");
        }

        string? projectId = GetInputOrCancel("Enter Project ID to view details or press Esc to cancel", "");
        if (projectId == null) return;

        var projectDetails = await _projectService.GetProjectByIdAsync(projectId);
        if (projectDetails == null)
        {
            Console.WriteLine("Project not found.");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Console.WriteLine("##### PROJECT DETAILS #####");
        Console.WriteLine(""); ;
        Console.WriteLine($"Project ID: {projectDetails.ProjectId}");
        Console.WriteLine($"Title: {projectDetails.Title}");
        Console.WriteLine($"Description: {projectDetails.Description}");
        Console.WriteLine($"Start Date: {projectDetails.StartDate:yyyy-MM-dd}");
        Console.WriteLine($"End Date: {projectDetails.EndDate.ToString("yyyy-MM-dd") ?? "N/A"}");

        var projectCustomer = await _customerService.GetCustomerByIdAsync(projectDetails.CustomerId);
        Console.WriteLine($"Customer: {projectCustomer?.CustomerName ?? "Unknown"}");

        var projectUser = await _userService.GetUserByIdAsync(projectDetails.UserId);
        Console.WriteLine($"User: {(projectUser != null ? $"{projectUser.FirstName} {projectUser.LastName} ({projectUser.Email})" : "Unknown")}");

        var projectStatus = await _statusTypeService.GetStatusTypeByIdAsync(projectDetails.StatusId);
        Console.WriteLine($"Status: {projectStatus?.StatusName ?? "Unknown"}");

        var projectProduct = await _productService.GetProductByIdAsync(projectDetails.ProductId);
        Console.WriteLine($"Product: {projectProduct?.ProductName ?? "Unknown"}");
        Console.WriteLine($"Price: {projectProduct?.Price ?? 0} kr");
        Console.WriteLine(""); ;

        Console.WriteLine("Press any key to return...");


        Console.ReadKey();
    }


    private async Task UpdateProjectAsync()
    {
        Console.Clear();
        Console.WriteLine("##### EDIT PROJECT #####");
        Console.WriteLine("");

        var projects = await _projectService.GetAllProjectsAsync();
        if (!projects.Any())
        {
            Console.WriteLine("No projects found.");
            Console.ReadKey();
            return;
        }
        else
        {
            foreach (var proj in projects)
            {
                var projectCustomer = await _customerService.GetCustomerByIdAsync(proj.CustomerId);
                string customerName = projectCustomer?.CustomerName ?? "Unknown";

                Console.WriteLine($"{proj.ProjectId}: {proj.Title} (Customer: {customerName})");
                Console.WriteLine("");
            }
        }

        string? projectId = GetInputOrCancel("Enter Project ID to Edit (e.g., P-101) or press Esc to cancel", "");
        if (projectId == null) return;
        var project = await _projectService.GetProjectByIdAsync(projectId);
        if (project == null)
        {
            Console.WriteLine("Project not found.");
            Console.ReadKey();
            return;
        }
        Console.Clear();
        Console.WriteLine("##### EDIT PROJECT #####");
        Console.WriteLine("(PRESS ESC TO CANCEL EDITING)");
        Console.WriteLine("");

        var projectDetails = await _projectService.GetProjectByIdAsync(projectId);

        if (projectDetails == null)
        {
            Console.WriteLine("Project not found.");
            return;
        }

        Console.WriteLine($"Current Title: {projectDetails.Title}");
        string? title = GetInputOrCancel("New Project Title", project.Title ?? "");
        if (title == null) return;
        Console.WriteLine("");

        Console.WriteLine($"Current Description: {projectDetails.Description}");
        string? description = GetInputOrCancel("New Project Description", project.Description ?? "");
        if (description == null) return;
        Console.WriteLine("");

        DateTime startDate = GetDateInput($"Enter New Start Date ({project.StartDate:yyyy-MM-dd})", project.StartDate);
        DateTime endDate = GetDateInput($"Enter New End Date ({project.EndDate:yyyy-MM-dd})", project.EndDate);
        Console.WriteLine("");

        //Help from ChatGPT how to implement editing for the customer name
        var customer = await _customerService.GetCustomerByIdAsync(project.CustomerId);
        Console.WriteLine($"Current Customer: {customer?.CustomerName ?? "Unknown"}");
        string? customersName = GetInputOrCancel("New Customer Name", customer?.CustomerName ?? "Unknown");
        if (customersName == null) return;
        project.CustomerId = await GetValidCustomerId(customersName);
        Console.WriteLine("");

        var user = await _userService.GetUserByIdAsync(project.UserId);
        Console.WriteLine($"Current User: {(user != null ? $"{user.FirstName} {user.LastName} ({user.Email})" : "Unknown")}");
        string? firstName = GetInputOrCancel("New First Name", user?.FirstName ?? "Unknown");
        if (firstName == null) return;
        string? lastName = GetInputOrCancel("New Last Name", user?.LastName ?? "Unknown");
        if (lastName == null) return;
        string? email = GetInputOrCancel("New Email", user?.Email ?? "unknown@example.com");
        if (email == null) return;
        project.UserId = await GetValidUserId(firstName, lastName, email);
        Console.WriteLine("");

        var currentStatus = await _statusTypeService.GetStatusTypeByIdAsync(project.StatusId);
        Console.WriteLine($"Current Status: {currentStatus?.StatusName ?? "Unknown"}");
        if (currentStatus == null) return;
        project.StatusId = await GetValidStatusId();
        Console.WriteLine("");

        var product = await _productService.GetProductByIdAsync(project.ProductId);
        Console.WriteLine($"Current Product: {product?.ProductName ?? "Unknown"} - Price: {product?.Price ?? 0} kr");
        string? productName = GetInputOrCancel("New Product Name", product?.ProductName ?? "Unknown") ?? product?.ProductName ?? "Unknown";
        string? priceInput = GetInputOrCancel("New Product Price (kr/tim)", product?.Price.ToString() ?? "");
        decimal productPrice = decimal.TryParse(priceInput, out decimal newPrice) ? newPrice : product?.Price ?? 0;
        project.ProductId = await GetValidProductId(productName, productPrice);
        Console.WriteLine("");






        var updatedProject = new ProjectDto
        {
            ProjectId = project.ProjectId,
            Title = title,
            Description = description,
            StartDate = startDate,
            EndDate = endDate,
            CustomerId = project.CustomerId,
            UserId = project.UserId,
            StatusId = project.StatusId,
            ProductId = project.ProductId
        };

        await _projectService.UpdateProjectAsync(updatedProject);
        Console.WriteLine("Project successfully updated!");
        Console.ReadKey();
    }


    private async Task DeleteProjectAsync()
    {
        Console.Clear();
        Console.WriteLine("##### DELETE PROJECT #####");

        var projects = await _projectService.GetAllProjectsAsync();
        if (!projects.Any())
        {
            Console.WriteLine("No projects found.");
            Console.ReadKey();
            return;
        }
        else
        {
            foreach (var proj in projects)
            {
                var projectCustomer = await _customerService.GetCustomerByIdAsync(proj.CustomerId);
                string customerName = projectCustomer?.CustomerName ?? "Unknown";

                Console.WriteLine($"{proj.ProjectId}: {proj.Title} (Customer: {customerName})");
                Console.WriteLine("");
            }
        }

        while (true)
        {
            string? projectId = GetInputOrCancel("Enter Project ID to Delete (e.g., P-101) or press Esc to cancel", "");
            if (projectId == null) return;

            if (string.IsNullOrEmpty(projectId))
            {
                Console.WriteLine("Invalid input. Please enter a valid Project ID.");
                continue;
            }

            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                Console.WriteLine("Project not found. Please enter a valid Project ID.");
                continue;
            }

            await _projectService.DeleteProjectAsync(projectId);
            Console.WriteLine("Project successfully deleted!");
            Console.ReadKey();
            break;
        }
    }

    //Help from ChatGPT how to start building helper classes
    private async Task<int> GetValidCustomerId(string customerName)
    {
        var customer = await _customerService.AddCustomerAsync(customerName);
        return customer.Id;
    }

    private async Task<int> GetValidCustomerName()
    {
        string customerName = GetRequiredInput("Enter Customer Name");
        return await GetValidCustomerId(customerName);
    }


    private async Task<int> GetValidUserId(string firstName, string lastName, string email)
    {
        var existingUser = await _userService.AddUserAsync(firstName, lastName, email);

        if (existingUser.Email != email)
        {
            existingUser.Email = email;
            await _userService.UpdateUserAsync(existingUser);
        }

        return existingUser.Id;
    }

    private async Task<int> GetValidUserName()
    {
        string firstName = GetRequiredInput("Enter First Name");
        string lastName = GetRequiredInput("Enter Last Name");
        string email = GetRequiredInput("Enter Email");

        return await GetValidUserId(firstName, lastName, email);
    }


    private async Task<int> GetValidStatusId()
    {
        while (true)
        {
            Console.WriteLine("Select a Status:");
            var statuses = await _statusTypeService.GetAllStatusTypesAsync();

            foreach (var statusOption in statuses)
            {
                Console.WriteLine($"{statusOption.Id}. {statusOption.StatusName}");
            }

            Console.Write("Enter Status ID: ");
            if (int.TryParse(Console.ReadLine(), out int selectedStatusId))
            {
                var selectedStatus = statuses.FirstOrDefault(s => s.Id == selectedStatusId);
                if (selectedStatus != null)
                {
                    Console.WriteLine($"Status \"{selectedStatus.StatusName}\" selected.");
                    return selectedStatusId;
                }
            }

            Console.WriteLine($"Invalid input. Please choose a status.");
        }
    }


    private async Task<int> GetValidProductName()
    {
        string productName = GetRequiredInput("Enter Product Name");

        Console.Write("Enter Price (kr/tim): ");
        string? priceInput = Console.ReadLine();
        decimal price = decimal.TryParse(priceInput, out decimal parsedPrice) ? parsedPrice : 0;

        return await GetValidProductId(productName, price);
    }

    private async Task<int> GetValidProductId(string productName, decimal productPrice)
    {
        var existingProduct = await _productService.GetProductByNameAsync(productName);

        if (existingProduct != null)
        {
            if (existingProduct.Price != productPrice)
            {
                existingProduct.Price = productPrice;
                await _productService.UpdateProductAsync(existingProduct);
            }
            return existingProduct.Id;
        }

        var newProduct = await _productService.AddProductAsync(productName, productPrice);
        return newProduct.Id;
    }


    private static string GetRequiredInput(string prompt)
    {
        string? input;
        do
        {
            Console.Write($"{prompt}: ");
            input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine($"{prompt} cannot be empty.");
            }
        } while (string.IsNullOrWhiteSpace(input));

        return input;
    }

    //Help from ChatGPT with method to cancel editing and exit the current page
    private static string? GetInputOrCancel(string prompt, string defaultValue)
    {
        Console.Write($"{prompt}: ");

        string? input = "";
        while (true)
        {
            var keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return null;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return string.IsNullOrWhiteSpace(input) ? defaultValue : input.Trim();
            }
            else if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (input.Length > 0)
                {
                    input = input[..^1];
                    Console.Write("\b \b");
                }
            }
            else
            {
                input += keyInfo.KeyChar;
                Console.Write(keyInfo.KeyChar);
            }
        }
    }

    private static DateTime GetDateInput(string prompt, DateTime defaultValue)
    {
        while (true)
        {
            Console.Write($"{prompt} (Press Enter for {defaultValue:yyyy-MM-dd}): ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return defaultValue;

            if (DateTime.TryParse(input, out DateTime parsedDate))
                return parsedDate;

            Console.WriteLine("Invalid date format. Please enter a valid date (YYYY-MM-DD).");
        }
    }
}
