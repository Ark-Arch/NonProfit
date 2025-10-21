using NonProfit.DTOs;
using NonProfit.Entities;
using NonProfit.Models;

public class Booking
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public Session Session { get; set; }
    public string UserId { get; set; } 
    public ApplicationUser User { get; set; }
    public DateTime BookingTime { get; set; }

    public static implicit operator Booking(BookingDto v)
    {
        throw new NotImplementedException();
    }
}
