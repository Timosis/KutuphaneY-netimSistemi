using Kys.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Repository
{
    public interface IKategoriRepository : IRepository<Kategori>
    {
    }

    public class KategoriRepository : Repository<Kategori>, IKategoriRepository
    {
        public KategoriRepository(KutuphaneDbContext dbContext) : base(dbContext)
        {
        }
    }
}
