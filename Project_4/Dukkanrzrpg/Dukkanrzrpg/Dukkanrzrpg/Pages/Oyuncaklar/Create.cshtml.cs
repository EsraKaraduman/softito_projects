using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Dukkanrzrpg.Pages.Oyuncaklar
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Oyuncak OyuncakBilgi { get; set; } = new Oyuncak();

        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            if (Request.Cookies["IsLoggedIn"] != "true")
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (Request.Cookies["IsLoggedIn"] != "true")
            {
                return RedirectToPage("/Login");
            }

            if (string.IsNullOrEmpty(OyuncakBilgi.Ad) || OyuncakBilgi.Fiyat <= 0 || OyuncakBilgi.Stok < 0)
            {
                ErrorMessage = "Lütfen oyuncak adı, fiyatı ve stok bilgilerini eksiksiz ve geçerli girin.";
                return Page();
            }

            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=crmM;Integrated Security=true;TrustServerCertificate=true;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Oyuncaklar (Ad, Kategori, Fiyat, Stok, Aciklama, YasGrubu, ResimUrl) " +
                                 "VALUES (@ad, @kategori, @fiyat, @stok, @aciklama, @yasGrubu, @resimUrl)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ad", OyuncakBilgi.Ad);
                        command.Parameters.AddWithValue("@kategori", string.IsNullOrEmpty(OyuncakBilgi.Kategori) ? DBNull.Value : OyuncakBilgi.Kategori);
                        command.Parameters.AddWithValue("@fiyat", OyuncakBilgi.Fiyat);
                        command.Parameters.AddWithValue("@stok", OyuncakBilgi.Stok);
                        command.Parameters.AddWithValue("@aciklama", string.IsNullOrEmpty(OyuncakBilgi.Aciklama) ? DBNull.Value : OyuncakBilgi.Aciklama);
                        command.Parameters.AddWithValue("@yasGrubu", string.IsNullOrEmpty(OyuncakBilgi.YasGrubu) ? DBNull.Value : OyuncakBilgi.YasGrubu);
                        command.Parameters.AddWithValue("@resimUrl", string.IsNullOrEmpty(OyuncakBilgi.ResimUrl) ? DBNull.Value : OyuncakBilgi.ResimUrl);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Veritabanı hatası: " + ex.Message;
                return Page();
            }

            SuccessMessage = "Oyuncak başarıyla eklendi.";
            return RedirectToPage("/Oyuncaklar/Index");
        }
    }
}
