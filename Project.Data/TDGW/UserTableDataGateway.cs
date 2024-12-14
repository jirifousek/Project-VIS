using Project.Data.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Project.Data.TDGW
{
    public class UserTableDataGateway
    {
        string host = "dbsys.cs.vsb.cz\\STUDENT";
        string username = "FOU0027";
        string password = "vOqGhHV8r7jvHWMB";

        private string connectionString;

        public UserTableDataGateway()
        {
            connectionString = $"Data Source={host};Initial Catalog=FOU0027;User ID={username};Password={password};Encrypt=False";
        }

        public UserDTO GetUserById(int id)
        {
            DataTable result = new DataTable();
            var query = "SELECT employee_id, first_name, last_name, password, role " +
                        "FROM employee " +
                        "WHERE employee_id  = @id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //command.Parameters.Add("id", SqlDbType.Int32).Value = id;
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result.Load(reader);
                    }
                }
            }

            return new UserDTO
            {
                Id = Convert.ToInt32(result.Rows[0]["employee_id"]),
                FirstName = result.Rows[0]["first_name"].ToString(),
                LastName = result.Rows[0]["last_name"].ToString(),
                Password = result.Rows[0]["password"].ToString(),
                Role = result.Rows[0]["role"].ToString()
            };

        }

        public List<UserDTO> GetAllUsers()
        {
            DataTable result = new DataTable();
            var query = "SELECT employee_id, first_name, last_name, password, role " +
                        "FROM employee;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result.Load(reader);
                    }
                }
            }

            List<UserDTO> users = new List<UserDTO>();
            foreach (DataRow row in result.Rows)
            {
                users.Add(new UserDTO
                {
                    Id = Convert.ToInt32(row["employee_id"]),
                    FirstName = row["first_name"].ToString(),
                    LastName = row["last_name"].ToString(),
                    Password = row["password"].ToString(),
                    Role = row["role"].ToString()
                });
            }

            return users;
        }

        public void InsertUser(UserDTO user)
        {
            var query = "INSERT INTO employee (first_name, last_name, password, role) " +
                        "VALUES (@first_name, @last_name, @password, @role);";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@first_name", user.FirstName);
                    command.Parameters.AddWithValue("@last_name", user.LastName);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@role", user.Role);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUser(UserDTO user)
        {
            var query = "UPDATE employee SET first_name = @first_name, last_name = @last_name, password = @password, role = @role " +
                        "WHERE employee_id = @employee_id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    /*command.Parameters.Add("employee_id", SqlDbType.Int32).Value = user.Id;
                    command.Parameters.Add("first_name", SqlDbType.Int32).Value = user.FirstName;
                    command.Parameters.Add("last_name", SqlDbType.Int32).Value = user.LastName;
                    command.Parameters.Add("password", SqlDbType.Int32).Value = user.Password;
                    command.Parameters.Add("role", SqlDbType.Int32).Value = user.Role;*/

                    command.Parameters.AddWithValue("@employee_id", user.Id);
                    command.Parameters.AddWithValue("@first_name", user.FirstName);
                    command.Parameters.AddWithValue("@last_name", user.LastName);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@role", user.Role);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int id)
        {
            var query = "DELETE FROM employee WHERE employee_id = @employee_id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //command.Parameters.Add("employee_id", SqlDbType.Int32).Value = id;

                    command.Parameters.AddWithValue("@employee_id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
