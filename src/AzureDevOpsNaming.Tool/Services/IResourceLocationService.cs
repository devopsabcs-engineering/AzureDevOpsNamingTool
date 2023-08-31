using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IResourceLocationService
    {
        Task<ServiceResponse> DeleteItem(int id);
        Task<ServiceResponse> GetItem(int id);
        Task<ServiceResponse> GetItems(bool admin = true);
        Task<ServiceResponse> PostConfig(List<ResourceLocation> items);
        Task<ServiceResponse> PostItem(ResourceLocation item);
        Task<ServiceResponse> RefreshResourceLocations(bool shortNameReset = false);
    }
}