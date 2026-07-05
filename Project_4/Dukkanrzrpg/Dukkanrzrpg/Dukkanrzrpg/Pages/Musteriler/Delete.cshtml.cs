using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Dukkanrzrpg.Pages.Musteriler
{
    public class DeleteModel : PageModel
    {
        public IActionResult OnGet(string id)
        {
            if (Request.Cookies["IsLoggedIn"] != "true")
            {
                return RedirectToPage("/Login");
            }

            if (!string.IsNullOrEmpty(id))
            {
                string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=crmM;Integrated Security=true;TrustServerCertificate=true;";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "DELETE FROM Musteriler WHERE ID = @ID";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@ID", id);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return RedirectToPage("/Musteriler/Index");
        }
    }
}
