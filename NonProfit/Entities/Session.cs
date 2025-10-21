
namespace NonProfit.Entities
{

    using NonProfit.Models;

    public class Session
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; }
        public string TherapistId { get; set; }
        public ApplicationUser Therapist { get; set; }
        public bool IsBooked { get; set; }
    }
}