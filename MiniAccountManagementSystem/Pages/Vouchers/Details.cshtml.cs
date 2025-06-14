using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace MiniAccountManagementSystem.Pages.Vouchers
{
    public class DetailsModel : PageModel
    {
        private readonly IConfiguration _config;

        public DetailsModel(IConfiguration config)
        {
            _config = config;
        }

        public VoucherDetailsDto Voucher { get; set; } = new();
        public List<EntryDto> Entries { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using SqlConnection conn = new(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using SqlCommand cmd = new("sp_GetVoucherDetails", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@VoucherId", id);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                if (Voucher.VoucherId == 0)
                {
                    Voucher.VoucherId = id;
                    Voucher.VoucherType = reader["VoucherType"].ToString();
                    Voucher.Date = Convert.ToDateTime(reader["Date"]);
                    Voucher.ReferenceNo = reader["ReferenceNo"].ToString();
                }

                Entries.Add(new EntryDto
                {
                    EntryId = (int)reader["EntryId"],
                    AccountName = reader["AccountName"].ToString(),
                    Debit = Convert.ToDecimal(reader["Debit"]),
                    Credit = Convert.ToDecimal(reader["Credit"])
                });
            }

            if (Voucher.VoucherId == 0)
                return NotFound();

            return Page();
        }

        public class VoucherDetailsDto
        {
            public int VoucherId { get; set; }
            public string VoucherType { get; set; }
            public DateTime Date { get; set; }
            public string ReferenceNo { get; set; }
        }

        public class EntryDto
        {
            public int EntryId { get; set; }
            public string AccountName { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
        }
    }
}
