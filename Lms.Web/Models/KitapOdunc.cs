using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Models
{
    public class KitapOdunc
    {
        [Key]
        public int KitapOduncKey { get; set; }
        public int KitapKey { get; set; }
        public int UyeKey { get; set; }
        public int KitapOduncDurum { get; set; }
        public DateTime AlisTarihi { get; set; }
        public DateTime GetirisTarihi { get; set; }

        [ForeignKey("KitapKey")]
        public Kitap Kitap { get; set;}

        [ForeignKey("UyeKey")]
        public Uye Uye { get; set; }

    }
}
