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
    public class MaterialTableDataGateway
    {
        string host = "dbsys.cs.vsb.cz";
        string port = "1521";
        string sid = "oracle";
        string username = "FOU0027";
        string password = "VO2fGM6Q60YP3NVG";

        string connectionString;

        public MaterialTableDataGateway()
        {
            connectionString = $"User Id={username};Password={password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port}))(CONNECT_DATA=(SID={sid})))";
        }

        public MaterialDTO GetMaterialById(int id)
        {
            DataTable result = new DataTable();
            var query = "SELECT m.material_id, m.description, m.qty_available, m.unit_of_measure, m.brutto_weight, mp.price" +
                        "FROM material m " +
                        "JOIN material_price mp on m.material_id = mp.material_id" +
                        "WHERE Id = @id" +
                        "AND mp.valid_from = (select max(valid_from) from material_price where material_id = @id);";
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

            return new MaterialDTO
            {
                Id = Convert.ToInt32(result.Rows[0]["material_id"]),
                Description = result.Rows[0]["description"].ToString(),
                Stock = Convert.ToInt32(result.Rows[0]["qty_available"]),
                UnitOfMeasure = Convert.ToInt32(result.Rows[0]["unit_of_measure"]),
                Weight = Convert.ToInt32(result.Rows[0]["brutto_weight"]),
                Price = Convert.ToDouble(result.Rows[0]["price"])
            };

        }

        public List<MaterialDTO> GetAllMaterials()
        {
            DataTable result = new DataTable();
            var query = "SELECT m.material_id, m.description, m.qty_available, m.unit_of_measure, m.brutto_weight, mp.price" +
                        "FROM material m " +
                        "JOIN material_price mp on m.material_id = mp.material_id" +
                        "WHERE mp.valid_from = (select max(valid_from) from material_price where material_id = m.material_id);";
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

            List<MaterialDTO> materials = new List<MaterialDTO>();
            foreach (DataRow row in result.Rows)
            {
                materials.Add(new MaterialDTO
                {
                    Id = Convert.ToInt32(row["material_id"]),
                    Description = row["description"].ToString(),
                    Stock = Convert.ToInt32(row["qty_available"]),
                    UnitOfMeasure = Convert.ToInt32(row["unit_of_measure"]),
                    Weight = Convert.ToInt32(row["brutto_weight"]),
                    Price = Convert.ToDouble(row["price"])
                });
            }

            return materials;
        }

        public void UpdateMaterial(MaterialDTO material)
        {
            var query = "UPDATE material SET description = @description, qty_available = @qty_available, unit_of_measure = @unit_of_measure, brutto_weight = @brutto_weight WHERE material_id = @material_id";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("material_id", OracleDbType.Int32).Value = material.Id;
                    command.Parameters.Add("description", OracleDbType.Int32).Value = material.Description;
                    command.Parameters.Add("qty_available", OracleDbType.Int32).Value = material.Stock;
                    command.Parameters.Add("unit_of_measure", OracleDbType.Int32).Value = material.UnitOfMeasure;
                    command.Parameters.Add("brutto_weight", OracleDbType.Int32).Value = material.Weight;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertMaterial(MaterialDTO material)
        {
            var query = "INSERT INTO material (description, qty_available, unit_of_measure, brutto_weight) VALUES (@description, @qty_available, @unit_of_measure, @brutto_weight)";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("description", OracleDbType.Int32).Value = material.Description;
                    command.Parameters.Add("qty_available", OracleDbType.Int32).Value = material.Stock;
                    command.Parameters.Add("unit_of_measure", OracleDbType.Int32).Value = material.UnitOfMeasure;
                    command.Parameters.Add("brutto_weight", OracleDbType.Int32).Value = material.Weight;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteMaterial(int id)
        {
            var query = "DELETE FROM material WHERE material_id = @material_id";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add("material_id", OracleDbType.Int32).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
