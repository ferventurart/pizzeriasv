using PizzaOrderService.Models;

namespace PizzaOrderService.Events;

public record OrderCreated(
    Guid OrderId,
    DateTime CreatedAt,
    PizzaSize Size,
    List<string> Toppings,
    OrderStatus Status
);
