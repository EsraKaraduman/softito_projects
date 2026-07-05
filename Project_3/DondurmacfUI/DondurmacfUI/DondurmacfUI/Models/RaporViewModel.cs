using System.Collections.Generic;

namespace DondurmacfUI.Models
{
    public class Rapor1Model
    {
        public string TurAdi { get; set; } = string.Empty;
        public int Fiyat { get; set; }
        public string UrunAdi { get; set; } = string.Empty;
    }

    public class Rapor2Model
    {
        public string TurAdi { get; set; } = string.Empty;
        public int Fiyat { get; set; }
        public string Olusturan { get; set; } = string.Empty;
    }

    public class Rapor3Model
    {
        public string TurAdi { get; set; } = string.Empty;
        public int Fiyat { get; set; }
        public string UrunAdi { get; set; } = string.Empty;
        public string Olusturan { get; set; } = string.Empty;
    }

    public class Rapor4Model
    {
        public string UrunAdi { get; set; } = string.Empty;
        public int TurSayisi { get; set; }
        public double OrtalamaFiyat { get; set; }
    }

    public class Rapor5Model
    {
        public string TurAdi { get; set; } = string.Empty;
        public int Fiyat { get; set; }
        public string UrunAdi { get; set; } = string.Empty;
    }

    public class RaporViewModel
    {
        public List<Rapor1Model> Rapor1 { get; set; } = new();
        public List<Rapor2Model> Rapor2 { get; set; } = new();
        public List<Rapor3Model> Rapor3 { get; set; } = new();
        public List<Rapor4Model> Rapor4 { get; set; } = new();
        public List<Rapor5Model> Rapor5 { get; set; } = new();
    }
}
