using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IResourceDelimiterService
    {
        Task<ServiceResponse> GetCurrentItem();
        Task<ServiceResponse> GetItem(int id);
        Task<ServiceResponse> GetItems(bool admin);
        Task<ServiceResponse> PostConfig(List<ResourceDelimiter> items);
        Task<ServiceResponse> PostItem(ResourceDelimiter item);
    }
}