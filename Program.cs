using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price} (Stock: {RemainingStock})");
    }

    public double GetItemTotal(int quantity)
    {
        return Price * quantity;
    }

    public bool HasEnoughStock(int quantity)
    {
        return RemainingStock >= quantity;
    }

    public void DeductStock(int quantity)
    {
        RemainingStock -= quantity;
    }
}

class CartItem
{
    public Product Product;
    public int Quantity;
    public double Subtotal;

    public void Update(int qty)
    {
        Quantity += qty;
        Subtotal = Product.Price * Quantity;
    }
}

class Program
{
    static void Main()
    {
        Product[] menu = new Product[]
        {
            new Product { Id = 1, Name = "Burger", Price = 120, RemainingStock = 10 },
            new Product { Id = 2, Name = "Pizza", Price = 300, RemainingStock = 5 },
            new Product { Id = 3, Name = "Fries", Price = 80, RemainingStock = 15 },
            new Product { Id = 4, Name = "Chicken Meal", Price = 150, RemainingStock = 8 },
            new Product { Id = 5, Name = "Softdrinks", Price = 50, RemainingStock = 20 }
        };

        CartItem[] cart = new CartItem[5];
        int cartCount = 0;

        bool continueShopping = true;

        while (continueShopping)
        {
        
            Console.WriteLine("===== RESTAURANT MENU =====");

            for (int i = 0; i < menu.Length; i++)
            {
                menu[i].DisplayProduct();
            }

            Console.Write("\nEnter product number: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                continue;
            }

            if (choice < 1 || choice > menu.Length)
            {
                Console.WriteLine("Invalid product number.");
                Pause();
                continue;
            }

            Product selected = menu[choice - 1];

            if (selected.RemainingStock == 0)
            {
                Console.WriteLine("This product is out of stock.");
                Pause();
                continue;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                Pause();
                continue;
            }

            if (!selected.HasEnoughStock(quantity))
            {
                Console.WriteLine("Not enough stock available.");
                Pause();
                continue;
            }

            // Check duplicate
            bool found = false;
            for (int i = 0; i < cartCount; i++)
            {
                if (cart[i].Product.Id == selected.Id)
                {
                    cart[i].Update(quantity);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                if (cartCount >= cart.Length)
                {
                    Console.WriteLine("Cart is full.");
                    Pause();
                    continue;
                }

                cart[cartCount] = new CartItem
                {
                    Product = selected,
                    Quantity = quantity,
                    Subtotal = selected.GetItemTotal(quantity)
                };
                cartCount++;
            }

            selected.DeductStock(quantity);

            Console.WriteLine("Item added to cart!");

            Console.Write("\nAdd more items? (Y/N): ");
            string ans = Console.ReadLine().ToUpper();

            if (ans != "Y")
                continueShopping = false;
        }

        // RECEIPT
        Console.Clear();
        Console.WriteLine("===== RECEIPT =====");

        double grandTotal = 0;

        for (int i = 0; i < cartCount; i++)
        {
            Console.WriteLine($"{cart[i].Product.Name} x{cart[i].Quantity} = ₱{cart[i].Subtotal}");
            grandTotal += cart[i].Subtotal;
        }

        Console.WriteLine($"\nGrand Total: ₱{grandTotal}");

        double discount = 0;
        if (grandTotal >= 5000)
        {
            discount = grandTotal * 0.10;
            Console.WriteLine($"Discount (10%): ₱{discount}");
        }

        double finalTotal = grandTotal - discount;
        Console.WriteLine($"Final Total: ₱{finalTotal}");

        // UPDATED STOCK
        Console.WriteLine("\n===== UPDATED STOCK =====");
        for (int i = 0; i < menu.Length; i++)
        {
            Console.WriteLine($"{menu[i].Name} - Remaining: {menu[i].RemainingStock}");
        }

        Console.WriteLine("\nThank you for ordering!");
    }

    static void Pause()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}