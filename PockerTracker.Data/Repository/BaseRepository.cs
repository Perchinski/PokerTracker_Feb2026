using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTracker.Data.Repository
{
    public abstract class BaseRepository
    {
        protected readonly ApplicationDbContext dbContext;

        protected BaseRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

    }
}
