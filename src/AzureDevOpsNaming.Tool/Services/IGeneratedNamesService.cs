using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public interface IGeneratedNamesService
    {
        public  Task<ServiceResponse> GetItems();

        public  Task<ServiceResponse> GetItem(int id);

        public Task<ServiceResponse> PostItem(GeneratedName generatedName);

        public Task<ServiceResponse> DeleteItem(int id);

        public Task<ServiceResponse> DeleteAllItems();

        public Task<ServiceResponse> PostConfig(List<GeneratedName> items);
    }
}
