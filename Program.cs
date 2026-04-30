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

            if (product != null && product.Stock > 0)
            {
                int qty = GetInt("Enter Quantity: ");

                if (qty <= product.Stock)
                {
                    cart.Add(new CartItem(product, qty));
                    product.Stock -= qty;
                    Console.WriteLine("Item added to cart.");
                }
                else Console.WriteLine("Not enough stock.");
            }
            else Console.WriteLine("Invalid product.");

            again = GetYN("Add another item? (Y/N): ");
        } while (again == 'Y');
    }

    static void SearchProduct()
    {
        Console.Write("Enter product name: ");
        string key = Console.ReadLine().ToLower();

        foreach (var p in products)
        {
            if (p.Name.ToLower().Contains(key))
                Console.WriteLine($"{p.Name} - PHP {p.Price} - Stock: {p.Stock}");
        }
    }

    static void FilterCategory()
    {
        Console.WriteLine("1. Food\n2. Electronics\n3. Clothing");
        int choice = GetInt("Choose category: ");

        string category = choice == 1 ? "Food" :
                          choice == 2 ? "Electronics" : "Clothing";

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
                case 4: cart.Clear(); Console.WriteLine("Cart cleared."); break;
                case 5: Checkout(); return;
                case 6: return;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }

    static void ViewCart()
    {
        double total = 0;
        Console.WriteLine("\nCART ITEMS:");

        foreach (var item in cart)
        {
            double subtotal = item.Quantity * item.Product.Price;
            Console.WriteLine($"{item.Product.Name} x{item.Quantity} = PHP
