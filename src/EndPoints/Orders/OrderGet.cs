using System.Security.Claims;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace Orders;

public class OrderGet
{
    public static string Template => "/orders";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize]
    public static async Task<IResult> Action(ApplicationDbContext context, HttpContext http)
    {
        var clientId =
            http.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        if(http.User.Claims.FirstOrDefault(c => c.Type == "Cpf") == null)
        {            
            List<Order> orders = new List<Order>();
            orders = await context.Orders
               .Include(p => p.Products)
               .OrderBy(p => p.Name)
               .ToListAsync();                         

            var responseEmployee = orders.Select(p => new OrderResponse(p.Id, p.ClientId, p.Name, p.Total, p.DeliveryAddress));

            return Results.Ok(responseEmployee);
        }

        var orderGet = await context.Orders
            .Include(p => p.Products)
            .OrderBy(p => p.Name)
            .Where(p => p.ClientId == clientId)
            .ToListAsync();     

        var response = orderGet.Select(p => new OrderResponse(p.Id, clientId, p.Name, p.Total, p.DeliveryAddress));

        return Results.Ok(response);
    }
}
