using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace MiniAccountManagementSystem.Pages.Vouchers
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;

        public IndexModel(IConfiguration config)
        {
            _config = config;
        }

        public List<VoucherSummary> Vouchers { get; set; } = new();

        public async Task OnGetAsync()
        {
            using SqlConnection conn = new(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using SqlCommand cmd = new("sp_GetVoucherList", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Vouchers.Add(new VoucherSummary
                {
                    VoucherId = (int)reader["VoucherId"],
                    VoucherType = reader["VoucherType"].ToString(),
                    Date = Convert.ToDateTime(reader["Date"]),
                    ReferenceNo = reader["ReferenceNo"].ToString(),
                    TotalDebit = Convert.ToDecimal(reader["TotalDebit"]),
                    TotalCredit = Convert.ToDecimal(reader["TotalCredit"])
                });
            }
        }

        public class VoucherSummary
        {
            public int VoucherId { get; set; }
            public string VoucherType { get; set; }
            public DateTime Date { get; set; }
            public string ReferenceNo { get; set; }
            public decimal TotalDebit { get; set; }
            public decimal TotalCredit { get; set; }
        }
    }
}
