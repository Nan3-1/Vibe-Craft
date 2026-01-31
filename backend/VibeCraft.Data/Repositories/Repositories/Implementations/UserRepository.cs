// VibeCraft.Data/Repositories/Implementations/UserRepository.cs

using Microsoft.EntityFrameworkCore;
using VibeCraft.Data.Repositories.Interfaces;
using VibeCraft.Models.Entities;

namespace VibeCraft.Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.CreatedEvents)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventPlannerUser>> GetAllPlannersAsync()
        {
            return await _context.Users
                .OfType<EventPlannerUser>()
                .Where(p => p.IsActive && p.IsCertified)
                .OrderByDescending(p => p.Rating)
                .ToListAsync();
        }

        public async Task<IEnumerable<RegularUser>> GetAllRegularUsersAsync()
        {
            return await _context.Users
                .OfType<RegularUser>()
                .Where(r => r.IsActive)
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                user.IsActive = false;
                await UpdateAsync(user);
            }
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username || u.Email == email);
        }
    }
}