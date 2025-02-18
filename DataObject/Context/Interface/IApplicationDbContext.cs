using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObject.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataObject.Context.Interface
{
    public interface IApplicationDbContext
    {
        public DbSet<AccountUser> AccountUser { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
