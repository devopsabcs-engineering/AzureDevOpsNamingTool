using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IAdminLogService
    {
        Task<ServiceResponse> DeleteAllItems();
        Task<ServiceResponse> GetItems();
        Task<ServiceResponse> PostConfig(List<AdminLogMessage> items);
        void PostItem(AdminLogMessage adminlogMessage);
    }
}