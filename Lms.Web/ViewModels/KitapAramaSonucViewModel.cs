using Kys.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.ViewModels
{
    public class KitapAramaSonucViewModel
    {
        public int KitapKey { get; set; }
        public int YazarKey { get; set; }
        public int YayineviKey { get; set; }
        public int KitapDurumKey { get; set; }
        public string Ad { get; set; }
        public string Isbn { get; set; }
        public int DemirbasNo { get; set; }
        public int SayfaSayisi { get; set; }
        public string KitapHakkindaOzet { get; set; }

        public YazarViewModel Yazar { get; set; }
        public YayineviViewModel Yayinevi { get; set; }
    }
}
