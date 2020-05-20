using Kys.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Repository
{
    public interface IUyeRepository : IRepository<Uye>
    {
    }
    public class UyeRepository : Repository<Uye>, IUyeRepository
    {
        public UyeRepository(KutuphaneDbContext dbContext) : base(dbContext)
        {
        }
    }
}
