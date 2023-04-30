using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Products;

namespace Orders;

public class ProductsSold
{
    public static string Template => "/products/mostsold";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(QueryAllProductsSold query)
    {
        int max = 0;
        var response = await query.ExecuteAsync();
        
        foreach(var item in response)
        {
            if(item.Amount > max)
                max = item.Amount;
        }

        var result = response.Where(p => p.Amount == max);
        return Results.Ok(result);
    }
}
