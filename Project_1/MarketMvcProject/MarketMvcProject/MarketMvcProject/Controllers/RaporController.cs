using Microsoft.AspNetCore.Mvc;
using MarketMvcProject.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class RaporController : Controller
{
    private readonly AppDbContext _context;

    public RaporController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult UrunRapor()
    {
        var urunler = _context.Urunler
            .Include(x => x.Kategori) // JOIN
            .ToList();

        var toplamUrun = urunler.Count;
        var toplamFiyat = urunler.Sum(x => x.Fiyat);
        var enPahali = urunler.OrderByDescending(x => x.Fiyat).FirstOrDefault();
        var enUcuz = urunler.OrderBy(x => x.Fiyat).FirstOrDefault();

        var kategoriGruplu = urunler
            .GroupBy(x => x.Kategori.KategoriAdi)
            .Select(g => new
            {
                Kategori = g.Key,
                Adet = g.Count(),
                Toplam = g.Sum(x => x.Fiyat)
            }).ToList();

        ViewBag.ToplamUrun = toplamUrun;
        ViewBag.ToplamFiyat = toplamFiyat;
        ViewBag.EnPahali = enPahali;
        ViewBag.EnUcuz = enUcuz;
        ViewBag.KategoriGruplu = kategoriGruplu;

        return View(urunler);
    }
}
