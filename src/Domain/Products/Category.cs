using Domain;
using Flunt.Validations;

namespace Products;

public class Category : Entity
{
    public string Name { get; private set; }
    public bool Active { get; private set; }

    public Category(string name, string createdBy, string editedBy)
    {
        Name = name;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        Active = true;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;

        Validate();        
    }    

    public void EditInfo(string name, bool active)
    {        
        Name = name;
        Active = active;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Category>()
                    .IsNotNullOrEmpty(Name, "Name")
                    .IsGreaterOrEqualsThan(Name, 3, "Name")
                    .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
                    .IsNotNullOrEmpty(EditedBy, "EditedBy");
        AddNotifications(contract);
    }
}
