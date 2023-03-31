using Dapper;
using Employees;
using Microsoft.Data.SqlClient;

namespace Data;

public class QueryAllUsersWithClaimName
{
    private readonly IConfiguration Configuration;

    public QueryAllUsersWithClaimName(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IEnumerable<EmployeeResponse> Execute(int page, int rows)
    {        
        var db = new SqlConnection(Configuration["ConnectionStrings:IWantDb"]);
        var query = @"SELECT Email, ClaimValue as Name
                FROM AspNetUsers u INNER JOIN AspNetUserClaims c
                on u.Id = UserId AND ClaimType =  'Name'
                order by name
                OFFSET(@page-1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";
        return db.Query<EmployeeResponse>(query, new { page, rows });        
    }   
}
