using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Project.Data.DTO;
using Microsoft.Data.SqlClient;

namespace Project.Data.TDGW
{
    public class OrderTableDataGateway
    {
        string host = "dbsys.cs.vsb.cz\\STUDENT";
        string username = "FOU0027";
        string password = "vOqGhHV8r7jvHWMB";

        string connectionString;

        public OrderTableDataGateway()
        {
            connectionString = $"Data Source={host};Initial Catalog=FOU0027;User ID={username};Password={password};Encrypt=False";
        }

        public OrderHeaderDTO GetOrderById(int id)
        {
            DataTable result = new DataTable();
            var query = "SELECT document_id, sold_to, expected_delivery_date, price, status " +
                        "FROM sales_order_header " +
                        "WHERE document_id = @id;";
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

            return new OrderHeaderDTO
            {
                Id = Convert.ToInt32(result.Rows[0]["document_id"]),
                CustomerId = Convert.ToInt32(result.Rows[0]["sold_to"]),
                ExpectedDeliveryDate = Convert.ToDateTime(result.Rows[0]["expected_delivery_date"]),
                TotalPrice = Convert.ToDouble(result.Rows[0]["price"]),
                Status = result.Rows[0]["status"].ToString()
            };

        }

        public List<OrderHeaderDTO> GetAllOrders()
        {
            DataTable result = new DataTable();
            var query = "SELECT document_id, sold_to, expected_delivery_date, price, status " +
                        "FROM sales_order_header;";
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

            List<OrderHeaderDTO> orders = new List<OrderHeaderDTO>();
            foreach (DataRow row in result.Rows)
            {
                orders.Add(new OrderHeaderDTO
                {
                    Id = Convert.ToInt32(row["document_id"]),
                    CustomerId = Convert.ToInt32(row["sold_to"]),
                    ExpectedDeliveryDate = Convert.ToDateTime(row["expected_delivery_date"]),
                    TotalPrice = Convert.ToDouble(row["price"]),
                    Status = row["status"].ToString()
                });
            }

            return orders;
        }

        public List<OrderItemDTO> GetOrderItems(int orderId)
        {
            DataTable result = new DataTable();
            var query = "SELECT document_id, posnr, material_id, quantity, price " +
                        "FROM sales_order_item " +
                        "WHERE document_id = @orderId;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result.Load(reader);
                    }
                }
            }

            List<OrderItemDTO> items = new List<OrderItemDTO>();
            foreach (DataRow row in result.Rows)
            {
                items.Add(new OrderItemDTO
                {
                    OrderId = Convert.ToInt32(row["document_id"]),
                    Id = Convert.ToInt32(row["posnr"]),
                    MaterialId = Convert.ToInt32(row["material_id"]),
                    Quantity = Convert.ToInt32(row["quantity"]),
                    Price = Convert.ToDouble(row["price"])
                });
            }

            return items;
        }

        public void UpdateOrderStatus(int id, string status)
        {
            var query = "UPDATE sales_order_header " +
                        "SET status = @status " +
                        "WHERE document_id = @id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@status", status);

                    command.ExecuteNonQuery();
                }
            }
        }

        public int CreateOrder(OrderHeaderDTO order, List<OrderItemDTO> items)
        {
            var query = "INSERT INTO sales_order_header (sold_to, expected_delivery_date, price, status) " +
                        "VALUES (@customerId, @expectedDeliveryDate, @totalPrice, @status); " +
                        "SELECT SCOPE_IDENTITY();";
            int orderId;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@customerId", order.CustomerId);
                        command.Parameters.AddWithValue("@expectedDeliveryDate", order.ExpectedDeliveryDate);
                        command.Parameters.AddWithValue("@totalPrice", order.TotalPrice);
                        command.Parameters.AddWithValue("@status", order.Status);

                        orderId = Convert.ToInt32(command.ExecuteScalar());
                    }

                    query = "INSERT INTO sales_order_item (document_id, posnr, material_id, quantity, price) " +
                            "VALUES (@orderId, @posnr, @materialId, @quantity, @price);";
                    foreach (var item in items)
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@orderId", orderId);
                            command.Parameters.AddWithValue("@posnr", item.Id);
                            command.Parameters.AddWithValue("@materialId", item.MaterialId);
                            command.Parameters.AddWithValue("@quantity", item.Quantity);
                            command.Parameters.AddWithValue("@price", item.Price);

                            command.ExecuteNonQuery();
                        }
                    }
                    
                    transaction.Commit();
                    return orderId;
                }
            }
        }
    }
}
