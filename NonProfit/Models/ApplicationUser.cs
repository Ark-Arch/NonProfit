using Microsoft.AspNetCore.Identity;
using NonProfit.Entities;

namespace NonProfit.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string ? Username { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public ICollection<Session> UserSessions { get; set; }
  }
}
