using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Dukkanrzrpg.Pages.Oyuncaklar
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Oyuncak OyuncakBilgi { get; set; } = new Oyuncak();

        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        private readonly string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=crmM;Integrated Security=true;TrustServerCertificate=true;";

        public IActionResult OnGet(string id)
        {
            if (Request.Cookies["IsLoggedIn"] != "true")
            {
                return RedirectToPage("/Login");
            }

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("/Oyuncaklar/Index");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT ID, Ad, Kategori, Fiyat, Stok, Aciklama, YasGrubu, ResimUrl FROM Oyuncaklar WHERE ID = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                OyuncakBilgi.ID = reader.GetInt32(0).ToString();
                                OyuncakBilgi.Ad = reader.GetString(1);
                                OyuncakBilgi.Kategori = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                OyuncakBilgi.Fiyat = reader.GetDecimal(3);
                                OyuncakBilgi.Stok = reader.GetInt32(4);
                                OyuncakBilgi.Aciklama = reader.IsDBNull(5) ? "" : reader.GetString(5);
                                OyuncakBilgi.YasGrubu = reader.IsDBNull(6) ? "" : reader.GetString(6);
                                OyuncakBilgi.ResimUrl = reader.IsDBNull(7) ? "" : reader.GetString(7);
                            }
                            else
                            {
                                return RedirectToPage("/Oyuncaklar/Index");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Hata oluştu: " + ex.Message;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (Request.Cookies["IsLoggedIn"] != "true")
            {
                return RedirectToPage("/Login");
            }

            if (string.IsNullOrEmpty(OyuncakBilgi.ID) || string.IsNullOrEmpty(OyuncakBilgi.Ad) || OyuncakBilgi.Fiyat <= 0 || OyuncakBilgi.Stok < 0)
            {
                ErrorMessage = "Lütfen tüm zorunlu alanları doğru şekilde doldurun.";
                return Page();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Oyuncaklar SET Ad = @ad, Kategori = @kategori, Fiyat = @fiyat, Stok = @stok, " +
                                 "Aciklama = @aciklama, YasGrubu = @yasGrubu, ResimUrl = @resimUrl WHERE ID = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ad", OyuncakBilgi.Ad);
                        command.Parameters.AddWithValue("@kategori", string.IsNullOrEmpty(OyuncakBilgi.Kategori) ? DBNull.Value : OyuncakBilgi.Kategori);
                        command.Parameters.AddWithValue("@fiyat", OyuncakBilgi.Fiyat);
                        command.Parameters.AddWithValue("@stok", OyuncakBilgi.Stok);
                        command.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(OyuncakBilgi.Aciklama) ? DBNull.Value : OyuncakBilgi.Aciklama);
                        command.Parameters.AddWithValue("@yasGrubu", string.IsNullOrEmpty(OyuncakBilgi.YasGrubu) ? DBNull.Value : OyuncakBilgi.YasGrubu);
                        command.Parameters.AddWithValue("@resimUrl", string.IsNullOrEmpty(OyuncakBilgi.ResimUrl) ? DBNull.Value : OyuncakBilgi.ResimUrl);
                        command.Parameters.AddWithValue("@id", OyuncakBilgi.ID);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Veritabanı hatası: " + ex.Message;
                return Page();
            }

            SuccessMessage = "Oyuncak başarıyla güncellendi.";
            return RedirectToPage("/Oyuncaklar/Index");
        }
    }
}
