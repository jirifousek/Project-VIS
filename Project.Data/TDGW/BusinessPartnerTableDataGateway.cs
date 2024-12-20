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
    public class BusinessPartnerTableDataGateway
    {
        string host = "dbsys.cs.vsb.cz\\STUDENT";
        string username = "FOU0027";
        string password = "vOqGhHV8r7jvHWMB";

        string connectionString;

        public BusinessPartnerTableDataGateway()
        {
            connectionString = $"Data Source={host};Initial Catalog=FOU0027;User ID={username};Password={password};Encrypt=False";
        }

        public BusinessPartnerDTO GetBusinessPartnerById(int id)
        {
            DataTable result = new DataTable();
            var query = "SELECT bp.bp_id, bp.name, bp.incoterms, bp.payment_terms, bp.role, bp.address " +
                        "FROM business_partner bp " +
                        "WHERE bp_id  = @id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result.Load(reader);
                    }
                }
            }

            return new BusinessPartnerDTO
            {
                Id = Convert.ToInt32(result.Rows[0]["bp_id"]),
                Name = result.Rows[0]["name"].ToString(),
                Incoterms = result.Rows[0]["incoterms"].ToString(),
                PaymentTerms = result.Rows[0]["payment_terms"].ToString(),
                Role = result.Rows[0]["role"].ToString(),
                Address = result.Rows[0]["address"].ToString()
            };

        }

        public List<BusinessPartnerDTO> GetAllBusinessPartners()
        {
            DataTable result = new DataTable();
            var query = "SELECT bp_id, name, incoterms, payment_terms, role, address " +
                        "FROM business_partner;";
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

            List<BusinessPartnerDTO> businessPartners = new List<BusinessPartnerDTO>();
            foreach (DataRow row in result.Rows)
            {
                businessPartners.Add(new BusinessPartnerDTO
                {
                    Id = Convert.ToInt32(row["bp_id"]),
                    Name = row["name"].ToString(),
                    Incoterms = row["incoterms"].ToString(),
                    PaymentTerms = row["payment_terms"].ToString(),
                    Role = row["role"].ToString(),
                    Address = row["address"].ToString()
                });
            }

            return businessPartners;
        }

        public void InsertBusinessPartner(BusinessPartnerDTO businessPartner)
        {
            var query = "INSERT INTO business_partner (name, incoterms, payment_terms, role, address) " +
                        "VALUES (@name, @incoterms, @payment_terms, @role, @address);";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("name", businessPartner.Name);
                    command.Parameters.AddWithValue("incoterms", businessPartner.Incoterms);
                    command.Parameters.AddWithValue("payment_terms", businessPartner.PaymentTerms);
                    command.Parameters.AddWithValue("role", businessPartner.Role);
                    command.Parameters.AddWithValue("address", businessPartner.Address);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateBusinessPartner(BusinessPartnerDTO businessPartner)
        {
            var query = "UPDATE business_partner SET name = @name, incoterms = @incoterms, payment_terms = @payment_terms, role = @role, address = @address WHERE bp_id = @bp_id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("bp_id", businessPartner.Id);
                    command.Parameters.AddWithValue("name", businessPartner.Name);
                    command.Parameters.AddWithValue("incoterms", businessPartner.Incoterms);
                    command.Parameters.AddWithValue("payment_terms", businessPartner.PaymentTerms);
                    command.Parameters.AddWithValue("role", businessPartner.Role);
                    command.Parameters.AddWithValue("address", businessPartner.Address);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteBusinessPartner(int id)
        {
            var query = "DELETE FROM business_partner WHERE bp_id = @bp_id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("bp_id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
