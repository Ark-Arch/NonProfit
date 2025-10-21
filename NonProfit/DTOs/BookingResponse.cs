namespace NonProfit.DTOs
{
    public class BookingResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Booking Booking { get; set; }
        public List<string> Errors { get; set; }
    }

    public class BookingDto
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public DateTime BookingTime { get; set; }
    }

}
    