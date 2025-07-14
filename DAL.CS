using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace DemoWebApp.Model
{
    public class DAL
    {
        public List<Users> GetUsers(IConfiguration configuration)
        {
            List<Users> UserList = new List<Users>();

            // Ensure the connection string is correct and present in appsettings.json
            string connectionString = configuration.GetConnectionString("DBCS");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TblUsers", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    Users user = new Users
                    {
                        Id = Convert.ToString(row["Id"]),
                        FirstName = Convert.ToString(row["FirstName"]),
                        LastName = Convert.ToString(row["LastName"])
                    };
                    UserList.Add(user);
                }
            }

            return UserList;
        }
        public int AddUser(Users user, IConfiguration configuration)
        {
            int i = 0;
            string connectionString = configuration.GetConnectionString("DBCS").ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO TblUsers (FirstName, LastName) VALUES (@FirstName, @LastName)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);

                try
                {
                    conn.Open();
                    i = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log or rethrow
                    throw new Exception("Error while inserting: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return i;
        }
        public Users GetUsers(string id, IConfiguration configuration)
        {
            Users user = new Users();

            // Ensure the connection string is correct and present in appsettings.json
            string connectionString = configuration.GetConnectionString("DBCS");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Use parameterized query to avoid SQL injection
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TblUsers WHERE Id = @Id", conn);
                da.SelectCommand.Parameters.AddWithValue("@Id", id);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    user.Id = Convert.ToString(dt.Rows[0]["Id"]);
                    user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                    user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                }
            }

            return user;
        }

        public int UpdateUser(Users user, IConfiguration configuration)
        {
            int i = 0;
            string connectionString = configuration.GetConnectionString("DBCS").ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE TblUsers SET FirstName = @FirstName, LastName = @LastName WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                cmd.Parameters.AddWithValue("@LastName", user.LastName);
                cmd.Parameters.AddWithValue("@Id", user.Id);

                try
                {
                    conn.Open();
                    i = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log or rethrow
                    throw new Exception("Error while inserting: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return i;
        }
        public int DeleteUser(string Id, IConfiguration configuration)
        {
            int i = 0;
            string connectionString = configuration.GetConnectionString("DBCS").ToString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM TblUsers WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", Id);

                try
                {
                    conn.Open();
                    i = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log or rethrow
                    throw new Exception("Error while inserting: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return i;
        }



    }
}
