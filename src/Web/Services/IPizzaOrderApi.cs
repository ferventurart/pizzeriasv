using Refit;
using Web.Models;

namespace Web.Services;

public interface IPizzaOrderApi
{
    [Get("/api/orders")]
    Task<IReadOnlyList<OrderResponse>> GetOrders();
    
    [Post("/api/orders")]
    Task<ApiResponse<Guid>> CreateOrder(OrderModel order);
}