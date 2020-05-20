using Kys.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Repository
{
    public interface IKitapRepository : IRepository<Kitap>
    {
    }
    public class KitapRepository : Repository<Kitap>, IKitapRepository
    {
        public KitapRepository(KutuphaneDbContext dbContext) : base(dbContext)
        {
        }
    }
}
