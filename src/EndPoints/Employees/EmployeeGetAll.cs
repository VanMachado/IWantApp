using Data;

namespace Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int? page, int? rows, QueryAllUsersWithClaimName query)
    {
        if (page == null || rows == null)
            return Results.BadRequest("Page and Rows is required");

        if (rows > 10)
            return Results.BadRequest("Rows cannot above 10");
                               
        return Results.Ok(query.Execute(page.Value, rows.Value));
    }
}
