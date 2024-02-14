namespace AWS.Playground.API;

public record Realty
{
    public Guid Id { get; set; }
    public string? Address { get; init; }
    public decimal Price { get; init; }
}
