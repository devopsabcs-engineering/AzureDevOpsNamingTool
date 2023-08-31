using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IResourceEnvironmentService
    {
        Task<ServiceResponse> DeleteItem(int id);
        Task<ServiceResponse> GetItem(int id);
        Task<ServiceResponse> GetItems();
        Task<ServiceResponse> PostConfig(List<ResourceEnvironment> items);
        Task<ServiceResponse> PostItem(ResourceEnvironment item);
    }
}