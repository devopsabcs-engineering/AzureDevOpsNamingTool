using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IResourceOrgService
    {
        Task<ServiceResponse> DeleteItem(int id);
        Task<ServiceResponse> GetItem(int id);
        Task<ServiceResponse> GetItems();
        Task<ServiceResponse> PostConfig(List<ResourceOrg> items);
        Task<ServiceResponse> PostItem(ResourceOrg item);
    }
}