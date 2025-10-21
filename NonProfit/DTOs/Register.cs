namespace NonProfit.DTOs
{
    public record Register(
     string Firstname,
     string Lastname,
     string Email,
     string Password,
    UserRole Role = UserRole.User 
 );

}


public enum UserRole
{
    User,
    Admin,
    Therapist
}
