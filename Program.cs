using System;
using System.Collections.Generic;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int Stock;
    public string Category;

    public Product(int id, string name, double price, int stock, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
        Category = category;
    }
}

class CartItem
{
    public Product Product;
    public int Quantity;

    public CartItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }
}

class Order
{
    public int ReceiptNumber;
    public DateTime Date;
    public double Total;
}

class Program
{
    static List<Product> products = new List<Product>()
    {
        new Product(1, "Burger", 100, 10, "Food"),
        new Product(2, "Mouse", 500, 7, "Electronics"),
        new Product(3, "Keyboard", 800, 4, "Electronics"),
        new Product(4, "T-Shirt", 300, 6, "Clothing")
    };

    static List<CartItem> cart = new List<CartItem>();
    static List<Order> orders = new List<Order>();
    static int receiptNo = 1;

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== MAIN MENU ===");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Search Product");
            Console.WriteLine("3. Filter by Category");
            Console.WriteLine("4. Cart Menu");
            Console.WriteLine("5. Order History");
            Console.WriteLine("6. Exit");

            int choice = GetInt("Choose: ");

            switch (choice)
            {
                case 1: ShowProducts(); AddToCart(); break;
                case 2: SearchProduct(); break;
                case 3: FilterCategory(); break;
                case 4: CartMenu(); break;
                case 5: ShowHistory(); break;
                case 6: return;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }
    static void ShowProducts()
    {
        Console.WriteLine("\nPRODUCT LIST:");
        foreach (var p in products)
        {
            Console.WriteLine($"{p.Id}. {p.Name} - PHP {p.Price} - Stock: {p.Stock} - {p.Category}");
        }
    }

    static void AddToCart()
    {
        char again;

        do
        {
            int id = GetInt("Enter Product ID: ");
            var product = products.Find(p => p.Id == id);

            if (product == null)
            {
                Console.WriteLine("Invalid product.");
            }
            else if (product.Stock <= 0)
            {
                Console.WriteLine("Out of stock.");
            }
            else
            {
                int qty = GetInt("Enter Quantity: ");

                if (qty > 0 && qty <= product.Stock)
                {
                    cart.Add(new CartItem(product, qty));
                    product.Stock -= qty;
                    Console.WriteLine("Item added to cart.");
                }
                else
                {
                    Console.WriteLine("Invalid quantity.");
                }
            }

            again = GetYN("Add another item? (Y/N): ");

        } while (again == 'Y');
    }

    static void SearchProduct()
    {
        Console.Write("Enter product name: ");
        string key = Console.ReadLine().ToLower();

        bool found = false;

        foreach (var p in products)
        {
            if (p.Name.ToLower().Contains(key))
            {
                Console.WriteLine($"{p.Name} - PHP {p.Price} - Stock: {p.Stock}");
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("No product found.");
    }

    static void FilterCategory()
    {
        Console.WriteLine("1. Food\n2. Electronics\n3. Clothing");
        int choice = GetInt("Choose category: ");

        string category =
            choice == 1 ? "Food" :
            choice == 2 ? "Electronics" :
            choice == 3 ? "Clothing" : "";

        if (category == "")
        {
            Console.WriteLine("Invalid category.");
            return;
        }

        foreach (var p in products)
        {
            if (p.Category == category)
                Console.WriteLine($"{p.Name} - PHP {p.Price} - Stock: {p.Stock}");
        }
    }

    static void CartMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== CART MENU ===");
            Console.WriteLine("1. View Cart");
            Console.WriteLine("2. Remove Item");
            Console.WriteLine("3. Update Quantity");
            Console.WriteLine("4. Clear Cart");
            Console.WriteLine("5. Checkout");
            Console.WriteLine("6. Back");

            int choice = GetInt("Choose: ");

            switch (choice)
            {
                case 1: ViewCart(); break;
                case 2: RemoveItem(); break;
                case 3: UpdateQuantity(); break;
                case 4: ClearCart(); break;
                case 5: Checkout(); return;
                case 6: return;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }
static void ViewCart()
    {
        if (cart.Count == 0)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        double total = 0;

        Console.WriteLine("\nCART ITEMS:");

        foreach (var item in cart)
        {
            double subtotal = item.Quantity * item.Product.Price;
            Console.WriteLine($"{item.Product.Name} x{item.Quantity} = PHP {subtotal}");
            total += subtotal;
        }

        Console.WriteLine($"Total: PHP {total}");
    }

    static void RemoveItem()
    {
        Console.Write("Enter product name: ");
        string name = Console.ReadLine();

        var item = cart.Find(x =>
            x.Product.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (item != null)
        {
            item.Product.Stock += item.Quantity;
            cart.Remove(item);
            Console.WriteLine("Item removed.");
        }
        else
        {
            Console.WriteLine("Item not found.");
        }
    }

    static void UpdateQuantity()
    {
        Console.Write("Enter product name: ");
        string name = Console.ReadLine();

        var item = cart.Find(x =>
            x.Product.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (item == null)
        {
            Console.WriteLine("Item not found.");
            return;
        }

        int newQty = GetInt("Enter new quantity: ");

        if (newQty <= 0)
        {
            Console.WriteLine("Invalid quantity.");
            return;
        }

        int diff = newQty - item.Quantity;

        if (diff > item.Product.Stock)
        {
            Console.WriteLine("Not enough stock.");
            return;
        }

        item.Product.Stock -= diff;
        item.Quantity = newQty;

        Console.WriteLine("Quantity updated.");
    }

    static void ClearCart()
    {
        foreach (var item in cart)
        {
            item.Product.Stock += item.Quantity;
        }

        cart.Clear();
        Console.WriteLine("Cart cleared.");
    }

    static void Checkout()
    {
        if (cart.Count == 0)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        double total = 0;

        foreach (var item in cart)
            total += item.Quantity * item.Product.Price;

        Console.WriteLine($"Final Total: PHP {total}");

        double payment;

        while (true)
        {
            payment = GetDouble("Enter payment: ");

            if (payment >= total)
                break;

            Console.WriteLine("Insufficient payment.");
        }

        double change = payment - total;

        Console.WriteLine("\n=== RECEIPT ===");
        Console.WriteLine($"Receipt No: {receiptNo:D4}");
        Console.WriteLine($"Date: {DateTime.Now}");
        Console.WriteLine($"Total: PHP {total}");
        Console.WriteLine($"Payment: PHP {payment}");
        Console.WriteLine($"Change: PHP {change}");

        orders.Add(new Order
        {
            ReceiptNumber = receiptNo,
            Date = DateTime.Now,
            Total = total
        });

        receiptNo++;
        cart.Clear();

        ShowLowStock();
    }

    static void ShowLowStock()
    {
        Console.WriteLine("\nLOW STOCK ALERT:");

        foreach (var p in products)
        {
            if (p.Stock <= 5)
                Console.WriteLine($"{p.Name} has only {p.Stock} left.");  }
    }

    static void ShowHistory()
    {
        Console.WriteLine("\nORDER HISTORY:");

        foreach (var o in orders)
        {
            Console.WriteLine($"Receipt #{o.ReceiptNumber:D4} - PHP {o.Total}");
        }
    }

    // VALIDATION
    static int GetInt(string msg)
    {
        int val;

        while (true)
        {
            Console.Write(msg);
            if (int.TryParse(Console.ReadLine(), out val))
                return val;

            Console.WriteLine("Invalid input.");
        }
    }

    static double GetDouble(string msg)
    {
        double val;

        while (true)
        {
            Console.Write(msg);
            if (double.TryParse(Console.ReadLine(), out val))
                return val;

            Console.WriteLine("Invalid input.");
        }
    }

    static char GetYN(string msg)
    {
        while (true)
        {
            Console.Write(msg);
            string input = Console.ReadLine().ToUpper();

            if (input == "Y" || input == "N")
                return input[0];

            Console.WriteLine("Enter Y or N only.");
        }
    }
}
