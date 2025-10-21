using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NonProfit.Data;
using NonProfit.Entities;

namespace NonProfit.Services.Session
{
    public interface ISessionService
    {
        Task<NonProfit.Entities.Session> CreateSessionAsync(NonProfit.Entities.Session session);
        Task<NonProfit.Entities.Session> GetSessionByIdAsync(int sessionId);
        Task<IEnumerable<NonProfit.Entities.Session>> GetSessionsForTherapistAsync(string therapistId);
        Task<bool> UpdateSessionAsync(NonProfit.Entities.Session session);
        Task<bool> DeleteSessionAsync(int sessionId);
    }

    public class SessionService : ISessionService
    {
        private readonly ApplicationDbContext _context;

        public SessionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NonProfit.Entities.Session> CreateSessionAsync(NonProfit.Entities.Session session)
        {
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<NonProfit.Entities.Session> GetSessionByIdAsync(int sessionId)
        {
            return await _context.Sessions.FindAsync(sessionId);
        }

        public async Task<IEnumerable<NonProfit.Entities.Session>> GetSessionsForTherapistAsync(string therapistId)
        {
            return await _context.Sessions.Where(s => s.TherapistId == therapistId).ToListAsync();
        }

        public async Task<bool> UpdateSessionAsync(NonProfit.Entities.Session session)
        {
            _context.Sessions.Update(session);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteSessionAsync(int sessionId)
        {
            var session = await GetSessionByIdAsync(sessionId);
            if (session == null) return false;

            _context.Sessions.Remove(session);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
