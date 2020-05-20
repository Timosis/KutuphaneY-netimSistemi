using Kys.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.ViewModels
{
    public class UyeOduncViewModel
    {
        public int OduncKey { get; set; }
        public KitapViewModel OduncKitap { get; set; }

        public KitapDurum OduncKitapDurum { get; set; }

    }
}
