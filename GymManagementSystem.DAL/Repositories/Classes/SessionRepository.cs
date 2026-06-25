using GymManagementSystem.DAL.DbContexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? predicate = null, CancellationToken ct = default)
        {
            IQueryable<Session> Query = _dbContext.Sessions.Where(predicate);
            if(predicate is not null)
            {
                Query= Query.Where(predicate);
            }
           
                return await Query.ToListAsync();
            
        }

        public Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default)
       =>_dbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId, ct);

        public Task<Session?> GetSessionWithTrainerAndCategoryAsync(int SessionId, CancellationToken ct = default)
      => _dbContext.Sessions.AsNoTracking().Include(s => s.Category).FirstOrDefaultAsync(s => s.Id == SessionId, ct);
    }
}
