using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VibeCraft.Business.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync();
        Task<string> LoginAsync();
        Task GetUserProfileAsync();
        Task LogoutAsync();
    }

}