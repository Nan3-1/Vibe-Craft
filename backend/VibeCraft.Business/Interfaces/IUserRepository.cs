using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeCraft.Models.Entities;

namespace VibeCraft.Business.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        }
}