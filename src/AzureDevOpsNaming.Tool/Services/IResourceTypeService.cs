using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IResourceTypeService
    {
        Task<ServiceResponse> DeleteItem(int id);
        List<ResourceType> GetFilteredResourceTypes(List<ResourceType> types, string filter);
        Task<ServiceResponse> GetItem(int id);
        Task<ServiceResponse> GetItems(bool admin = true);
        List<string> GetTypeCategories(List<ResourceType> types);
        Task<ServiceResponse> PostConfig(List<ResourceType> items);
        Task<ServiceResponse> PostItem(ResourceType item);
        Task<ServiceResponse> RefreshResourceTypes(bool shortNameReset = false);
        Task<ServiceResponse> UpdateTypeComponents(string operation, int componentid);
        Task<ServiceResponse> ValidateResourceTypeName(ValidateNameRequest validateNameRequest);
    }
}