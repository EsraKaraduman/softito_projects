using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Dukkanrzrpg.Pages.Musteriler
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public List<Musteriler> listele { get; set; } = new List<Musteriler>();

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            if (Request.Cookies["IsLoggedIn"] != "true")
            {
                return RedirectToPage("/Login");
            }

            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=crmM;Integrated Security=true;TrustServerCertificate=true;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT ID, AdSoyad, Email, Telefon, Adres, Giris FROM Musteriler";
                    
                    if (!string.IsNullOrEmpty(SearchTerm))
                    {
                        sql += " WHERE AdSoyad LIKE @searchTerm OR Email LIKE @searchTerm OR Telefon LIKE @searchTerm";
                    }

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (!string.IsNullOrEmpty(SearchTerm))
                        {
                            command.Parameters.AddWithValue("@searchTerm", "%" + SearchTerm + "%");
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Musteriler Musteri = new Musteriler
                                {
                                    ID = reader.GetInt32(0).ToString(),
                                    AdSoyad = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    Email = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                    Telefon = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                    Adres = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                    Giris = reader.IsDBNull(5) ? "" : reader.GetString(5)
                                };

                                listele.Add(Musteri);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Page();
        }
    }
}
