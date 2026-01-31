// VibeCraft.Data/Repositories/Interfaces/IUserRepository.cs

using VibeCraft.Models.Entities;

namespace VibeCraft.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<EventPlannerUser>> GetAllPlannersAsync();
        Task<IEnumerable<RegularUser>> GetAllRegularUsersAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<bool> UserExistsAsync(string username, string email);
    }
}