using System.Data.SqlClient;
using System.Data;
using StoreManagementSystem.Datas;

namespace StoreManagementSystem.Repostory
{
    public class OrderRepository
    {
        private readonly StoreDbContext _context;

        public OrderRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<int> ProcessOrderAsync(int customerId, int employeeId, List<OrderItem> items)
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();

                // Create DataTable for TVP
                var orderItemsTable = new DataTable();
                orderItemsTable.Columns.Add("ProductId", typeof(int));
                orderItemsTable.Columns.Add("Quantity", typeof(int));
                orderItemsTable.Columns.Add("Discount", typeof(decimal));

                foreach (var item in items)
                {
                    orderItemsTable.Rows.Add(item.ProductId, item.Quantity, item.Discount);
                }

                using (var command = new SqlCommand("sp_ProcessOrder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    var tvpParam = command.Parameters.AddWithValue("@OrderItems", orderItemsTable);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "OrderItemsType";

                    var orderId = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(orderId);
                }
            }
        }

        // Other order-related methods...
    }
}
