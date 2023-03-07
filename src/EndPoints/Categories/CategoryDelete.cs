using Data;
using Microsoft.AspNetCore.Mvc;
using Products;

namespace Categories;

public class CategoryDelete
{
    public static string Template => "/categories/{id}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid id, ApplicationDbContext context)
    {
        var category = context.Categories.Where(x => x.Id == id)
            .FirstOrDefault();

        if (category == null)
            return Results.NotFound("Sorry, ID not found");

        context.Categories.Remove(category);
        context.SaveChanges();

        return Results.Ok();
    }
}
