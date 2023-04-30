using Dapper;
using Microsoft.Data.SqlClient;
using Products;

namespace Data;

public class QueryAllProductsSold
{
    private readonly IConfiguration Configuration;

    public QueryAllProductsSold(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public async Task<IEnumerable<ProductsSoldResponse>> ExecuteAsync()
    {
        var db = new SqlConnection(Configuration["ConnectionStrings:IWantDb"]);
        var query = @"select 
                        p.Id,
                        p.Name,
                        count(*) Amount
                    from
                        Orders o inner join OrderProducts op on o.Id = op.OrdersId
                        inner join Products p on p.Id = op.ProductsId
                    group by
                        p.Id, p.Name
                    order by Amount desc";
        return await db.QueryAsync<ProductsSoldResponse>(query);
    }
}
