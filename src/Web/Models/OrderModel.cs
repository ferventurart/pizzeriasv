using System.Text.Json.Serialization;

namespace Web.Models;

public class OrderModel
{
    public int Quantity { get; set; } = 1;
    public IEnumerable<string> Toppings { get; set; } = [];
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PizzaSize Size { get; set; }
    
    [JsonIgnore]
    public string ToppingsCollection { get; set; }
    
    [JsonIgnore]
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
}

public enum PizzaSize
{
    Small,
    Medium,
    Large,
    ExtraLarge
}

