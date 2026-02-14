using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeCraft.Models.DTOs;
using VibeCraft.Models.Entities;
using VibeCraft.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static VibeCraft.Models.DTOs.Login;

namespace VibeCraft.Business.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto registerDto);
        Task<string> Login(LoginDto loginDto);
        Task<UserProfileDto> GetUserProfile(int userId);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Register(RegisterDto registerDto)
        {
            
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new Exception("User with this email already exists");

            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                throw new Exception("Username is already taken");

            
            User newUser = registerDto.UserType switch
            {
                "Regular" => new RegularUser
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber
                },
                "Planner" => new EventPlannerUser
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber,
                    Bio = registerDto.Bio,
                    Specialization = registerDto.Specialization,
                    CompanyName = registerDto.CompanyName
                },
                "Admin" => new AdminUser
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber
                },
                _ => throw new Exception("Invalid user type")
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return GenerateJwtToken(newUser);
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            return GenerateJwtToken(user);
        }

        public async Task<UserProfileDto> GetUserProfile(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            return user switch
            {
                RegularUser regular => new UserProfileDto
                {
                    Id = regular.Id,
                    Username = regular.Username,
                    Email = regular.Email,
                    FirstName = regular.FirstName,
                    LastName = regular.LastName,
                    ProfilePicture = regular.ProfilePicture,
                    UserType = "Regular"
                },
                EventPlannerUser planner => new UserProfileDto
                {
                    Id = planner.Id,
                    Username = planner.Username,
                    Email = planner.Email,
                    FirstName = planner.FirstName,
                    LastName = planner.LastName,
                    ProfilePicture = planner.ProfilePicture,
                    UserType = "Planner",
                    Bio = planner.Bio,
                    Specialization = planner.Specialization,
                    Rating = planner.Rating,
                    CompletedEvents = planner.CompletedEvents,
                    CompanyName = planner.CompanyName
                },
                AdminUser admin => new UserProfileDto
                {
                    Id = admin.Id,
                    Username = admin.Username,
                    Email = admin.Email,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName,
                    ProfilePicture = admin.ProfilePicture,
                    UserType = "Admin"
                },
                _ => throw new Exception("User not found")
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserType", user.GetType().Name)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    
    }
}