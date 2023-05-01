namespace Orders;

public record OrderResponse(Guid OrderId, string ClientId, string Name, string DeliveryAddress, IEnumerable<OrderProduct> products);
public record OrderResponseEmployee(Guid OrderId, string ClientId, string Name, string DeliveryAddress);
public record OrderProduct(Guid Id, string Name);
