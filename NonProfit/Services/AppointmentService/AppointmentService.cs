using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NonProfit.Data;
using NonProfit.Entities;
using NonProfit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NonProfit.Services.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<AppointmentService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        private Guid GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            Console.WriteLine(userId);
            throw new InvalidOperationException("User ID claim is not valid.");
        }

        private string GetUserEmail()
        {
            var emailClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(emailClaim))
            {
                throw new InvalidOperationException("Email claim is not valid.");
            }
            return emailClaim;
        }

        public async Task<Appointment> BookAppointmentAsync(Appointment appointment)
        {
            var userId = GetUserId();
            var email = GetUserEmail(); 

            _logger.LogInformation($"Booking appointment for user with email: {email}");
            _logger.LogInformation($"Booking appointment for user with ID: {userId}");

           
                appointment.UserId = new Guid();
            appointment.Email = "gbolahanlao@gmail.com";
            appointment.Status = Appointment.AppointmentStatus.Upcoming; 

            _context.Appointments.Add(appointment); 
            await _context.SaveChangesAsync();

            return appointment; 
        }


        public async Task<IEnumerable<Appointment>> GetAllAppointmentsForUserAsync()
        {
            var userId = GetUserId();
            return await _context.Appointments
                                 .Where(a => a.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<bool> RescheduleAppointmentAsync(Guid appointmentId, DateTime newDate)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null || appointment.Status == Appointment.AppointmentStatus.Cancelled)
            {
                return false;
            }

            appointment.AppointmentDate = newDate;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelAppointmentAsync(Guid appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null || appointment.Status == Appointment.AppointmentStatus.Cancelled)
            {
                return false;
            }

            appointment.Status = Appointment.AppointmentStatus.Cancelled;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid appointmentId)
        {
            return await _context.Appointments.FindAsync(appointmentId);
        }
    }
}
