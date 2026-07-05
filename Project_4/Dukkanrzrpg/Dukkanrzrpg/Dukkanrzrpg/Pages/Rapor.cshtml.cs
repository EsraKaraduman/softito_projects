using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Dukkanrzrpg.Pages
{
    public class RaporModel : PageModel
    {
        public int ToplamOyuncak { get; set; }
        public int ToplamMusteri { get; set; }
        public int KritikStokSayisi { get; set; }
        public decimal ToplamStokDegeri { get; set; }

        public List<KategoriRaporu> KategoriRaporListesi { get; set; } = new List<KategoriRaporu>();

        private readonly string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=crmM;Integrated Security=true;TrustServerCertificate=true;";

        public IActionResult OnGet()
        {
            if (Request.Cookies["IsLoggedIn"] != "true")
            {
                return RedirectToPage("/Login");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Oyuncaklar", connection))
                    {
                        ToplamOyuncak = (int)cmd.ExecuteScalar()!;
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Musteriler", connection))
                    {
                        ToplamMusteri = (int)cmd.ExecuteScalar()!;
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Oyuncaklar WHERE Stok < 10", connection))
                    {
                        KritikStokSayisi = (int)cmd.ExecuteScalar()!;
                    }

                    using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(Stok * Fiyat), 0) FROM Oyuncaklar", connection))
                    {
                        ToplamStokDegeri = (decimal)cmd.ExecuteScalar()!;
                    }

                    string categorySql = "SELECT ISNULL(Kategori, 'Kategorisiz'), COUNT(*), SUM(Stok) FROM Oyuncaklar GROUP BY Kategori";
                    using (SqlCommand cmd = new SqlCommand(categorySql, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                KategoriRaporListesi.Add(new KategoriRaporu
                                {
                                    KategoriAdi = reader.GetString(0),
                                    OyuncakSayisi = reader.GetInt32(1),
                                    ToplamStok = reader.IsDBNull(2) ? 0 : reader.GetInt32(2)
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
    }

    public class KategoriRaporu
    {
        public string KategoriAdi { get; set; } = string.Empty;
        public int OyuncakSayisi { get; set; }
        public int ToplamStok { get; set; }
    }
}
