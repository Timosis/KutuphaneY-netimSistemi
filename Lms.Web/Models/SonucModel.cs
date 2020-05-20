using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Models
{
    public enum SonucTip
    {
        Belirsiz,
        Basarili,
        Basarisiz
    }

    public class SonucModel : SonucModel<bool>
    {
    }

    public class SonucModel<T>
    {
        public SonucTip Tip { get; set; }
        public T Data { get; set; }
        public string HataMesaji { get; set; }
    }
}
