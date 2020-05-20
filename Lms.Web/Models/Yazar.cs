using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Models
{
    public class Yazar
    {
        [Key]
        public int YazarKey { get; set; }
        public string Ad { get; set; }
        public string YazarHakkinda { get; set; }
        public ICollection<Kitap> Kitaplar { get; set; }
    }
}
