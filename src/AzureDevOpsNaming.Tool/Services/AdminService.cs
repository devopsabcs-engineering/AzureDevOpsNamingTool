using AzureNaming.Tool.Helpers;
using AzureNaming.Tool.Models;

namespace AzureNaming.Tool.Services
{
    public class AdminService : IAdminService
    {
        private SiteConfiguration _config = ConfigurationHelper.GetConfigurationData();

        private IAdminLogService _adminLogService;


        public AdminService(IAdminLogService adminLogService)
        {
            _adminLogService = adminLogService;
        }

        public async Task<ServiceResponse> UpdatePassword(string password)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                if (ValidationHelper.ValidatePassword(password))
                {
                    _config.AdminPassword = GeneralHelper.EncryptString(password, _config.SALTKey!);
                    await ConfigurationHelper.UpdateSettings(_config);
                    serviceResponse.Success = true;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.ResponseObject = "The pasword does not meet the security requirements.";
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

        public async Task<ServiceResponse> GenerateAPIKey()
        {
            ServiceResponse serviceResponse = new();
            try
            {
                // Set the new api key
                Guid guid = Guid.NewGuid();
                _config.APIKey = GeneralHelper.EncryptString(guid.ToString(), _config.SALTKey!);
                await ConfigurationHelper.UpdateSettings(_config);
                serviceResponse.ResponseObject = guid.ToString();
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                serviceResponse.Success = false;
                serviceResponse.ResponseObject = ex;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdateAPIKey(string apikey)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                _config.APIKey = GeneralHelper.EncryptString(apikey, _config.SALTKey!);
                await ConfigurationHelper.UpdateSettings(_config);
                serviceResponse.ResponseObject = apikey;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                serviceResponse.Success = false;
                serviceResponse.ResponseObject = ex;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse> UpdateIdentityHeaderName(string identityheadername)
        {
            ServiceResponse serviceResponse = new();
            try
            {
                _config.IdentityHeaderName = GeneralHelper.EncryptString(identityheadername, _config.SALTKey!);
                await ConfigurationHelper.UpdateSettings(_config);
                serviceResponse.ResponseObject = identityheadername;
                serviceResponse.Success = true;
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