using Domain;
using Flunt.Validations;

namespace Products;

public class Product : Entity
{
    public string Name { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public string Description { get; private set; }
    public bool HasStock { get; private set; }
    public bool Active { get; private set; } = true;
    public decimal Price { get; private set; }

    public Product()
    {
    }

    public Product(string name, Category category, string createdBy, decimal price)
    {
        Name = name;
        Category = category;
        CreatedBy = createdBy;
        EditedBy = createdBy;
        Price = price;
        Description = "New Item";
        HasStock = true;
        Active = true;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Category>()
                    .IsNotNullOrEmpty(Name, "Name")
                    .IsNotNull(Category, "Category", "Category not found")
                    .IsGreaterOrEqualsThan(Price, 1, "Price")
                    .IsGreaterOrEqualsThan(Name, 3, "Name")
                    .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
                    .IsNotNullOrEmpty(EditedBy, "EditedBy");
        AddNotifications(contract);
    }
}
