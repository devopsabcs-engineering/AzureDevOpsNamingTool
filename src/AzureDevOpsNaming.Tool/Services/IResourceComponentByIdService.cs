using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IResourceComponentByIdService
    {
        Task<ServiceResponse> GetItem(int id);
    }
}