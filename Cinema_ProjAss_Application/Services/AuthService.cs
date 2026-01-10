using Cinema_ProjAss_Application.DTOs.Auth;
using Cinema_ProjAss_Application.Exceptions;
using Cinema_ProjAss_Domain.Entities;
using Cinema_ProjAss_Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Cinema_ProjAss_Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<AppUser> _hasher = new();

        public AuthService(IUserRepository users, ITokenService tokenService)
        {
            _users = users;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            Validate(dto.Username, dto.Password);

            var username = dto.Username.Trim();

            var existing = await _users.GetByUsernameAsync(username);
            if (existing != null)
                throw new ValidationException("Username is already taken.");

            var user = new AppUser
            {
                Username = username,
                Role = "Customer" // ✅ Register = Kunde
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            var created = await _users.AddAsync(user);

            return new AuthResponseDto
            {
                UserId = created.Id,
                Username = created.Username,
                Role = string.IsNullOrWhiteSpace(created.Role) ? "Customer" : created.Role,
                Token = _tokenService.CreateToken(created)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            Validate(dto.Username, dto.Password);

            var username = dto.Username.Trim();

            var user = await _users.GetByUsernameAsync(username);
            if (user == null)
                throw new ValidationException("Invalid username or password.");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new ValidationException("Invalid username or password.");

            return new AuthResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Role = string.IsNullOrWhiteSpace(user.Role) ? "Customer" : user.Role, // ✅ Send rolle!
                Token = _tokenService.CreateToken(user)
            };
        }

        private static void Validate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ValidationException("Username is required.");

            if (username.Trim().Length < 3)
                throw new ValidationException("Username must be at least 3 characters.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ValidationException("Password is required.");

            if (password.Length < 6)
                throw new ValidationException("Password must be at least 6 characters.");
        }
    }
}
