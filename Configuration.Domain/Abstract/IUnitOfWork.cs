using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Configuration.Domain.Abstract
{
    public interface IUnitOfWork :IDisposable
    {
        IRepository<Models.Configuration> Configurations { get; }

        Task<int> SaveChangesAsync();
    }
}
