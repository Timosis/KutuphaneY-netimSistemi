using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Models
{
    public class UyeTip
    {
        [Key]
        public int UyeTipKey { get; set; }
        public string Ad { get; set; }
    }
}
