Part 1 – Project Initialization (Day 1)

Today I started working on the Restaurant Shopping Cart System. The goal is to build a console-based program in C# that allows users to select menu items, input quantities, and simulate a simple checkout process.

I began by planning the structure of the program. I decided to create a Product class that will represent each menu item. This class includes properties such as Id, Name, Price, and RemainingStock. These will allow me to manage each item properly, especially when tracking available stock.

I also added a method called DisplayProduct() to print each item in the menu. In addition, I planned to include helper methods like GetItemTotal(), HasEnoughStock(), and DeductStock() to handle computations and stock updates.

At the end of today, I created an array of Product objects to serve as the restaurant menu. The program can now display the menu using a loop, which is a good starting point.
