using Microsoft.AspNetCore.Identity;

namespace NonProfit.Models.Responses
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
