using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SiteMovie.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> Commit();
    }
}
