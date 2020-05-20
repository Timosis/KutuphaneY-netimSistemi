using Kys.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Repository
{
    public interface IKitapOduncRepository : IRepository<KitapOdunc>
    {
    }

    public class KitapOduncRepository : Repository<KitapOdunc>,IKitapOduncRepository
    {
        public KitapOduncRepository(KutuphaneDbContext dbContext) : base(dbContext)
        {
        }
    }
}
