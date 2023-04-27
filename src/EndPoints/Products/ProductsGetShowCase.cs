﻿using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Products;

public class ProductsGetShowCase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ApplicationDbContext context, int page = 1, int row = 10, string orderBy = "name")
    {
        if (row > 10)
            return Results.Problem(title: "Row's out of range", statusCode: 400);

        var queryBase = context.Products.AsNoTracking().Include(p => p.Category)
            .Where(p => p.HasStock == true && p.Category.Active);

        if (orderBy == "name")
            queryBase = queryBase.OrderBy(p => p.Name);
        else if (orderBy == "price")
            queryBase = queryBase.OrderBy(p => p.Price);
        else
            return Results.Problem(title: "Order only by name or price", statusCode: 400);

        var queryFilter = queryBase.Skip((page - 1) * row).Take(row);

        var products = await queryFilter.ToListAsync();
        var response = products.Select(p => new ProductResponse(p.Name, p.Category.Name,
            p.Description, p.HasStock, p.Active, p.Price));

        return Results.Ok(response);
    }
}
