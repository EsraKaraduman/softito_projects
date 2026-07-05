using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Text;

namespace Dukkanrzrpg.Pages.Oyuncaklar
{
    public class IndexModel : PageModel
    {
        public List<Oyuncak> OyuncaklarListesi { get; set; } = new List<Oyuncak>();
        
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; } = string.Empty;

        public List<string> Kategoriler { get; set; } = new List<string>();

        private readonly string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=crmM;Integrated Security=true;TrustServerCertificate=true;";

        public IActionResult OnGet()
        {
            if (Request.Cookies["IsLoggedIn"] != "true")
            {
                return RedirectToPage("/Login");
            }

            LoadCategories();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT ID, Ad, Kategori, Fiyat, Stok, Aciklama, YasGrubu, ResimUrl FROM Oyuncaklar WHERE 1=1";

                    if (!string.IsNullOrEmpty(SearchTerm))
                    {
                        sql += " AND (Ad LIKE @searchTerm OR Aciklama LIKE @searchTerm)";
                    }
                    if (!string.IsNullOrEmpty(SelectedCategory))
                    {
                        sql += " AND Kategori = @category";
                    }

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (!string.IsNullOrEmpty(SearchTerm))
                        {
                            command.Parameters.AddWithValue("@searchTerm", "%" + SearchTerm + "%");
                        }
                        if (!string.IsNullOrEmpty(SelectedCategory))
                        {
                            command.Parameters.AddWithValue("@category", SelectedCategory);
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OyuncaklarListesi.Add(new Oyuncak
                                {
                                    ID = reader.GetInt32(0).ToString(),
                                    Ad = reader.GetString(1),
                                    Kategori = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                    Fiyat = reader.GetDecimal(3),
                                    Stok = reader.GetInt32(4),
                                    Aciklama = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                    YasGrubu = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                    ResimUrl = reader.IsDBNull(7) ? "" : reader.GetString(7)
                                });
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

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT DISTINCT Kategori FROM Oyuncaklar WHERE Kategori IS NOT NULL AND Kategori <> ''";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Kategoriler.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public IActionResult OnGetExportCsv()
        {
            if (Request.Cookies["IsLoggedIn"] != "true")
            {
                return RedirectToPage("/Login");
            }

            var builder = new StringBuilder();
            builder.AppendLine("ID;Oyuncak Adı;Kategori;Fiyat;Stok;Yaş Grubu Açıklaması;Açıklama");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT ID, Ad, Kategori, Fiyat, Stok, YasGrubu, Aciklama FROM Oyuncaklar";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var id = reader.GetInt32(0);
                                var ad = reader.GetString(1);
                                var kat = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                var fiyat = reader.GetDecimal(3);
                                var stok = reader.GetInt32(4);
                                var yas = reader.IsDBNull(5) ? "" : reader.GetString(5);
                                var aciklama = reader.IsDBNull(6) ? "" : reader.GetString(6);

                                builder.AppendLine($"{id};{ad};{kat};{fiyat:F2};{stok};{yas};{aciklama}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("Hata oluştu: " + ex.Message);
            }

            var csvBytes = Encoding.UTF8.GetBytes(builder.ToString());
            var bom = new byte[] { 0xEF, 0xBB, 0xBF };
            var fileBytes = new byte[bom.Length + csvBytes.Length];
            Buffer.BlockCopy(bom, 0, fileBytes, 0, bom.Length);
            Buffer.BlockCopy(csvBytes, 0, fileBytes, bom.Length, csvBytes.Length);

            return File(fileBytes, "text/csv; charset=utf-8", "Oyuncaklar.csv");
        }
    }
}
