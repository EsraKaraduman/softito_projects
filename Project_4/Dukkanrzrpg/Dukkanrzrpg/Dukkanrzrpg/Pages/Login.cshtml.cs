using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Dukkanrzrpg.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Lütfen tüm alanları doldurun.";
                return Page();
            }

            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=crmM;Integrated Security=true;TrustServerCertificate=true;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT AdSoyad FROM Kullanicis WHERE KullaniciAdi = @username AND Sifre = @password";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", Username);
                        command.Parameters.AddWithValue("@password", Password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string adSoyad = reader.GetString(0);

                                CookieOptions options = new CookieOptions
                                {
                                    Expires = DateTime.Now.AddHours(2),
                                    HttpOnly = true,
                                    Secure = true
                                };

                                Response.Cookies.Append("IsLoggedIn", "true", options);
                                Response.Cookies.Append("UserName", adSoyad, options);

                                return RedirectToPage("/Oyuncaklar/Index");
                            }
                            else
                            {
                                ErrorMessage = "Kullanıcı adı veya şifre hatalı!";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Bir hata oluştu: " + ex.Message;
            }

            return Page();
        }

        public IActionResult OnGetLogout()
        {
            Response.Cookies.Delete("IsLoggedIn");
            Response.Cookies.Delete("UserName");
            return RedirectToPage("/Login");
        }
    }
}
