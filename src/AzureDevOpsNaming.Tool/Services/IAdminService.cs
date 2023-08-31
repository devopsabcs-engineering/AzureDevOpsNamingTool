using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IAdminService
    {
        Task<ServiceResponse> GenerateAPIKey();
        Task<ServiceResponse> UpdateAPIKey(string apikey);
        Task<ServiceResponse> UpdateIdentityHeaderName(string identityheadername);
        Task<ServiceResponse> UpdatePassword(string password);
    }
}