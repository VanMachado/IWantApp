using System.Security.Claims;
using Data;
using EndPoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Products;

public class ProductsPost
{
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ProductRequest productRequest, HttpContext http,
        ApplicationDbContext context)
    {        
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == productRequest.CategoryId);
        var newProduct = new Product(productRequest.Name, category, userId, productRequest.Price);
                        
        if (!newProduct.IsValid)
            return Results.ValidationProblem(newProduct.Notifications.ConvertToProblemDetails());

        await context.AddAsync(newProduct);
        await context.SaveChangesAsync();

        return Results.Created($"/employees/{newProduct.Id}", newProduct.Id);
    }
}
