using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dukkanrzrpg.Pages.Musteriler
{
    public class CreateModel : PageModel
    {
        public Musteriler Musteribilgi = new Musteriler();
        public string errorMessage = "";
        public string successMessage = "";

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

            Musteribilgi.AdSoyad = Request.Form["AdSoyad"]!;
            Musteribilgi.Email = Request.Form["Email"]!;
            Musteribilgi.Telefon = Request.Form["Telefon"]!;
            Musteribilgi.Adres = Request.Form["Adres"]!;
            Musteribilgi.Giris = Request.Form["Giris"]!;

            if (Musteribilgi.AdSoyad.Length == 0 || Musteribilgi.Email.Length == 0 ||
                Musteribilgi.Telefon.Length == 0 || Musteribilgi.Adres.Length == 0 ||
                Musteribilgi.Giris.Length == 0)
            {
                errorMessage = "Tüm alanlar zorunludur.";
                return Page();
            }

            try
            {
                string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=crmM;Integrated Security=true;TrustServerCertificate=true;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string Sql = "INSERT INTO Musteriler (Adsoyad, Email, Telefon, Adres, Giris) VALUES (@Adsoyad, @Email, @Telefon, @Adres, @Giris)";
                    using (SqlCommand command = new SqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@Adsoyad", Musteribilgi.AdSoyad);
                        command.Parameters.AddWithValue("@Email", Musteribilgi.Email);
                        command.Parameters.AddWithValue("@Telefon", Musteribilgi.Telefon);
                        command.Parameters.AddWithValue("@Adres", Musteribilgi.Adres);
                        command.Parameters.AddWithValue("@Giris", Musteribilgi.Giris);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return Page();
            }

            successMessage = "Kayıt başarılı.";
            return RedirectToPage("/Musteriler/Index");
        }
    }
}