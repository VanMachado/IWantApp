using System.Linq;
using Domain;
using Flunt.Validations;
using Products;

namespace Orders;

public class Order : Entity
{
    public string ClientId { get; private set; }
    public List<Product> Products { get; private set; } = new List<Product>();
    public decimal Total { get; set; }
    public string DeliveryAddress { get; private set; }

    private Order()
    {
    }

    public Order(string clientId, string clientName,List<Product> products , string deliveryAddress)
    {
        ClientId = clientId;        
        DeliveryAddress = deliveryAddress;
        Name = clientName;
        Products = products;
        CreatedBy = clientName;
        EditedBy = clientName;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
        Total = TotalSum();

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Order>()
                    .IsNotNullOrEmpty(Name, "Name")
                    .IsGreaterOrEqualsThan(Name, 3, "Name")
                    .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
                    .IsTrue(Products != null || Products.Any(), "Products")
                    .IsNotNullOrEmpty(EditedBy, "EditedBy")
                    .IsNotEmpty(Products, "Products");
        AddNotifications(contract);
    }

    private decimal TotalSum()
    {
        decimal total = 0;
        foreach(var item in Products)
        {
            total += item.Price;
        }

        return total;
    }
}
