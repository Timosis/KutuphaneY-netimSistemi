using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Models
{
    public class Kitap
    {
        [Key]
        public int KitapKey { get; set; }
        public int YazarKey { get; set; }
        public int YayineviKey { get; set; }
        public int KitapDurumKey { get; set; }
        public string Ad { get; set; }
        public string Isbn { get; set; }
        public int DemirbasNo { get; set; }
        public int SayfaSayisi { get; set; }
        public string KitapHakkindaOzet { get; set; }

        [ForeignKey("YazarKey")]
        public Yazar Yazar { get; set; }

        [ForeignKey("YayineviKey")]
        public Yayinevi Yayinevi { get; set; }
    }
}
