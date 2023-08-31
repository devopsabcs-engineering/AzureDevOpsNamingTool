using AzureNaming.Tool.Helpers;
using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public class ResourceComponentByIdService : IResourceComponentByIdService
    {
        private IAdminLogService _adminLogService;

        public ResourceComponentByIdService(IAdminLogService adminLogService)
        {
            _adminLogService = adminLogService;
        }

        public async Task<ServiceResponse> GetItem(int id)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Get list of items
                var items = await ConfigurationHelper.GetList<ResourceComponent>();
                if (GeneralHelper.IsNotNull(items))
                {
                    var item = items.Find(x => x.Id == id);
                    if (GeneralHelper.IsNotNull(item))
                    {
                        serviceResponse.ResponseObject = item;
                        serviceResponse.Success = true;
                    }
                    else
                    {
                        serviceResponse.ResponseObject = "Resource Component not found!";
                    }
                }
                else
                {
                    serviceResponse.ResponseObject = "Resource Component not found!";
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                serviceResponse.Success = false;
                serviceResponse.ResponseObject = ex;
            }
            return serviceResponse;
        }
    }
}
