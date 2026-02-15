using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VibeCraft.Models.Entities;

namespace VibeCraft.Business.Interfaces
{
    public interface ISeviceService
    {
        Task<Service> CreateServiceAsync(Service service);
        Task<Service> GetServiceByIdAsync(int serviceId);
        Task<List<Service>> GetAllServicesAsync();
        Task<List<Service>> GetServicesByCategoryAsync(ServiceCategory category);
        Task<Service> UpdateServiceAsync(Service service);
        Task<bool> DeleteServiceAsync(int serviceId);

    }
}