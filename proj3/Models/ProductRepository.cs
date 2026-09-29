using Microsoft.Data.SqlClient;
using System.Data;

namespace poj3.Models
{
    public class ProductRepository
    {
        private readonly string _cs;
        public ProductRepository(string connectionString) => _cs = connectionString;

        private SqlConnection Connection() => new(_cs);

        public List<Product> GetAll(string? search = null, string? category = null)
        {
            var list = new List<Product>();
            const string sql = @"SELECT id,name,category,description,price,image,stock
                                 FROM dbo.products
                                 WHERE is_active=1
                                   AND (@search='' OR name LIKE '%' + @search + '%' OR category LIKE '%' + @search + '%')
                                   AND (@category='' OR category=@category)
                                 ORDER BY id DESC;";
            using var con = Connection();
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@search", SqlDbType.NVarChar, 200).Value = search?.Trim() ?? "";
            cmd.Parameters.Add("@category", SqlDbType.NVarChar, 100).Value = category?.Trim() ?? "";
            con.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read()) list.Add(Map(r));
            return list;
        }

        public Product? GetById(int id)
        {
            const string sql = @"SELECT id,name,category,description,price,image,stock
                                 FROM dbo.products WHERE id=@id AND is_active=1;";
            using var con = Connection();
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            con.Open();
            using var r = cmd.ExecuteReader();
            return r.Read() ? Map(r) : null;
        }

        public bool Exists(int id)
        {
            using var con = Connection();
            using var cmd = new SqlCommand("SELECT COUNT(1) FROM dbo.products WHERE id=@id AND is_active=1;", con);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            con.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public bool HasStock(int id, int quantity)
        {
            using var con = Connection();
            using var cmd = new SqlCommand("SELECT stock FROM dbo.products WHERE id=@id AND is_active=1;", con);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            con.Open();
            var v = cmd.ExecuteScalar();
            return v != null && Convert.ToInt32(v) >= quantity;
        }

        public bool ReduceStock(SqlConnection con, SqlTransaction tx, int id, int quantity)
        {
            using var cmd = new SqlCommand(@"UPDATE dbo.products SET stock=stock-@qty
                                             WHERE id=@id AND is_active=1 AND stock>=@qty;", con, tx);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = quantity;
            return cmd.ExecuteNonQuery() == 1;
        }

        private static Product Map(SqlDataReader r) => new()
        {
            Id = r.GetInt32(0),
            Name = r.GetString(1),
            Category = r.GetString(2),
            Description = r.IsDBNull(3) ? "" : r.GetString(3),
            Price = r.GetDecimal(4),
            Image = r.IsDBNull(5) ? "" : r.GetString(5),
            Stock = r.GetInt32(6)
        };
    }
}
