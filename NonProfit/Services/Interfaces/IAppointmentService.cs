using NonProfit.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NonProfit.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointment> BookAppointmentAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAllAppointmentsForUserAsync();
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<bool> RescheduleAppointmentAsync(Guid appointmentId, DateTime newDate);
        Task<bool> CancelAppointmentAsync(Guid appointmentId);
        Task<Appointment> GetAppointmentByIdAsync(Guid appointmentId);
    }
}
