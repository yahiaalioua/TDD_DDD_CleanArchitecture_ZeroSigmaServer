using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Common.Interfaces;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _ctx;

        public UnitOfWork(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public Task SaveChangesAsync()
        {
            return _ctx.SaveChangesAsync();
        }
    }
}
