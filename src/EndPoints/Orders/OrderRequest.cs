using Products;

namespace Orders;

public record OrderRequest(List<Guid> ProductsId, string Name, string DeliveryAddress);
