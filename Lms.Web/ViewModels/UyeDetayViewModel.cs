using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.ViewModels
{
    public class UyeDetayViewModel
    {
        public UyeViewModel Uye { get; set; }

        public List<UyeOduncViewModel> Oduncler { get; set; }

        public UyeDetayViewModel()
        {
            Oduncler = new List<UyeOduncViewModel>();
        }
    }
}
