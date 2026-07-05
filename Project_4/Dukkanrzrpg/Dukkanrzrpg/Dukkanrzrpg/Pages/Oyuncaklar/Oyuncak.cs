namespace Dukkanrzrpg.Pages.Oyuncaklar
{
    public class Oyuncak
    {
        public string ID { get; set; } = string.Empty;
        public string Ad { get; set; } = string.Empty;
        public string Kategori { get; set; } = string.Empty;
        public decimal Fiyat { get; set; }
        public int Stok { get; set; }
        public string Aciklama { get; set; } = string.Empty;
        public string YasGrubu { get; set; } = string.Empty;
        public string ResimUrl { get; set; } = string.Empty;
    }
}
