using System.Text.Json.Serialization;
using PizzaOrderService.Models;

namespace PizzaOrderService.Events;

public record KitchenOrderEvent
{
    public Guid Id { get; init; }
    public int Quantity { get; init; }
    public DateTime CreatedAt { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PizzaSize Size { get; init; }
    public List<string> Toppings { get; init; } = [];
    public string Comments { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; init; }
}