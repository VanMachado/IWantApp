using Data;
using Microsoft.AspNetCore.Mvc;
using EndPoints;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Categories;

public class CategoryPut
{
    public static string Template => "/categories/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action([FromRoute] Guid id, HttpContext http, CategoryRequest categoryRequest,
        ApplicationDbContext context)
    {
        var userId = http.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;               
        var category = context.Categories
            .Where(c => c.Id == id)
            .FirstOrDefault();

        if (category == null)
            return Results.NotFound("Sorry, ID not found");

        category.EditInfo(categoryRequest.Name, categoryRequest.Active, userId);

        if (!category.IsValid)
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        
        context.SaveChanges();

        return Results.Ok();
    }
}
