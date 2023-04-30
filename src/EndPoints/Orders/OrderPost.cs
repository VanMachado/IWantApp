using System.Security.Claims;
using Data;
using Microsoft.AspNetCore.Authorization;
using EndPoints;
using Products;
using Microsoft.EntityFrameworkCore;

namespace Orders;

public class OrderPost
{
    public static string Template => "/orders";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "CpfPolicy")]
    public static async Task<IResult> Action(OrderRequest orderRequest, HttpContext http,
        ApplicationDbContext context)
    {
        var clientId =
            http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var clientName =
            http.User.Claims.First(c => c.Type == "Name").Value;
                       
        var productFound = context.Products.Where(p => orderRequest.ProductsId.Contains(p.Id)).ToList();

        var order = new Order(clientId, clientName, productFound, orderRequest.DeliveryAddress);

        if (!order.IsValid)
            return Results.ValidationProblem(order.Notifications.ConvertToProblemDetails());

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return Results.Created($"/ordes/{order.Id}", order.Id);
    }
}
