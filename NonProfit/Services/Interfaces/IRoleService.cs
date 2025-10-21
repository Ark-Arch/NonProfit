namespace NonProfit.Services.Interfaces
{
    public interface IRoleService
    {
        Task InitializeRolesAsync();
        Task<bool> AssignRoleAsync(string email, string role);
    }
}
