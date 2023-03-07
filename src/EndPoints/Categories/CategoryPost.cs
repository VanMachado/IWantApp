using Data;
using Products;
using EndPoints;

namespace Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context)
    {
        //Funciona, porem eh mais trabalhoso validar todas as propriedades necessarias
        //if(string.IsNullOrEmpty(categoryRequest.Name))
        //    return Results.BadRequest("Name is required");
        var category = new Category(categoryRequest.Name, categoryRequest.CreatedBy,
            categoryRequest.EditedBy);

        if (!category.IsValid)
        {            
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }            

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
