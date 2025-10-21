namespace NonProfit.Properties.Services.AuthService
{
    using NonProfit.DTOs;
    using NonProfit.Models.Responses;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NonProfit.Models;
    using Microsoft.AspNetCore.Identity;
    using NonProfit.Services.Interfaces;

    public class AuthService : IAuthService
    {
        private readonly UserService _userService;
        private readonly TokenService _tokenService;

        public AuthService(UserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> RegisterAsync(Register model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
                RefreshToken = _tokenService.GenerateRefreshToken(),
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            var result = await _userService.CreateUserAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new AuthResponse { Success = false, Errors = result.Errors };
            }

            await _userService.AssignRoleAsync(user, UserRole.User.ToString());
            bool roleAssigned = await _userService.IsUserInRoleAsync(user, UserRole.User.ToString());

            if (roleAssigned)
            {
                Console.WriteLine($"Successfully assigned Role: {UserRole.User.ToString()} to User: {user.Email}");
            }
            else
            {
                Console.WriteLine($"Failed to assign Role: {UserRole.User.ToString()} to User: {user.Email}");
            }

            var token = _tokenService.GenerateJwtToken(user);
          

            return new AuthResponse
            {
                Success = true,
                Token = token,
                RefreshToken = user.RefreshToken
            };
        }

        public async Task<AuthResponse> RegisterAdminOrTherapistAsync(Register model, UserRole role)
        {
            var currentUser = await _userService.GetCurrentUserAsync(); 
            if (!await _userService.IsUserInRoleAsync(currentUser, UserRole.User.ToString()))
            {
                return new AuthResponse
                {
                    Success = false,
                    Errors = new List<IdentityError>
            {
                new IdentityError { Description = "You are not authorized to perform this action." }
            }
                };
            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
                RefreshToken = _tokenService.GenerateRefreshToken(),
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            var result = await _userService.CreateUserAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Errors = result.Errors
                };
            }

            await _userService.AssignRoleAsync(user, role.ToString());

            var token = _tokenService.GenerateJwtToken(user);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                RefreshToken = user.RefreshToken
            };
        }



        public async Task<AuthResponse> LoginAsync(LoginDTO model)
        {
            var user = await _userService.FindByNameAsync(model.Username);
            if (user == null || !await _userService.CheckPasswordAsync(user, model.Password))
            {
                var errors = new List<IdentityError>
                {
                    new IdentityError
                    {
                        Code = "InvalidLoginAttempt",
                        Description = "Invalid login attempt"
                    }
                };
                return new AuthResponse { Success = false, Errors = errors };
            }

            var token = _tokenService.GenerateJwtToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userService.UpdateUserAsync(user);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var user = await _userService.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                var errors = new List<IdentityError>
                {
                    new IdentityError
                    {
                        Code = "InvalidRefreshToken",
                        Description = "Invalid refresh token"
                    }
                };
                return new AuthResponse { Success = false, Errors = errors };
            }

            var newJwtToken = _tokenService.GenerateJwtToken(user);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(12);

            await _userService.UpdateUserAsync(user);

            return new AuthResponse { Success = true, Token = newJwtToken, RefreshToken = user.RefreshToken };
        }
    }
}
