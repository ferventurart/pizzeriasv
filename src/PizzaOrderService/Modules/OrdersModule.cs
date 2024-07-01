using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using PizzaOrderService.Events;
using PizzaOrderService.Models;

namespace PizzaOrderService.Modules;

public static class OrdersModule
{
    public static void RegisterOrdersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/orders/").WithTags("Orders");

        group.MapGet("", async (AppDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var orders = await dbContext.Orders.ToListAsync(cancellationToken);
                
                var response = orders.Select(s => new OrderResponse
                {
                    Id = s.Id,
                    Quantity = s.Quantity,
                    Size = s.Size,
                    Timestamp = s.Timestamp,
                    Comments = s.Comments,
                    Toppings = s.Toppings.ToList()
                });

                return Results.Ok(response);
            })
            .WithSummary("Get all pizza orders.")
            .WithDescription("Get all the pizza orders.")
            .WithName("GetOrders")
            .WithOpenApi()
            .Produces<List<OrderResponse>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        
        group.MapGet("{id:guid}", async (Guid id, AppDbContext dbContext, CancellationToken cancellationToken) =>
            {
                var order = await dbContext.Orders.SingleOrDefaultAsync(s => s.Id == id, cancellationToken);

                if (order is null)
                {
                    return Results.NotFound();
                }

                var response = new OrderResponse
                {
                    Id = order.Id,
                    Quantity = order.Quantity,
                    Size = order.Size,
                    Comments = order.Comments,
                    Timestamp = order.Timestamp,
                    Toppings = order.Toppings.ToList()
                };

                return Results.Ok(response);
            })
            .WithSummary("Get order by Id.")
            .WithDescription("Get the pizza order by their Identifier (Id).")
            .WithName("GetOrder")
            .WithOpenApi(op =>
            {
                op.Parameters[0].Description = "Unique Id that identifies the order.";
                return op;
            })
            .Produces(StatusCodes.Status404NotFound)
            .Produces<OrderResponse>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapPost("", async (
                IValidator<OrderRequest> validator,
                OrderRequest request,
                AppDbContext dbContext,
                ISendEndpointProvider bus,
                CancellationToken cancellationToken) =>
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var order = new Order()
                {
                    Id = Guid.NewGuid(),
                    Quantity = request.Quantity,
                    Size = request.Size,
                    Comments = request.Comments,
                    Status = OrderStatus.Created,
                    Timestamp = DateTime.Now
                };

                foreach (var topping in request.Toppings)
                {
                    order.AddTopping(topping);
                }

                dbContext.Orders.Add(order);

                await dbContext.SaveChangesAsync(cancellationToken);

                var endpoint = await bus.GetSendEndpoint(new Uri("queue:kitchen.orders"));

                await endpoint.Send(new KitchenOrderEvent
                {
                    Id = order.Id,
                    CreatedAt = order.Timestamp,
                    Quantity = order.Quantity,
                    Size = order.Size,
                    Status = order.Status,
                    Comments = order.Comments,
                    Toppings = order.Toppings.ToList(),
                }, cancellationToken);
                
                return Results.Created("orders/id", order.Id);
            })
            .WithSummary("Create a new pizza order.")
            .WithDescription("Allows you create a new order and send it to Kitchen.")
            .WithName("CreateOrder")
            .WithOpenApi(op =>
            {
                op.RequestBody.Description = "The contract to create a new order";
                op.RequestBody.Content["application/json"].Example =
                    new OpenApiString(JsonSerializer.Serialize(
                        new OrderRequest
                        {
                            Quantity = 1,
                            Size = PizzaSize.ExtraLarge,
                            Toppings = Order.AvailableToppings.ToList(),
                            Comments = "This is an example"
                        }
                    ));
                return op;
            })
            .Accepts<OrderRequest>("application/json")
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

public record OrderRequest
{
    public int Quantity { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PizzaSize Size { get; init; }
    public List<string> Toppings { get; init; } = [];
    public string Comments { get; init; } = string.Empty;
};

internal class OrderRequestValidator : AbstractValidator<OrderRequest>
{
    public OrderRequestValidator()
    {
        RuleFor(r => r.Quantity)
            .GreaterThan(0);

        RuleFor(r => r.Size)
            .IsInEnum();

        RuleFor(r => r.Toppings)
            .Must(t => t.Count <= 5)
            .WithMessage("The pizza cannot have more than 5 ingredients.");

        RuleFor(r => r.Comments)
            .MaximumLength(50);
    }
}
public record OrderResponse
{
    public Guid Id { get; init; }
    public int Quantity { get; init; }
    public IReadOnlyList<string> Toppings { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PizzaSize Size { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; init; }
    public string Comments { get; init; }
    public DateTime Timestamp { get; init; }
};