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
            var orders = await context.Orders
               .Include(p => p.Products)
               .OrderBy(p => p.Name)
               .ToListAsync();                         

            var responseEmployee = orders.Select(p => new OrderResponseEmployee(p.Id, p.ClientId, p.Name, p.DeliveryAddress));

            return Results.Ok(responseEmployee);
        }

        var op = context.Orders           
            .Include(p => p.Products)
            .FirstOrDefault(o => o.ClientId == clientId);   
                
        var orderProduct = op.Products.Select(p => new OrderProduct(p.Id, p.Name));
        var response = new OrderResponse(op.Id, clientId, op.Name, op.DeliveryAddress, orderProduct);

        return Results.Ok(response);
    }
}
