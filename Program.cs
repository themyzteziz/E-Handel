using E_Handel;
using E_Handel.Data;
using E_Handel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using System.Linq;

await DbSeeder.SeedAsync();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Commands: listcustomer, addcustomer, editcustomer, deletecustomer, listorders, orderdetails, addorder, productpages, listcategory, addcategory, editcategory, deletecategory, listproduct, addproduct, editproduct, deleteproduct, exit ");
    var cmd = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

    if (cmd == "exit")
        break;

    switch (cmd)
    {
        case "listcustomer":
            await ListCustomerAsync();
            break;
        case "addcustomer":
            await AddCustomerAsync();
            break;
        case "editcustomer":
            await EditCustomerAsync();
            break;
        case "deletecustomer":
            await DeleteCustomerAsync();
            break;
        case "listorders":
            await ListOrdersAsync();
            break;
        case "orderdetails":
            await OrderDetailsAsync();
            break;
        case "addorder":
            await AddOrderAsync();
            break;
        case "productpages":
            Console.WriteLine("Enter page number:");
            var pageInput = Console.ReadLine();
            Console.WriteLine("Enter page size:");
            var pageSizeInput = Console.ReadLine();
            if (int.TryParse(pageInput, out var page) && int.TryParse(pageSizeInput, out var pageSize))
            {
                await ListProductPagesAsync(page, pageSize);
            }
            else
            {
                Console.WriteLine("Invalid page number or page size.");
            }
            break;
            case "listcategory":
                await ListCategoryAsync();
                break;
            case "addcategory":
                await AddCategoryAsync();
                break;
            case "editcategory":
                await EditCategoryAsync();
                break;
            case "deletecategory":
                await DeleteCategoryAsync();
                break;
            case "listproduct":
                await ListProductAsync();
                break;
            case "addproduct":
                await AddProductAsync();
                break;
            case "editproduct":
                await EditProductAsync();
                break;
            case "deleteproduct":
                await DeleteProductAsync();
                break;
        default:
            Console.WriteLine("Unknown command.");
            break;
    }
}

static async Task DeleteProductAsync()
{
    using var db = new ShopContext();

    Console.WriteLine("Enter product ID to delete:");
    var idInput = Console.ReadLine();
    if (!int.TryParse(idInput, out var productId))
    {
        Console.WriteLine("Invalid product ID.");
        return;
    }

    var product = await db.Products.FindAsync(productId);
    if (product == null)
    {
        Console.WriteLine("Product not found.");
        return;
    }

    // Kolla om produkten används i orderrader
    var usedInOrders = await db.OrderRows.AnyAsync(or => or.ProductId == productId);
    if (usedInOrders)
    {
        Console.WriteLine("Cannot delete product – it is used in one or more order rows.");
        return;
    }

    db.Products.Remove(product);
    await db.SaveChangesAsync();

    Console.WriteLine("Product deleted successfully.");
}

static async Task EditProductAsync()
{
    using var db = new ShopContext();

    Console.WriteLine("Enter product ID to edit:");
    var idInput = Console.ReadLine();
    if (!int.TryParse(idInput, out var productId))
    {
        Console.WriteLine("Invalid product ID.");
        return;
    }

    var product = await db.Products
        .Include(p => p.Category)
        .FirstOrDefaultAsync(p => p.ProductId == productId);

    if (product == null)
    {
        Console.WriteLine("Product not found.");
        return;
    }

    Console.WriteLine($"Current name: {product.Name}");
    Console.WriteLine("Enter new name (leave blank to keep current):");
    var name = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(name))
    {
        product.Name = name;
    }

    Console.WriteLine($"Current price: {product.Price}");
    Console.WriteLine("Enter new price (leave blank to keep current):");
    var priceInput = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(priceInput))
    {
        if (!decimal.TryParse(priceInput, out var price) || price < 0)
        {
            Console.WriteLine("Invalid price. Changes cancelled.");
            return;
        }
        product.Price = price;
    }

    Console.WriteLine($"Current stock: {product.Stock}");
    Console.WriteLine("Enter new stock (leave blank to keep current):");
    var stockInput = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(stockInput))
    {
        if (!int.TryParse(stockInput, out var stock) || stock < 0)
        {
            Console.WriteLine("Invalid stock. Changes cancelled.");
            return;
        }
        product.Stock = stock;
    }

    Console.WriteLine($"Current category: {product.Category?.Name ?? "None"}");
    Console.WriteLine("Change category? (y/n)");
    var changeCat = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
    if (changeCat == "y")
    {
        var categories = await db.Categories
            .OrderBy(c => c.CategoryId)
            .ToListAsync();

        if (!categories.Any())
        {
            Console.WriteLine("No categories found. Cannot change category.");
        }
        else
        {
            Console.WriteLine("Available categories:");
            foreach (var c in categories)
            {
                Console.WriteLine($"- #{c.CategoryId} {c.Name}");
            }

            Console.WriteLine("Enter new category ID:");
            var catInput = Console.ReadLine();
            if (!int.TryParse(catInput, out var categoryId) ||
                !categories.Any(c => c.CategoryId == categoryId))
            {
                Console.WriteLine("Invalid category ID. Category not changed.");
            }
            else
            {
                product.CategoryId = categoryId;
            }
        }
    }

    await db.SaveChangesAsync();

    Console.WriteLine("Product updated successfully.");
}

static async Task AddProductAsync()
{
    using var db = new ShopContext();

    // Kolla att det finns kategorier
    var categories = await db.Categories
        .OrderBy(c => c.CategoryId)
        .ToListAsync();

    if (!categories.Any())
    {
        Console.WriteLine("No categories found. Add a category first.");
        return;
    }

    Console.WriteLine("Enter product name:");
    var name = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Name cannot be empty.");
        return;
    }

    Console.WriteLine("Enter product price:");
    var priceInput = Console.ReadLine();
    if (!decimal.TryParse(priceInput, out var price) || price < 0)
    {
        Console.WriteLine("Invalid price.");
        return;
    }

    Console.WriteLine("Enter product stock:");
    var stockInput = Console.ReadLine();
    if (!int.TryParse(stockInput, out var stock) || stock < 0)
    {
        Console.WriteLine("Invalid stock.");
        return;
    }

    Console.WriteLine("Available categories:");
    foreach (var c in categories)
    {
        Console.WriteLine($"- #{c.CategoryId} {c.Name}");
    }

    Console.WriteLine("Enter category ID:");
    var catInput = Console.ReadLine();
    if (!int.TryParse(catInput, out var categoryId) ||
        !categories.Any(c => c.CategoryId == categoryId))
    {
        Console.WriteLine("Invalid category ID.");
        return;
    }

    var product = new Product
    {
        Name = name,
        Price = price,
        Stock = stock,
        CategoryId = categoryId
    };

    await db.Products.AddAsync(product);
    await db.SaveChangesAsync();

    Console.WriteLine($"Product #{product.ProductId} added successfully.");
}

static async Task ListProductAsync()
{
    using var db = new ShopContext();

    var products = await db.Products
        .Include(p => p.Category)
        .OrderBy(p => p.ProductId)
        .ToListAsync();

    Console.WriteLine("Products:");

    if (!products.Any())
    {
        Console.WriteLine("No products found.");
        return;
    }

    foreach (var product in products)
    {
        Console.WriteLine(
            $"- #{product.ProductId} {product.Name} | Price: {product.Price} | Stock: {product.Stock} | Category: {product.Category?.Name ?? "None"}");
    }
}


static async Task DeleteCategoryAsync()
{
    using var db = new ShopContext();

    Console.WriteLine("Enter category ID to delete:");
    var idInput = Console.ReadLine();
    if (!int.TryParse(idInput, out var categoryId))
    {
        Console.WriteLine("Invalid category ID.");
        return;
    }

    var category = await db.Categories
        .Include(c => c.Products)
        .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

    if (category == null)
    {
        Console.WriteLine("Category not found.");
        return;
    }

    if (category.Products.Any())
    {
        Console.WriteLine("Cannot delete category because it has products linked to it.");
        Console.WriteLine("Move or delete those products first.");
        return;
    }

    db.Categories.Remove(category);
    await db.SaveChangesAsync();

    Console.WriteLine("Category deleted successfully.");
}


static async Task EditCategoryAsync()
{
    using var db = new ShopContext();

    Console.WriteLine("Enter category ID to edit:");
    var idInput = Console.ReadLine();
    if (!int.TryParse(idInput, out var categoryId))
    {
        Console.WriteLine("Invalid category ID.");
        return;
    }

    var category = await db.Categories.FindAsync(categoryId);
    if (category == null)
    {
        Console.WriteLine("Category not found.");
        return;
    }

    Console.WriteLine("Enter new category name (leave blank to keep current):");
    var name = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(name))
    {
        category.Name = name;
    }

    Console.WriteLine("Enter new category description (leave blank to keep current):");
    var description = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(description))
    {
        category.Description = description;
    }

    await db.SaveChangesAsync();

    Console.WriteLine("Category updated successfully.");
}


static async Task AddCategoryAsync()
{
    using var db = new ShopContext();

    Console.WriteLine("Enter category name:");
    var name = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Name cannot be empty.");
        return;
    }

    Console.WriteLine("Enter category description (optional):");
    var description = Console.ReadLine() ?? "";

    var category = new Category
    {
        Name = name,
        Description = string.IsNullOrWhiteSpace(description) ? null : description
    };

    await db.Categories.AddAsync(category);
    await db.SaveChangesAsync();

    Console.WriteLine("Category added successfully.");
}

static async Task ListCategoryAsync()
{
    using var db = new ShopContext();

    var categories = await db.Categories
        .Include(c => c.Products)
        .ToListAsync();

    Console.WriteLine("Categories:");

    if (categories.Any())
    {
        foreach (var category in categories)
        {
            var productCount = category.Products?.Count ?? 0;
            Console.WriteLine($"- {category.Name} | Description: {category.Description} | Products Count: {productCount}");
        }
    }
    else
    {
        Console.WriteLine("No categories found.");
    }
}



static async Task ListProductPagesAsync(int page, int pageSize)
{
    using var db = new ShopContext();

    var query = db.Products.AsQueryable();


    var totalCount = await query.CountAsync();
    var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

    var products = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    Console.WriteLine($"Products - Page {page} of {totalPages}:");
    foreach (var product in products)
    {
        Console.WriteLine($"- Product #{product.ProductId} {product.Name} - Price: {product.Price} | Stock: {product.Stock}");
    }
}

static async Task ListCustomerAsync()
{
    using var db = new ShopContext();

    var customers = await db.Customers
        .Include(c => c.Orders)
        .ToListAsync();

    Console.WriteLine("Customers:");

    if (customers.Any())
    {
        foreach (var customer in customers)
        {
            Console.WriteLine($"- {customer.Name} ({customer.Email}, {customer.City}) | Orders Count: {customer.Orders.Count}");
        }
    }
    else
    {
        Console.WriteLine("No customers found.");
    }
}

static async Task AddCustomerAsync()
{
    using var db = new ShopContext();

    Console.WriteLine("Enter customer name:");
    var name = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Name cannot be empty.");
        return;
    }

    Console.WriteLine("Enter customer email:");
    var email = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(email))
    {
        Console.WriteLine("Email cannot be empty.");
        return;
    }

    Console.WriteLine("Enter customer city:");
    var city = Console.ReadLine() ?? "";
    if (string.IsNullOrWhiteSpace(city))
    {
        Console.WriteLine("City cannot be empty.");
        return;
    }

    var customer = new Customer
    {
        Name = name,
        Email = email,
        City = city
    };
    await db.Customers.AddAsync(customer);
    await db.SaveChangesAsync();

    Console.WriteLine("Customer added successfully.");
}

static async Task EditCustomerAsync()
{
    using var db = new ShopContext();

    Console.WriteLine("Enter customer ID to edit:");
    var idInput = Console.ReadLine();
    if (!int.TryParse(idInput, out var customerId))
    {
        Console.WriteLine("Invalid customer ID.");
        return;
    }

    var customer = await db.Customers.FindAsync(customerId);
    if (customer == null)
    {
        Console.WriteLine("Customer not found.");
        return;
    }

    Console.WriteLine("Enter new customer name (leave blank to keep current):");
    var name = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(name))
    {
        customer.Name = name;
    }

    Console.WriteLine("Enter new customer email (leave blank to keep current):");
    var email = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(email))
    {
        customer.Email = email;
    }

    Console.WriteLine("Enter new customer city (leave blank to keep current):");
    var city = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(city))
    {
        customer.City = city;
    }

    await db.SaveChangesAsync();

    Console.WriteLine("Customer updated successfully.");
}

static async Task DeleteCustomerAsync()
{
    using var db = new ShopContext();

    Console.WriteLine("Enter customer ID to delete:");
    var idInput = Console.ReadLine();
    if (!int.TryParse(idInput, out var customerId))
    {
        Console.WriteLine("Invalid customer ID.");
        return;
    }

    var customer = await db.Customers.FindAsync(customerId);
    if (customer == null)
    {
        Console.WriteLine("Customer not found.");
        return;
    }

    db.Customers.Remove(customer);
    await db.SaveChangesAsync();

    Console.WriteLine("Customer deleted successfully.");
}

static async Task ListOrdersAsync()
{
    using var db = new ShopContext();

    var orders = await db.Orders
        .Include(o => o.Customer)        
        .OrderBy(o => o.OrderId)
        .ToListAsync();

    Console.WriteLine("Orders:");

    if (orders.Any())
    {
        foreach (var order in orders)
        {
            Console.WriteLine($"- Order #{order.OrderId} by {order.Customer?.Name} on {order.OrderDate} | Total: {order.TotalAmount}");
        }
    }
    else
    {
        Console.WriteLine("No orders found.");
    }
}

static async Task OrderDetailsAsync()
{
    using var db = new ShopContext();

    Console.WriteLine("Enter order ID to view details:");
    var idInput = Console.ReadLine();
    if (!int.TryParse(idInput, out var orderId))
    {
        Console.WriteLine("Invalid order ID.");
        return;
    }

    var order = await db.Orders
        .Include(o => o.Customer)
        .Include(o => o.OrderRows)
        .FirstOrDefaultAsync(o => o.OrderId == orderId);

    if (order == null)
    {
        Console.WriteLine("Order not found.");
        return;
    }

    Console.WriteLine($"Order #{order.OrderId} Details:");
    Console.WriteLine($"- Customer: {order.Customer?.Name}");
    Console.WriteLine($"- Order Date: {order.OrderDate}");
    Console.WriteLine($"- Total Amount: {order.TotalAmount}");

    if (order.OrderRows.Any())
    {
        Console.WriteLine("Order rows:");
        foreach (var row in order.OrderRows)
        {
            Console.WriteLine($" Product ID: {row.ProductId}, Name: {row.Product?.Name}, Quantity: {row.Quantity}, Unit Price: {row.UnitPrice}");
        }
    }
    else
    {
        Console.WriteLine("No items in this order.");
    }
}

static async Task AddOrderAsync()
{
    using var db = new ShopContext();

    Console.WriteLine("Enter customer ID:");
    var customerIdInput = Console.ReadLine();
    if (!int.TryParse(customerIdInput, out var customerId))
    {
        Console.WriteLine("Invalid customer ID.");
        return;
    }

    var customer = await db.Customers.FindAsync(customerId);
    if (customer == null)
    {
        Console.WriteLine("Customer not found.");
        return;
    }

    Console.WriteLine("Enter order date (yyyy-mm-dd):");
    var orderDateInput = Console.ReadLine();
    if (!DateTime.TryParse(orderDateInput, out var orderDate))
    {
        Console.WriteLine("Invalid order date.");
        return;
    }

    Console.WriteLine("Enter total amount:");
    var totalAmountInput = Console.ReadLine();
    if (!decimal.TryParse(totalAmountInput, out var totalAmount))
    {
        Console.WriteLine("Invalid total amount.");
        return;
    }

    var order = new Order
    {
        Customer = customer,
        OrderDate = orderDate,
        Status = "Created",
        TotalAmount = totalAmount
    };
    await db.Orders.AddAsync(order);
    await db.SaveChangesAsync();

    Console.WriteLine("Order added successfully.");
}