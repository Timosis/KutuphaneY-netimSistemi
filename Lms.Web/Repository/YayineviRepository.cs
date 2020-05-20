using Kys.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kys.Web.Repository
{
    public interface IYayineviRepository : IRepository<Yayinevi>
    {
    }

    public class YayineviRepository : Repository<Yayinevi>, IYayineviRepository
    {
        public YayineviRepository(KutuphaneDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
