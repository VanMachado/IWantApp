using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Products;

public class ProductsGetAll
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ApplicationDbContext context)
    {
        var products = await context.Products.Include(p => p.Category).OrderBy(p => p.Name).ToListAsync();
        var response = products.Select(p => new ProductResponse(p.Name, p.Category.Name, 
            p.Description, p.HasStock, p.Active, p.Price));

        return Results.Ok(response);
    }
}
