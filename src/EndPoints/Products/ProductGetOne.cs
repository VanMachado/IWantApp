using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Products;

public class ProductGetOne
{
    public static string Template => "/products/{id}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, ApplicationDbContext context)
    {
        var product = context.Products.Where(x => x.Id == id)
            .Include(c => c.Category)
            .FirstOrDefault();
        
        if (product == null)
            return Results.NotFound("Sorry, Product not found");

        var response = new ProductResponse(product.Name, product.Category.Name, product.Description, product.HasStock,
            product.Active, product.Price);
        
        return Results.Ok(response);
    }
}
