using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dukkanrzrpg.Pages.Musteriler
{
    public class EditModel : PageModel
    {
        public Musteriler Musteribilgi = new Musteriler();
        public string errorMessage = "";
        public string successMessage = "";

        public IActionResult OnGet(string id)
        {
            if (Request.Cookies["IsLoggedIn"] != "true")
            {
                return RedirectToPage("/Login");
            }

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("/Musteriler/Index");
            }

            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=crmM;Integrated Security=true;TrustServerCertificate=true;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT ID, Adsoyad, Email, Telefon, Adres, Giris FROM Musteriler WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Musteribilgi.ID = reader.GetInt32(0).ToString();
                                Musteribilgi.AdSoyad = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                Musteribilgi.Email = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                Musteribilgi.Telefon = reader.IsDBNull(3) ? "" : reader.GetString(3);
                                Musteribilgi.Adres = reader.IsDBNull(4) ? "" : reader.GetString(4);
                                Musteribilgi.Giris = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            }
                            else
                            {
                                return RedirectToPage("/Musteriler/Index");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (Request.Cookies["IsLoggedIn"] != "true")
            {
                return RedirectToPage("/Login");
            }

            Musteribilgi.ID = Request.Form["ID"]!;
            Musteribilgi.AdSoyad = Request.Form["AdSoyad"]!;
            Musteribilgi.Email = Request.Form["Email"]!;
            Musteribilgi.Telefon = Request.Form["Telefon"]!;
            Musteribilgi.Adres = Request.Form["Adres"]!;

            if (string.IsNullOrEmpty(Musteribilgi.ID) || Musteribilgi.AdSoyad.Length == 0 || 
                Musteribilgi.Email.Length == 0 || Musteribilgi.Telefon.Length == 0 || 
                Musteribilgi.Adres.Length == 0)
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
                    string Sql = "UPDATE Musteriler SET Adsoyad=@Adsoyad, Email=@Email, Telefon=@Telefon, Adres=@Adres WHERE ID=@ID";
                    using (SqlCommand command = new SqlCommand(Sql, connection))
                    {
                        command.Parameters.AddWithValue("@Adsoyad", Musteribilgi.AdSoyad);
                        command.Parameters.AddWithValue("@Email", Musteribilgi.Email);
                        command.Parameters.AddWithValue("@Telefon", Musteribilgi.Telefon);
                        command.Parameters.AddWithValue("@Adres", Musteribilgi.Adres);
                        command.Parameters.AddWithValue("@ID", Musteribilgi.ID);
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
