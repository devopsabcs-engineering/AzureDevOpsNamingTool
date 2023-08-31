using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface ICustomComponentService
    {
        Task<ServiceResponse> DeleteItem(int id);
        Task<ServiceResponse> GetItem(int id);
        Task<ServiceResponse> GetItems();
        Task<ServiceResponse> GetItemsByParentType(string parenttype);
        Task<ServiceResponse> PostConfig(List<CustomComponent> items);
        Task<ServiceResponse> PostItem(CustomComponent item);
    }
}