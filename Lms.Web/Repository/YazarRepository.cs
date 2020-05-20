using Kys.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Repository
{

    public interface IYazarRepository : IRepository<Yazar>
    {
    }

    public class YazarRepository : Repository<Yazar>, IYazarRepository
    {
        public YazarRepository(KutuphaneDbContext dbContext) : base(dbContext)
        {
        }
    }
}
