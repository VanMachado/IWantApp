namespace Products;

public record ProductRequest(string Name, Guid CategoryId, decimal Price);
