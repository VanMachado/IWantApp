using Products;

namespace Orders;

public record OrderResponse(Guid OrderId, string ClientId, string Name, decimal Total, string DeliveryAddress);
