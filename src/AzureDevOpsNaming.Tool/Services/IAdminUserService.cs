using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IAdminUserService
    {
        Task<ServiceResponse> DeleteItem(int id);
        Task<ServiceResponse> GetItem(string name);
        Task<ServiceResponse> GetItems();
        Task<ServiceResponse> PostConfig(List<AdminUser> items);
        Task<ServiceResponse> PostItem(AdminUser item);
    }
}