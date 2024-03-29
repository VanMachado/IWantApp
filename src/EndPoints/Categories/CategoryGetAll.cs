﻿using Data;
using Products;

namespace Categories;

public class CategoryGetAll
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;        

    public static IResult Action(ApplicationDbContext context)
    {
        var categories = context.Categories.ToList();
        var response = categories.Select(p => new CategoryResponse(p.Name, p.Active, p.Id));

        return Results.Ok(response);
    }
}
