using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using MiniAccountManagementSystem.Models;
using System.Data;

namespace MiniAccountManagementSystem.Pages.Vouchers
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _config;

        public CreateModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public Voucher Voucher { get; set; } = new();

        public List<Account> AccountList { get; set; } = new();

        public async Task OnGetAsync()
        {
            using SqlConnection conn = new(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using SqlCommand cmd = new("sp_GetChartOfAccounts", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                AccountList.Add(new Account
                {
                    AccountId = (int)reader["AccountId"],
                    AccountName = reader["AccountName"].ToString()
                });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int voucherId;

            using SqlConnection conn = new(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using var transaction = conn.BeginTransaction();

            try
            {
                if (string.IsNullOrWhiteSpace(Voucher.VoucherType))
                {
                    ModelState.AddModelError("Voucher.VoucherType", "Voucher Type is required.");
                    await OnGetAsync();
                    return Page();
                }


                // Insert Voucher
                using SqlCommand cmd = new("sp_SaveVoucher", conn, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@VoucherType", Voucher.VoucherType);
                cmd.Parameters.AddWithValue("@Date", Voucher.Date);
                cmd.Parameters.AddWithValue("@ReferenceNo", Voucher.ReferenceNo);

                SqlParameter outputId = new("@NewVoucherId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputId);

                await cmd.ExecuteNonQueryAsync();
                voucherId = (int)outputId.Value;

                // Insert Entries
                foreach (var entry in Voucher.Entries)
                {
                    using SqlCommand entryCmd = new("sp_SaveVoucherEntry", conn, transaction)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    entryCmd.Parameters.AddWithValue("@VoucherId", voucherId);
                    entryCmd.Parameters.AddWithValue("@AccountId", entry.AccountId);
                    entryCmd.Parameters.AddWithValue("@Debit", entry.Debit);
                    entryCmd.Parameters.AddWithValue("@Credit", entry.Credit);

                    await entryCmd.ExecuteNonQueryAsync();
                }

                transaction.Commit();
                return RedirectToPage("/Vouchers/Details", new { id = voucherId });

            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

}
