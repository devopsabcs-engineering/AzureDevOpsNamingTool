using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IPolicyService
    {
        Task<ServiceResponse> GetPolicy();
    }
}