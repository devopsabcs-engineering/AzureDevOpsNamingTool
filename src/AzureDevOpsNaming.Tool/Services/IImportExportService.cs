using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IImportExportService
    {
        Task<ServiceResponse> ExportConfig(bool includeadmin = false);
        Task<ServiceResponse> PostConfig(ConfigurationData configdata);
    }
}