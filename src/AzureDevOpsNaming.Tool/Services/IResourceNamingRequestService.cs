using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IResourceNamingRequestService
    {
        Task<ResourceNameResponse> RequestName(ResourceNameRequest request);
        Task<ResourceNameResponse> RequestNameWithComponents(ResourceNameRequestWithComponents request);
    }
}