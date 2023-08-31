using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IResourceProjAppSvcService
    {
        Task<ServiceResponse> DeleteItem(int id);
        Task<ServiceResponse> GetItem(int id);
        Task<ServiceResponse> GetItems();
        Task<ServiceResponse> PostConfig(List<ResourceProjAppSvc> items);
        Task<ServiceResponse> PostItem(ResourceProjAppSvc item);
    }
}