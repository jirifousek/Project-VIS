﻿using Project.Data.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;


namespace Project.Data.TDGW
{
    public class UserTableDataGateway
    {
        string host = "dbsys.cs.vsb.cz";
        string port = "1521";
        string sid = "oracle";
        string username = "FOU0027";
        string password = "VO2fGM6Q60YP3NVG";

        string connectionString;

        public UserTableDataGateway()
        {
            connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port}))(CONNECT_DATA=(SID={sid})));Connection Timeout=60";
        }

        public UserDTO GetUserById(int id)
        {
            DataTable result = new DataTable();
            var query = "SELECT employee_id, first_name, last_name, password, role " +
                        "FROM employee " +
                        "WHERE employee_id  = @id;";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("id", OracleDbType.Int32).Value = id;

                    using (OracleDataReader reader = command.ExecuteReader())
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
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
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
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("first_name", OracleDbType.Int32).Value = user.FirstName;
                    command.Parameters.Add("last_name", OracleDbType.Int32).Value = user.LastName;
                    command.Parameters.Add("password", OracleDbType.Int32).Value = user.Password;
                    command.Parameters.Add("role", OracleDbType.Int32).Value = user.Role;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUser(UserDTO user)
        {
            var query = "UPDATE employee SET first_name = @first_name, last_name = @last_name, password = @password, role = @role " +
                        "WHERE employee_id = @employee_id;";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("employee_id", OracleDbType.Int32).Value = user.Id;
                    command.Parameters.Add("first_name", OracleDbType.Int32).Value = user.FirstName;
                    command.Parameters.Add("last_name", OracleDbType.Int32).Value = user.LastName;
                    command.Parameters.Add("password", OracleDbType.Int32).Value = user.Password;
                    command.Parameters.Add("role", OracleDbType.Int32).Value = user.Role;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int id)
        {
            var query = "DELETE FROM employee WHERE employee_id = @employee_id;";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("employee_id", OracleDbType.Int32).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
