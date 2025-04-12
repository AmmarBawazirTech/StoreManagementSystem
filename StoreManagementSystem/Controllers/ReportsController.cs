using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using StoreManagementSystem.Datas;

namespace StoreManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly StoreDbContext _context;

        public ReportsController(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> SalesReport(DateTime? startDate, DateTime? endDate)
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_GetSalesReport", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@StartDate", startDate ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", endDate ?? (object)DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var report = new List<SalesReport>();

                        while (await reader.ReadAsync())
                        {
                            report.Add(new SalesReport
                            {
                                OrderId = reader.GetInt32(0),
                                OrderDate = reader.GetDateTime(1),
                                CustomerName = reader.GetString(2),
                                EmployeeName = reader.GetString(3),
                                TotalAmount = reader.GetDecimal(4),
                                ItemCount = reader.GetInt32(5)
                            });
                        }

                        return View(report);
                    }
                }
            }
        }
    }

}
