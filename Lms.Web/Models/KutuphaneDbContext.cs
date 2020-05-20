using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Models
{
    public class KutuphaneDbContext :  DbContext
    {
        public KutuphaneDbContext(DbContextOptions<KutuphaneDbContext> options): base(options){ }

        DbSet<Kategori> Kategori { get; set; }
        DbSet<Kitap> Kitap { get; set; }
        DbSet<KitapOdunc> KitapOdunc { get; set; }
        DbSet<UyeTip> UyeTip { get; set; }
        DbSet<Yayinevi> Yayinevi { get; set; }
        DbSet<Yazar> Yazar { get; set; }
        DbSet<Uye> Uye { get; set; }
    }
}
