namespace PizzaOrderService.Models;

public sealed class Order
{
    public Guid Id { get; set; }
    public PizzaSize Size { get; set; }
    public ICollection<string> Toppings { get; init; } = [];
    public int Quantity { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime Timestamp { get; set; }

    public static readonly string[] AvailableToppings =
    [
        "Pepperoni", "Mushrooms", "Onions", "Sausage", "Bacon",
        "Black Olives", "Green Peppers", "Spinach", "Tomatoes",
        "Chicken", "Artichokes", "Pineapple", "Jalapenos", "Ham",
        "Anchovies", "Basil", "Feta Cheese", "Cheddar Cheese",
        "Parmesan Cheese", "Ricotta Cheese", "Garlic", "Bell Peppers",
        "Red Onions", "Sun-dried Tomatoes", "BBQ Chicken", "Buffalo Chicken",
        "Steak", "Goat Cheese", "Provolone Cheese", "Cherry Tomatoes",
        "Banana Peppers", "Broccoli", "Cilantro", "Corn", "Pesto",
        "Salami", "Shrimp", "Tuna", "Zucchini", "Avocado"
    ];

    public void AddTopping(string topping)
    {
        Toppings.Add(topping);
    }
}