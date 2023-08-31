using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IResourceFunctionService
    {
        Task<ServiceResponse> DeleteItem(int id);
        Task<ServiceResponse> GetItem(int id);
        Task<ServiceResponse> GetItems();
        Task<ServiceResponse> PostConfig(List<ResourceFunction> items);
        Task<ServiceResponse> PostItem(ResourceFunction item);
    }
}