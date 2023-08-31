using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IResourceUnitDeptService
    {
        Task<ServiceResponse> DeleteItem(int id);
        Task<ServiceResponse> GetItem(int id);
        Task<ServiceResponse> GetItems();
        Task<ServiceResponse> PostConfig(List<ResourceUnitDept> items);
        Task<ServiceResponse> PostItem(ResourceUnitDept item);
    }
}