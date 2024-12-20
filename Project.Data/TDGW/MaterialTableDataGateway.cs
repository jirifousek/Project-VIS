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
    public class MaterialTableDataGateway
    {
        string host = "dbsys.cs.vsb.cz\\STUDENT";
        string username = "FOU0027";
        string password = "vOqGhHV8r7jvHWMB";

        string connectionString;

        public MaterialTableDataGateway()
        {
            connectionString = $"Data Source={host};Initial Catalog=FOU0027;User ID={username};Password={password};Encrypt=False";
        }

        public MaterialDTO GetMaterialById(int id)
        {
            DataTable result = new DataTable();
            var query = "SELECT material_id, description, qty_available, unit_of_measure, brutto_weight, price" +
                        "FROM material m " +
                        "WHERE Id = @id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result.Load(reader);
                    }
                }
            }

            return new MaterialDTO
            {
                Id = Convert.ToInt32(result.Rows[0]["material_id"]),
                Description = result.Rows[0]["description"].ToString(),
                Stock = Convert.ToInt32(result.Rows[0]["qty_available"]),
                UnitOfMeasure = result.Rows[0]["unit_of_measure"].ToString(),
                Weight = Convert.ToInt32(result.Rows[0]["brutto_weight"]),
                Price = Convert.ToDouble(result.Rows[0]["price"])
            };

        }

        public List<MaterialDTO> GetAllMaterials()
        {
            DataTable result = new DataTable();
            var query = "SELECT material_id, description, qty_available, unit_of_measure, brutto_weight, price " +
                        "FROM material; ";
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

            List<MaterialDTO> materials = new List<MaterialDTO>();
            foreach (DataRow row in result.Rows)
            {
                materials.Add(new MaterialDTO
                {
                    Id = Convert.ToInt32(row["material_id"]),
                    Description = row["description"].ToString(),
                    Stock = Convert.ToInt32(row["qty_available"]),
                    UnitOfMeasure = row["unit_of_measure"].ToString(),
                    Weight = Convert.ToInt32(row["brutto_weight"]),
                    Price = Convert.ToDouble(row["price"])
                });
            }

            return materials;
        }

        public void UpdateMaterial(MaterialDTO material)
        {
            var query = "UPDATE material SET description = @description, qty_available = @qty_available, unit_of_measure = @unit_of_measure, brutto_weight = @brutto_weight, price = @price " +
                "WHERE material_id = @material_id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", material.Id);
                    command.Parameters.AddWithValue("@description", material.Description);
                    command.Parameters.AddWithValue("@qty_available", material.Stock);
                    command.Parameters.AddWithValue("@unit_of_measure", material.UnitOfMeasure);
                    command.Parameters.AddWithValue("@brutto_weight", material.Weight);
                    command.Parameters.AddWithValue("@price", material.Price);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertMaterial(MaterialDTO material)
        {
            var query = "INSERT INTO material (description, qty_available, unit_of_measure, brutto_weight, price) " +
                "VALUES (@description, @qty_available, @unit_of_measure, @brutto_weight, @price)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@description", material.Description);
                    command.Parameters.AddWithValue("@qty_available", material.Stock);
                    command.Parameters.AddWithValue("@unit_of_measure", material.UnitOfMeasure);
                    command.Parameters.AddWithValue("@brutto_weight", material.Weight);
                    command.Parameters.AddWithValue("@price", material.Price);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteMaterial(int id)
        {
            var query = "DELETE FROM material WHERE material_id = @material_id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@material_id", id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
