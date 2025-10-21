using NonProfit.Models;

namespace NonProfit.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Message { get; set; }
        public AppointmentStatus Status { get; set; }
        public Guid UserId { get; set; }

        public enum AppointmentStatus
        {
            Upcoming,
            Completed,
            Cancelled
        }
    }

}
