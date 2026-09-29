using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AdminLTE.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }

        [Required]
        public string name { get; set; } = "";

        [Required]
        public string email { get; set; } = "";

        [Required]
        public string password { get; set; } = "";

        private const string ConnectionString =
            "Data Source=.\\SQLEXPRESS;Initial Catalog=pro3;Integrated Security=True;pwd=123;TrustServerCertificate=True";

        public void Insert()
        {
            const string query = @"INSERT INTO register(name, email, password)
                                   VALUES (@name, @email, @password)";

            using var con = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = name.Trim();
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = email.Trim();
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = password;

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public DataSet Login(string loginEmail, string loginPassword)
        {
            var ds = new DataSet();
            const string query = @"SELECT TOP 1 id, name, email, password
                                   FROM register
                                   WHERE email = @email AND password = @password";

            using var con = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = loginEmail.Trim();
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = loginPassword;

            using var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        public DataSet GetById(int userId)
        {
            var ds = new DataSet();
            const string query = @"SELECT id, name, email, password
                                   FROM register WHERE id = @id";

            using var con = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;

            using var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        public bool EmailExists(string checkEmail, int? exceptId = null)
        {
            const string query = @"SELECT COUNT(1) FROM register
                                   WHERE email = @email
                                   AND (@id IS NULL OR id <> @id)";

            using var con = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = checkEmail.Trim();
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = exceptId.HasValue ? exceptId.Value : DBNull.Value;

            con.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public void UpdateAccount(int userId)
        {
            const string query = @"UPDATE register
                                   SET name = @name, email = @email, password = @password
                                   WHERE id = @id";

            using var con = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;
            cmd.Parameters.Add("@name", SqlDbType.VarChar, 100).Value = name.Trim();
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = email.Trim();
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = password;

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteAccount(int userId)
        {
            const string query = "DELETE FROM register WHERE id = @id";

            using var con = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = userId;

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public DataSet CheckEmail(string checkEmail)
        {
            var ds = new DataSet();
            const string query = @"SELECT id, name, email, password
                                   FROM register WHERE email = @email";

            using var con = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = checkEmail.Trim();

            using var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        public void UpdatePassword(string updateEmail, string newPassword)
        {
            const string query = @"UPDATE register
                                   SET password = @password
                                   WHERE email = @email";

            using var con = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = newPassword;
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = updateEmail.Trim();

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
