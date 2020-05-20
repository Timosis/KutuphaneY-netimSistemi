using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.ViewModels
{
    public class KitapKayitViewModel
    {
        public int? KitapKey { get; set; }
        public int KitapDurumKey { get; set; }
        public string Ad { get; set; }
        public string Isbn { get; set; }
        public string SayfaSayisi { get; set; }
        public string KitapHakkindaOzet { get; set; }
        public YazarViewModel Yazar { get; set; }
        public YayineviViewModel Yayinevi { get; set; }
    }
}
