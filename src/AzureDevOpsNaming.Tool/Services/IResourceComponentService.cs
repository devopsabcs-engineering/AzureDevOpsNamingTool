using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IResourceComponentService
    {
        Task<ServiceResponse> DeleteItem(int id);
        Task<ServiceResponse> GetItem(int id);
        Task<ServiceResponse> GetItems(bool admin);
        Task<ServiceResponse> PostConfig(List<ResourceComponent> items);
        Task<ServiceResponse> PostItem(ResourceComponent item);
    }
}