using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Models
{
    public class Kategori
    {
        [Key]
        public int KategoriKey { get; set; }
        public string Ad { get; set; }
    }
}
