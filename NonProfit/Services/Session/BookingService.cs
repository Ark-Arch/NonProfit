using Microsoft.EntityFrameworkCore;
using NonProfit.Data;

namespace NonProfit.Services.Session
{
    public interface IBookingService
    {
        Task<Booking> BookSessionAsync(int sessionId, string userId);
        Task<IEnumerable<Booking>> GetBookingsForUserAsync(string userId);
        Task<IEnumerable<Booking>> GetBookingsForTherapistAsync(string therapistId);
    }

    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> BookSessionAsync(int sessionId, string userId)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session == null || session.IsBooked)
            {
                throw new InvalidOperationException("Session is not available.");
            }

            var booking = new Booking
            {
                SessionId = sessionId,
                UserId = userId,
                BookingTime = DateTime.UtcNow
            };

            session.IsBooked = true;
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<IEnumerable<Booking>> GetBookingsForUserAsync(string userId)
        {
            return await _context.Bookings.Where(b => b.UserId == userId).Include(b => b.Session).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsForTherapistAsync(string therapistId)
        {
            return await _context.Bookings
                .Where(b => b.Session.TherapistId == therapistId)
                .Include(b => b.Session)
                .ToListAsync();
        }
    }

}
