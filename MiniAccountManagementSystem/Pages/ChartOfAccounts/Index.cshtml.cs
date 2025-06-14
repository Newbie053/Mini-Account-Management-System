using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MiniAccountManagementSystem.Models;
using System.Collections.Generic;
using System.Data;

using System.Data.SqlClient;


namespace MiniAccountManagementSystem.Pages.ChartOfAccounts
{
    /*public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<ChartOfAccount> Accounts { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            Accounts = new List<ChartOfAccount>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name, ParentId FROM ChartOfAccounts", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Accounts.Add(new ChartOfAccount
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                ParentId = reader.IsDBNull(2) ? null : reader.GetInt32(2)
                            });
                        }
                    }
                }
            }
        }
    }*/
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        public IndexModel(IConfiguration config) => _config = config;

        public List<Account> Accounts { get; set; } = new();

        [BindProperty]
        public Account NewAccount { get; set; }

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
                Accounts.Add(new Account
                {
                    AccountId = (int)reader["AccountId"],
                    AccountName = reader["AccountName"].ToString(),
                    ParentAccountId = reader["ParentAccountId"] as int?,
                    AccountType = reader["AccountType"].ToString()
                });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using SqlConnection conn = new(_config.GetConnectionString("DefaultConnection"));
            await conn.OpenAsync();

            using SqlCommand cmd = new("sp_ManageChartOfAccounts", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@AccountName", NewAccount.AccountName);
            cmd.Parameters.AddWithValue("@ParentAccountId", (object?)NewAccount.ParentAccountId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AccountType", NewAccount.AccountType);

            await cmd.ExecuteNonQueryAsync();

            return RedirectToPage();
        }
    }

}
