namespace Web.Services;

public record OrderResponse
{
    public Guid Id { get; init; }
    public int Quantity { get; init; }
    public IReadOnlyList<string> Toppings { get; init; }
    public string Size { get; init; }
    public string Status { get; init; }
    public DateTime Timestamp { get; init; }
};