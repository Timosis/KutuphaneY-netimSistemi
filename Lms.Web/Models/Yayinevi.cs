using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Models
{
    public class Yayinevi
    {
        [Key]
        public int YayineviKey { get; set; }
        public string Ad { get; set; }
    }
}

